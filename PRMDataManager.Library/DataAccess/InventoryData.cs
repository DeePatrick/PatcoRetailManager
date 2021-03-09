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
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData()
        {

        }
        public InventoryData(IConfiguration config)
        {
            _config = config;
        }
        public List<InventoryModel> GetInventory()
        {

            SqlDataAccess sql = new SqlDataAccess(_config);
            var output = sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "PRMData");
            return output;
        }

        public void SaveInventory(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            sql.SaveData("[dbo].[spInventory_Insert]", item, "PRMData");
        }

    }
}

