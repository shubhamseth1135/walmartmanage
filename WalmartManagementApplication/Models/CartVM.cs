using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WalmartManagementApplication.Models
{
    public class CartVM
    {
        public int productid { get; set; }
        public string productname { get; set; }
        public int qty { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public double netprice { get; set; }
        public double totalamount { get; set; }
    }
}