using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [Display(Name = "Size")]
        [StringLength(50)]
        public string Size { get; set; }

        [Display(Name = "HS Code")]
        [StringLength(100)]
        public string HSCode { get; set; }

        [Display(Name = "Item Description")]
        [StringLength(100)]
        public string ItemDesc { get; set; }

        
    }
}