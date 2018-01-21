using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.ViewModel
{
    public class VMVenderPayment
    {
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public int ReqNo { get; set; }
        public string PONo { get; set; }
        public int ItemId { get; set; }
        public decimal ReqQty { get; set; }
        public int UnitId { get; set; }
        public string ChallanNo { get; set; }
        public string BillNo { get; set; }
        public decimal Payment { get; set; }
        public string Remarks { get; set; }
        public int Proc_MaterialEntryDetId { get; set; }


    }
}