namespace Conway.CRM.WebUI.Helpers
{
    public static class ValidationHelper
    {
        public static string GetValidationClass(string propertyName, Dictionary<string, string> validationErrors)
        {
            return validationErrors.ContainsKey(propertyName) ? "field-validation-error" : string.Empty;
        }
    }
}
