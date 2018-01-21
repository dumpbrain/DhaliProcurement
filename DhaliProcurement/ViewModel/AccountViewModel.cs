using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace DhaliProcurement.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]        
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
       
    }

    public class ChangePasswordVM
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Does not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordUserVM
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(155)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Does not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserVM
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
               
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
               

    }
}