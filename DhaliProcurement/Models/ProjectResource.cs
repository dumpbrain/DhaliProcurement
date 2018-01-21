using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class ProjectResource
    {
        [Key, Column(Order = 0)]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 1)]
        public int CompanyResourceId { get; set; }             
        public virtual CompanyResource CompanyResource { get; set; }
    }
}