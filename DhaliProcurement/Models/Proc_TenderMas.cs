using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_TenderMas
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        [Index("TNo", IsUnique = true)]
        public string TNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TDate { get; set; }
        [MaxLength(100)]
        public string Specs { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        [StringLength(1)]
        public string isApproved { get; set; }
    }
}