using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_VendorPaymentMas
    {
    
        [Key]
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime VPDate { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }

    }
}