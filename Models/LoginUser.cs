using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models{
    public class LoginUser{

        
        [Required]
        [EmailAddress]
        [Display(Name="Email: ")]
        public string LoginEmail { get; set; }

      
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password: ")]
        [MinLength(8, ErrorMessage="Password is incorrect!")]
        public string LoginPassword { get; set; }


    }
}