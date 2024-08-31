using Conway.CRM.Application.Interfaces;
using Conway.CRM.Application.Services;
using Conway.CRM.Domain.Validations;
using Conway.CRM.Infrastructure.Persistence;
using Conway.CRM.Infrastructure.Repositories;
using Conway.CRM.WebUI.Hubs;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Conway.CRM.Domain.Entities.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Configure logging 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
var logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

// Log at the start of the application
logger.LogInformation("Starting the application...");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "Conway.CRM.WebUITheme";
    options.Duration = TimeSpan.FromDays(365);
});

builder.Services.AddDbContext<CRMDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);

        options.Events.OnTokenValidated = async context =>
        {
            try
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("OnTokenValidated event triggered");

                var dbContext = context.HttpContext.RequestServices.GetRequiredService<CRMDbContext>();

                var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var userEmail = context.Principal.FindFirstValue(ClaimTypes.Email);
                var userName = context.Principal.Identity.Name;

                logger.LogInformation($"User ID: {userId}, Email: {userEmail}, Name: {userName}");

                var user = await dbContext.Users.SingleOrDefaultAsync(u => u.AzureAdUserId == userId);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        AzureAdUserId = userId,
                        DisplayName = userName,
                        Email = userEmail
                    };

                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation("User added to the database");
                }
            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while adding the user to the database.");
            }
        };
    });





builder.Services.AddValidatorsFromAssemblyContaining<OpportunityValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
// Register Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<IOpportunityRepository, OpportunityRepository>();
builder.Services.AddScoped<IStageRepository, StageRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonStatusRepository, PersonStatusRepository>();
builder.Services.AddScoped<IOpportunityStatusChangeRepository, OpportunityStatusChangeRepository>();
// Register Services
builder.Services.AddSingleton<LockingService>();
// SignalR
builder.Services.AddSignalR();
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddHttpClient("Conway.CRM.WebUI").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddScoped<AuthenticationStateProvider, Conway.CRM.WebUI.ApplicationAuthenticationStateProvider>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Conway.CRM.WebUI.SecurityService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapHub<EditNotificationHub>("/editNotificationHub");
app.MapFallbackToPage("/_Host");
app.Run();