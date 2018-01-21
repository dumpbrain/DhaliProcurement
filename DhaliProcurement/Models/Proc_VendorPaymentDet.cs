using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DhaliProcurement.Models
{
    public class Proc_VendorPaymentDet
    {
        [Key]
        public int Id { get; set; }
        public int Proc_VendorPaymentMasId { get; set; }
        public virtual Proc_VendorPaymentMas Proc_VendorPaymentMas { get; set; }
        public int Proc_MaterialEntryDetId { get; set; }
        public virtual Proc_MaterialEntryDet Proc_MaterialEntryDet { get; set; }
        [StringLength(50)]
        public string BillNo { get; set; }
        [Required]
        public decimal PayAmt { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }


    }
}