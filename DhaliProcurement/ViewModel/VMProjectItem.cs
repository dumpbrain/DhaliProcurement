using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DhaliProcurement.Models;

namespace DhaliProcurement.ViewModel
{
    public class VMProjectItem
    {

        public List<ProcProjectItem> ProcProjectItem { get; set; }
        public Unit Unit { get; set; }
        public Item Item { get; set; }

        public int ProjectId { get; set; }
        public int SiteId { get; set; }

        //public int ISLNO { get; set; }
        //public int USLNO { get; set; }
        //public string ItemName { get; set; }
        //public string UnitName { get; set; }
        //public int PQuantity { get; set; }
        //public int PCost { get; set; }
        //public string ItemRemarks { get; set; }

    }
}