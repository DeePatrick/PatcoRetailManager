using PRMDataManager.Library.Models;
using PRMDataManager.Library.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PRMDataManager.Controllers
{
   [Authorize(Roles ="Cashier")]
    public class ProductController : ApiController
    {
        // GET api/products
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();
            return data.GetProducts();
        }

    }
}

