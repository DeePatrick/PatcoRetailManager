﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMDataManager.Library.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        public string RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
    }
}
