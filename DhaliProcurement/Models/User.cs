using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DhaliProcurement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Salt { get; set; }
                
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
               
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
                
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
                
        [MaxLength(100)]
        public string Phone { get; set; }
                
        [MaxLength(255)]
        public string Address { get; set; }
                
        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        //[Display(Name = "Full Name")]
        //public string fullname
        //{
        //    get { return FirstName + " " + LastName; }
        //}
    }
}