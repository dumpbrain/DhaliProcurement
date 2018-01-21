using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int? ClientId { get; set; }

        public int? ProjectGroupId { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public DateTime? EndDate { get; set; }

        public ProjectStatus Status { get; set; }
        [StringLength(200)]

        public string Remarks { get; set; }

        public virtual Client Client { get; set; }
        public virtual ProjectGroup ProjectGroup { get; set; }

        public virtual ICollection<ProjectSite> ProjectSites { get; set; }
        public virtual ICollection<ProjectResource> ProjectResources { get; set; }

        public enum ProjectStatus
        {
            Complete, Suspended, Closed
        }



    }
}