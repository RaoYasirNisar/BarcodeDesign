using System;
using System.Web.Services;

namespace BarcodeDesign
{
    public partial class BarcodeScanner : System.Web.UI.Page
    {
        // Define a class to represent the response data
        public class BarcodeValidationResponse
        {
            public bool exists { get; set; }
            public string assetInfo { get; set; }
        }

        // Web method to validate the scanned barcode
        [WebMethod]
        public static BarcodeValidationResponse ValidateBarcode(string barcode)
        {
            // Simulate database check
            bool assetExists = CheckIfAssetExists(barcode);
            string assetInfo = "";

            if (assetExists)
            {
                // Simulate retrieving asset information from the database
                assetInfo = GetAssetInformation(barcode);
            }

            // Create and return the validation response
            return new BarcodeValidationResponse
            {
                exists = assetExists,
                assetInfo = assetInfo
            };
        }

        // Simulated method to check if the asset exists in the database
        private static bool CheckIfAssetExists(string barcode)
        {
            // Your logic to check if the asset exists in the database
            // For demonstration purposes, we'll just return true if the barcode is not empty
            return !string.IsNullOrEmpty(barcode);
        }

        // Simulated method to retrieve asset information from the database
        private static string GetAssetInformation(string barcode)
        {
            // Your logic to retrieve asset information from the database based on the barcode
            // For demonstration purposes, we'll just return a dummy asset information
            return "Asset Information for barcode: " + barcode;
        }
    }
}
