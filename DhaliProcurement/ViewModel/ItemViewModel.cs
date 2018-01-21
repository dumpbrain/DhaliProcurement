using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class ItemViewModel
    {


        public int ProjectId { get; set; }
        public int ProjectSiteId { get; set; }
        public int ItemISLNO { get; set; }
        public int UnitUSLNO { get; set; }
        public decimal PQuantity { get; set; }
        public decimal PCost { get; set; }
        public string Remarks { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}