using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;

namespace Conway.CRM.WebUI.Pages.FileImport
{
    public partial class ImportOpportunities : ComponentBase
    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        protected string ImportResult { get; set; }
        protected string ErrorMessage { get; set; }
        private IBrowserFile UploadedFile { get; set; }

        protected void OnInputFileChange(InputFileChangeEventArgs e)
        {
            UploadedFile = e.File;
            ErrorMessage = string.Empty;
        }

        protected async Task ProcessFile()
        {
            if (UploadedFile == null)
            {
                ErrorMessage = "Please select a valid Excel file.";
                return;
            }

            try
            {
                var opportunities = new List<Opportunity>();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                {
                    await UploadedFile.OpenReadStream().CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets["Opportunities"];
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var accMgrId = worksheet.Cells[row, 1].Text;

                            var opportunity = new Opportunity
                            {
                                Id = new Guid(),
                                AccountManagerId = Guid.Parse(worksheet.Cells[row, 1].Text),
                                AggregatesVolume = int.Parse(worksheet.Cells[row, 2].Text),
                                AsphaltVolume = int.Parse(worksheet.Cells[row, 3].Text),
                                Site = worksheet.Cells[row, 4].Text,
                                Comments = worksheet.Cells[row, 5].Text,
                                ExpectedCloseDate = DateTime.Now.AddDays(30),
                                ExpectedStartDate = DateTime.Now.AddDays(60),
                                CustomerId = Guid.Parse(worksheet.Cells[row, 6].Text),
                                StageId = Guid.Parse(worksheet.Cells[row, 8].Text),
                                NextChaseDate = DateTime.UtcNow.AddDays(30)
                            };

                            opportunities.Add(opportunity);
                            await OpportunityRepository.AddOpportunityAsync(opportunity);
                        }
                    }
                }

                var result = string.Empty;
                foreach (var opportunity in opportunities)
                {
                    result += GenerateInsertStatement(opportunity) + Environment.NewLine;
                }

                ImportResult = result;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while processing the file: {ex.Message}";
            }
        }

        public string GenerateInsertStatement(Opportunity opportunity)
        {
            return $@"
            INSERT INTO Opportunities (Id, AccountManagerId, AggregatesVolume, AsphaltVolume, Site, Comments, EstimatedValue, ExpectedCloseDate, ExpectedStartDate, CustomerId, StageId, CreatedAt, NextChaseDate)
            VALUES ('{opportunity.Id}', 
                    '{opportunity.AccountManagerId}', 
                    {opportunity.AggregatesVolume}, 
                    {opportunity.AsphaltVolume}, 
                    '{opportunity.Site}', 
                    '{opportunity.Comments}', 
                    {opportunity.EstimatedValue}, 
                    '{opportunity.ExpectedCloseDate:yyyy-MM-dd HH:mm:ss}', 
                    '{opportunity.ExpectedStartDate:yyyy-MM-dd HH:mm:ss}', 
                    '{opportunity.CustomerId}', 
                    '{opportunity.StageId}', 
                    '{opportunity.CreatedAt:yyyy-MM-dd HH:mm:ss}', 
                    '{opportunity.NextChaseDate:yyyy-MM-dd HH:mm:ss}')";
        }
    }
}
