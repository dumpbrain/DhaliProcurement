using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_PurchaseOrderMas
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string PONo { get; set; }
        [Required]
        public DateTime PODate { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public int Proc_TenderMasId { get; set; }
        public virtual Proc_TenderMas Proc_TenderMas { get; set; }
        public int LeadTime { get; set; }
        [StringLength(100)]
        public string OrderTo { get; set; }
        [StringLength(100)]
        public string Attention { get; set; }
        [StringLength(100)]
        public string AttnCell { get; set; }
        [StringLength(100)]
        public string AttnEmail { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(800)]
        public string Content { get; set; }
        [StringLength(100)]
        public string RecvConcernPerson { get; set; }
        [StringLength(100)]
        public string RecvConcernPersonCell { get; set; }
        public decimal POTotalAmt { get; set; }

    }
}