using Microsoft.Extensions.Configuration;
using PRMDataManager.Library.Internal.DataAccess;
using PRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMDataManager.Library.DataAccess
{
    public class ProductData
    {
        private readonly IConfiguration _config;
        public ProductData()
        {

        }
        public ProductData(IConfiguration config)
        {
            _config = config;
        }
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "PRMData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var output = sql.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetById]", new { Id = productId}, "PRMData").FirstOrDefault();
            return output;
        }
    }
}



