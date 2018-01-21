using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace DhaliProcurement.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}