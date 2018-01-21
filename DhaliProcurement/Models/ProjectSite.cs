using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class ProjectSite
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [Display(Name = "Site Status")]
        public Status SiteStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<ProjectSiteResource> ProjectSiteResources { get; set; }

        public enum Status
        {
            Running, Complete, Suspended, Closed
        }

    }
}