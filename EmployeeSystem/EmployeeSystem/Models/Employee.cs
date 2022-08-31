using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter emailId")]
        [DataType(DataType.EmailAddress)]

        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \'{0}\' must have {2} character", MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match !!")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
         [Required(ErrorMessage = "Please Enter Phone number")]
        // [StringLength(11, ErrorMessage = "Phone Number must have {2} character", MinimumLength = 10)]
         [MaxLength(10, ErrorMessage = "{0} can have {1} numbers")]
         [RegularExpression(@"^([0-9]{10})$",ErrorMessage="Invalid Phone Number")]
         [DataType(DataType.PhoneNumber)]
        //[Range(1000000000,10000000000)]
        public string Phone { get; set; }

        [Display(Name="Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public string Role { get; set; }

    }
}
