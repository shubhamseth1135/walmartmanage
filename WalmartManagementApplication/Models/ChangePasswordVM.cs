using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WalmartManagementApplication.Models
{
    public class ChangePasswordVM
    {
        
        [Required(ErrorMessage ="Username Required")]
        [StringLength(20,ErrorMessage ="Max 20 characters allowed")]
        public string username { get; set; }
        [Required(ErrorMessage = "Old Password Required")]
        [StringLength(15, ErrorMessage = "Max 15 characters allowed")]
        public string password { get; set; }
        [Required(ErrorMessage = "New Password Required")]
        [StringLength(15, ErrorMessage = "Max 15 characters allowed")]
        public string newpassword { get; set; }
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("newpassword", ErrorMessage = "Password Doesnt Match")]
        [StringLength(15, ErrorMessage = "Max 15 characters allowed")]
        public string confirmpassword { get; set; }

    }
}