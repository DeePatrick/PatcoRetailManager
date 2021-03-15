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
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sql;

        public InventoryData(IConfiguration config, ISqlDataAccess sql)
        {
            _config = config;
            _sql = sql;
        }
        public List<InventoryModel> GetInventory()
        {
            var output = _sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "PRMData");
            return output;
        }

        public void SaveInventory(InventoryModel item)
        {
            _sql.SaveData("[dbo].[spInventory_Insert]", item, "PRMData");
        }

    }
}

