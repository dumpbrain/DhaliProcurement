using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class VMMaterialsEntry
    {
        public int ItemId { get; set; }
        public string RCode { get; set; }
        public string PONo { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public decimal EntryQty { get; set; }
        public string Status { get; set; }
    }

    public class VMMaterialsEntryMasterDetail
    {
        public Proc_MaterialEntryMas proc_MaterialEntryMas { get; set; }
        public ICollection<Proc_MaterialEntryDet> proc_MaterialEntryDet { get; set; }
    }

    public class VMEditMaterialsEntryItem
    {
        public int Proc_MaterialEntryDetId { get; set; }       
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SiteId { get; set; }
        public string SitetName { get; set; }
        public string RCode { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string PONo { get; set; }
        public int PurchaseOrderDetId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal TotalMaterial { get; set; }
        public decimal RemainingQty { get; set; }
        public decimal UnitPrice { get; set; }
        public string ChallanNo { get; set; }
        //public DateTime? ChallanDate { get; set; }
        public string ChallanDate { get; set; }
        public decimal EntryQty { get; set; }
        public string Status { get; set; }
    }
}