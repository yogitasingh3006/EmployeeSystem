using EmployeeSystem.Controllers;
using EmployeeSystem.Data;
using EmployeeSystem.Models;
using EmployeeSystem.Models.Helper;

namespace EmployeeSystem.Helper
{
    public static class DatabaseQueries
    {

        public static Employee GetEmpByEmailAndPass(Employee obj,ApplicationDbContext _db)
        {
            var emp = _db.Employees.Where(d => d.EmailId == obj.EmailId && d.Password == obj.Password).FirstOrDefault();
            return emp;
        }

        public static Employee GetEmpById(int? id,ApplicationDbContext _db)
        {
            System.Diagnostics.Debug.WriteLine("In called Method: " + Thread.CurrentThread.Name);
            lock (_db)
            {

                return _db.Employees.Find(id);
            }
        }

        public static List<string> GetAllEmails(ApplicationDbContext _db)
        {
            return _db.Employees.Select(e => e.EmailId).ToList();
        }
        public static List<string> GetAllEmpEmailsExcludingOne(Employee emp, ApplicationDbContext _db)
        {
            return _db.Employees.Where(e => e.Id != emp.Id).Select(e => e.EmailId).ToList();
        }
        public static IEnumerable<ResultModel> GetAllEmployeesAndDepartments(ApplicationDbContext _db)
        {
            
                System.Diagnostics.Debug.WriteLine("In called Method: " + Thread.CurrentThread.Name);
            lock (_db)
            {
                List<Employee> employees = _db.Employees.ToList();
                List<Department> departments = _db.Departments.ToList();

                var employeeRecord = from e in employees
                                     join d in departments on e.DepartmentId equals d.Id
                                     select new ResultModel
                                     {
                                         Emp = e,
                                         Dept = d
                                     };
                return employeeRecord;
            }
            

        }
        public static Department GetDepartByName(Department obj, ApplicationDbContext _db)
        {
            return _db.Departments.Where(d => d.DepartName == obj.DepartName).FirstOrDefault();
        }
        public static string GetDepartNameById(Employee employeeFromDb,ApplicationDbContext _db)
        {
            return _db.Departments.Where(d => d.Id == employeeFromDb.DepartmentId).Select(d => d.DepartName).FirstOrDefault();
        }

    }
}
