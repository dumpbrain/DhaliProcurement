using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_TenderDet
    {
        public int Id { get; set; }
        public int Proc_TenderMasId { get; set; }
        public virtual Proc_TenderMas Proc_TenderMas { get; set; }

        public int Proc_RequisitionDetId { get; set; }
        public virtual Proc_RequisitionDet Proc_RequisitionDet { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TQDate { get; set; }
        [StringLength(50)]
        public string TQNo { get; set; }
        public decimal TQPrice { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
    }
}