using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class VMPurchaseOrder
    {    
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal POQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Size { get; set; }

    }
}