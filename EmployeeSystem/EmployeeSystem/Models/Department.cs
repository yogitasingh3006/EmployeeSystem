using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "Please Enter Department name")]
        //[Remote("IsDepartExists","Home",ErrorMessage="Already exists")]
        public string DepartName { get; set; }

      
    }
}
