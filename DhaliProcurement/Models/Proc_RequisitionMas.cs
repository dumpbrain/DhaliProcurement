using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_RequisitionMas
    {
        [Key]
        public int Id { get; set; }

        public int ProcProjectId { get; set; }
        public virtual ProcProject ProcProject { get; set; }
        [Required]
        [StringLength(20)]
        [Index("RCodeIndex", IsUnique = true)]
        public string Rcode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReqDate { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        //public string isApproved { get; set; }
    }
}