using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_MaterialEntryDet
    {
        public int Id { get; set; }
        public int Proc_MaterialEntryMasId { get; set; }
        public virtual Proc_MaterialEntryMas Proc_MaterialEntryMas { get; set; }
        public int Proc_PurchaseOrderDetId { get; set; }
        public virtual Proc_PurchaseOrderDet Proc_PurchaseOrderDet { get; set; }
        [Required]
        [StringLength(50)]
        [Index("ChallanNo", IsUnique = true)]
        public string ChallanNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ChallanDate { get; set; }
        public decimal EntryQty { get; set; }
        [Required]
        [StringLength(1)]
        public string Status { get; set; }
    }
}