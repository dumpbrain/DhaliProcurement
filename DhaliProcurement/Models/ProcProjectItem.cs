using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace DhaliProcurement.Models
{
    public class ProcProjectItem
    {
        [Key]
        public int Id { get; set; }

        public int ProcProjectId { get; set; }
        public virtual ProcProject ProcProject { get; set; }


        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        [Required]
        public decimal PQuantity { get; set; }

        public decimal PCost { get; set; }

        [StringLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}