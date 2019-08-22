using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models{
    public class DateIsInTheFuture : ValidationAttribute
        {
            public override string FormatErrorMessage(string name)
            {
                return "Date value should not be a future date";
            }

            protected override ValidationResult IsValid(object objValue,ValidationContext validationContext)
            {
                var dateValue = objValue as DateTime? ?? new DateTime();

                //alter this as needed. I am doing the date comparison if the value is not null

                if (dateValue.Date < DateTime.Now.Date)
                {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                return ValidationResult.Success;
            }
        }
    public class User{
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name="First Name: ")]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name: ")]
        [MinLength(2,ErrorMessage="Last Name must be at least 2 characters!")]
        public string  LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name="Email: ")]
        public string Email { get; set; }


        [Required]
        [Display(Name="Password: ")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage="Confirm password is incorrect!")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

        public List<Attendance> WedstoAttend { get; set; }
    }
}