using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRMDataManager.Library.Internal.DataAccess;
using PRMDataManager.Library.Models;

namespace PRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {

            // Start filling in the sales detail models with save to database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get information about the product sold

                var productInfo = products.GetProductById(detail.ProductId);

                // Fill in the available information
                if (productInfo == null)
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                else
                {
                    detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                    if (productInfo.IsTaxable) 
                        detail.Tax = productInfo.RetailPrice * detail.Quantity * taxRate;
                    else
                        detail.Tax = 0;

                }

                details.Add(detail);

            }

            // Create the sale model 
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            // Save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("[dbo].[spSale_Insert]",sale, "PRMData");


            // Get the Id from the sale Model
            sale.Id = sql.LoadData<int, dynamic>("[dbo].[spSale_Lookup]", new { sale.CashierId, sale.SaleDate }, "PRMData").FirstOrDefault();

            // Finish filling in the sale detail model
            foreach( var item in details)
            {
                item.SaleId = sale.Id;
                // save the sale detail model
                sql.SaveData("[dbo].[spSaleDetail_Insert]", item, "PRMData");
            }
           
        }
    }
}




