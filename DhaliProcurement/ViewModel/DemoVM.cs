using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class DemoVM
    {
        public int ItemISLNO { get; set; }
        public int UnitUSLNO { get; set; }
        public int PQuantity { get; set; }
        public int PCost { get; set; }
        public string Remarks { get; set; }


        public int ProcProjectId { get; set; }
        public int ProjectId { get; set; }
        public int ProjectSiteId { get; set; }
        //public DateTime? BOQDate { get; set; }
        //public DateTime? NOADate { get; set; }
        //public string BOQNo { get; set; }
        //public string NOANo { get; set; }
        //public string ProjectStatus { get; set; }
        //public string ProjectRemarks { get; set; }
        //public string ProjectType { get; set; }

        //public string ItemName { get; set; }
        //public string UnitName { get; set; }


    }
}