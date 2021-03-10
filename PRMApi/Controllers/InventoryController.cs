using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRMDataManager.Library.Models;
using PRMDataManager.Library.DataAccess;
using Microsoft.Extensions.Configuration;

namespace PRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager, Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(_config);
            return data.GetInventory();
        }
        [HttpPost]
        public void Post(InventoryModel Inventory)
        {
            InventoryData data = new InventoryData(_config);
            data.SaveInventory(Inventory);
        }
    }
}
