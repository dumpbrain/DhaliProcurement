using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class VMRequisitionItem
    {
        //public int ProcRequisitionMasId { get; set; }
        public int ItemId { get; set; }
        public decimal ReqQty { get; set; }
        public decimal CStockQty { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string ItemRemarks { get; set; }
        public DateTime? RequiredDate { get; set; }
    }

    public class VMRequisitionMaster {
        public int ProcProjectId { get; set; }
        public string ProcProjectName { get; set; }
        public string Rcode { get; set; }
        public DateTime ReqDate { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }


        public class VMRequisitionMasterDetail
    {
        public Proc_RequisitionMas requisitionMaster { get; set; }
        public ICollection<Proc_RequisitionDet> requisitionDetail { get; set; }
    }

}