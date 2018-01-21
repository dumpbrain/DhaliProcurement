using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_RequisitionDet
    {
        public int Id { get; set; }
        public int Proc_RequisitionMasId { get; set; }
        public virtual Proc_RequisitionMas Proc_RequisitionMas { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public decimal ReqQty { get; set; }
        public decimal CStockQty { get; set; }
        [StringLength(50)]
        public string Brand { get; set; }
        [StringLength(50)]
        public string Size { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RequiredDate { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }

    }
}