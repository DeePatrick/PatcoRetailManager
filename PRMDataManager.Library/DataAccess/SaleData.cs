using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PRMDataManager.Library.Internal.DataAccess;
using PRMDataManager.Library.Models;

namespace PRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess _sql;
        private readonly IProductData _productData;
        public SaleData(ISqlDataAccess sql, IProductData productData)
        {
            _sql = sql;
            _productData = productData;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {

            // Start filling in the sales detail models with save to database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get information about the product sold

                var productInfo = _productData.GetProductById(detail.ProductId);

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

            try
            {
                _sql.StartTransaction("PRMData");
                // Save the sale model
                _sql.SaveDataInTransaction("[dbo].[spSale_Insert]", sale);
                // Get the Id from the sale Model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("[dbo].[spSale_Lookup]", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                // Finish filling in the sale detail model
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    // save the sale detail model
                    _sql.SaveDataInTransaction("[dbo].[spSaleDetail_Insert]", item);
                }

                _sql.CommitTransaction();
            }
            catch
            {
                _sql.RollBackTransaction();
                throw;
            }


        }

        public List<SaleReportModel> GetSaleReports()
        {
            var output = _sql.LoadData<SaleReportModel, dynamic>("[dbo].[spSale_SaleReport]", new { }, "PRMData");
            return output;
        }
    }
}




