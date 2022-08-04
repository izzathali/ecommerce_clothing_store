﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ViewModel
{
    public class ShoppingCartVM
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}
