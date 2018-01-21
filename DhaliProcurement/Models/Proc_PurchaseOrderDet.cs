using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_PurchaseOrderDet
    {
        [Key]
        public int Id { get; set; }
        public int Proc_PurchaseOrderMasId { get; set; }
        public virtual Proc_PurchaseOrderMas Proc_PurchaseOrderMas { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Required]
        public decimal POQty { get; set; }
        public decimal POAmt { get; set; }
    }
}