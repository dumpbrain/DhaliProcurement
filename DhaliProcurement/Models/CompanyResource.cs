using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class CompanyResource
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        [Display(Name = "Date of Joining")]
        public DateTime? DOJ { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }
       
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string Address { get; set; }
        
        [StringLength(1)]
        [Required]
        public string Status { get; set; }

        public virtual ICollection<ProjectResource> ProjectResources { get; set; }
        public virtual ICollection<ProjectSiteResource> ProjectSiteResources { get; set; }
    }
}