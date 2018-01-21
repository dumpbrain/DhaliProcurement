using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class VMTenderItem
    {
        public int TenderDetailId { get; set; }
        public int Proc_RequisitionDetId { get; set; }
        public int VendorId { get; set; }
        public string TQDate { get; set; }
        public string TQNo { get; set; }
        public decimal TQPrice { get; set; }
        public string Status { get; set; }
    }

    public class VMTenderMasterDetail
    {
        public Proc_TenderMas proc_TenderMas { get; set; }
        public ICollection<Proc_TenderDet> proc_TenderDet { get; set; }
    }

    public class VMEditTenderItem
    {
        public int TenderDetailId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SiteId { get; set; }
        public string SitetName { get; set; }
        public string RCode { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Proc_RequisitionDetId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal Qty { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string TQDate { get; set; }
        public string TQNo { get; set; }
        public decimal TQPrice { get; set; }
        public string Status { get; set; }
    }

    public class VMPendingTender {
        public string TQNo { get; set; }
        public string TDate { get; set; }
        public string Status { get; set; }
    }
}