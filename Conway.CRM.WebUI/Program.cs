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

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapControllers();
app.MapBlazorHub();

app.MapHub<EditNotificationHub>("/editNotificationHub");

app.MapFallbackToPage("/_Host");

app.Run();