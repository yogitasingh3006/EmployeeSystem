using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Models.Helper
{
    public class UserDetails
    {

        [Required(ErrorMessage = "Please Enter User Name")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Please Enter emailId")]
        [DataType(DataType.EmailAddress)]

        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must have {2} character", MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match !!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        public int DepartName { get; set; }

    }
}
