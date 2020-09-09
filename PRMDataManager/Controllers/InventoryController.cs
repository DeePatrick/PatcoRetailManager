using PRMDataManager.Library.Models;
using PRMDataManager.Library.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace PRMDataManager.Controllers
{

   [Authorize]
    public class InventoryController : ApiController
    {
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            return data.GetInventory();
        }
        public void Post(InventoryModel Inventory)
        {
            InventoryData data = new InventoryData();
            //string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveInventory(Inventory);
        }
    }
}
