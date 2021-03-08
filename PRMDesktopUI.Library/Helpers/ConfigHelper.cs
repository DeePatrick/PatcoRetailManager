using System.Configuration;


namespace PRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO: Move this fromconfig to API
        public decimal GetTaxRate()
        {

            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);
            if (IsValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("tax rate not setup properly");
            }
            return output;
        }
    }
}
