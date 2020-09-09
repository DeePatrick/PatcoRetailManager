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
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> Get()
        {
            SaleData data = new SaleData();
            return data.GetSaleReports();
        }
    }
}

