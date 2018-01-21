using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class ProcProject
    {
        [Key]
        public int Id { get; set; }

        public int ProjectSiteId { get; set; }
        public virtual ProjectSite ProjectSite { get; set; }

        [Display(Name = "BOQ Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BOQDate { get; set; }

        [StringLength(100)]
        public string BOQNo { get; set; }

        [Display(Name = "NOA Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NOADate { get; set; }

        [StringLength(100)]
        public string NOANo { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }






    }
}