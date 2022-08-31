using EmployeeSystem.Data;
using EmployeeSystem.Helper;
using EmployeeSystem.Models;
using EmployeeSystem.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSystem.Controllers
{
    public class AdminController : Controller
    {
        //create reference for ApplicationDBContext
        private readonly ApplicationDbContext _db;

        //constructor injection
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        //get
        //get metho for adding a new department in database
        public IActionResult AddDepartment()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDepartment(Department obj)
        {
            if(ModelState.IsValid)
            {
                //getting all the department names
                var data = DatabaseQueries.GetDepartByName(obj,_db);
                //ad department only if department name do not exists already in the database
                if (data == null)
                {
                    //adding new department in database
                    _db.Departments.Add(obj);
                    //save all changes made in the context to the database
                    _db.SaveChanges();
                    //successfull notification
                    TempData["message"] = "Department Added Successfully";
                    //redirecting to the AdminPage after adding
                    return RedirectToAction("AdminPage", "Employee");
                }
                else
                    //error message if department name already exists
                    ModelState.AddModelError("departName", "Department Name Already Exists");
                    TempData["error"] = "Department already exists";
            }
            return View(obj);
        }
        //get
        //get method for Adding new User(can be employee or Admin)
        public IActionResult AddUser()
        { 
            //getting department data for dropdown list
            var nameList = (from d in _db.Departments
                             select new SelectListItem()
                             {
                                 Text = d.DepartName,
                                 Value=d.Id.ToString()
                             }).ToList() ;
             nameList.Insert(0, new SelectListItem()
             {
                 Text="---Select---",
                 Value=String.Empty
             });
            //adding the list into viewbag
             ViewBag.ListOfNames = nameList;
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(UserDetails obj)
        {
            //checking if ModelState values in UserDetails model is valid or not
            if (ModelState.IsValid)
            {
                //Mapping data of UserDetails model to Employee And Department models
                Employee emp = new Employee();
                Department dept = new Department();
                emp.Name = obj.EmpName;
                emp.Phone = obj.Phone;
                emp.City = obj.City;
                emp.Role = obj.Role;
                emp.EmailId = obj.EmailId;
                emp.Password = obj.Password;
                emp.DepartmentId = obj.DepartName;
                //Getting all the emails of Employees in the database
                var emails = DatabaseQueries.GetAllEmails(_db);
                //checking if email already exists or not
                foreach(string mail in emails)
                {   
                    if(emp.EmailId == mail)
                    {
                        //error message if email already exists
                        TempData["error"] = "Email already exists";
                        return AddUser();
                    }
                }
              //Adding new employee into the database
                _db.Employees.Add(emp);
                    _db.SaveChanges(); 
                //successfull notification
                   TempData["message"] = "Employee Added Successfully";
                    return RedirectToAction("AdminPage", "Employee");
                
            }
            return View();
        }
        //get
        //get method to delete the employee from the database
        public IActionResult Delete(int? id)
        {
            //if id is null or 0, returning the StatusCode 404 response
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //if id is not null or 0, getting employee from database by its Id
            var employeeFromDb = DatabaseQueries.GetEmpById(id,_db);
            //getting department name of a particular employee by its Id from the database 
            var departmentname = DatabaseQueries.GetDepartNameById(employeeFromDb,_db);
            //adding department name into temporary data
            TempData["departmentname"] = departmentname;
            if (employeeFromDb == null)
            {
                return NotFound();
            }
            return View(employeeFromDb);
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            //getting employee by its Id from the database
            var obj = DatabaseQueries.GetEmpById(id,_db);
            //if employee do not exists, returning the StatusCode 404 response
            if (obj == null)
            {
                return NotFound();
            }
            //if employee exists, delete from the database
            _db.Employees.Remove(obj);
            _db.SaveChanges();
            //successfull notification
            TempData["message"] = "Employee Deleted Successfully";
            return RedirectToAction("AdminPage","Employee");
        }
        //get
        //get method of editing the employee details
        public IActionResult Edit(int? id)
        {
            Employee empFromDB = null ;
            //if id is null or 0, returning the StatusCode 404 response
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //if id is not null or 0, getting employee from database by its Id
            Thread child = new Thread(new ThreadStart(() => empFromDB = DatabaseQueries.GetEmpById(id,_db)));
            child.Name = "ChildThread2";
            child.Start();
            child.Join();
            System.Diagnostics.Debug.WriteLine("Calling Method again: " + Thread.CurrentThread.Name);
            //if employee do not exists with this id, returning the StatusCode 404 response
            if (empFromDB == null)
            {
                return NotFound();
            }
            //getting department data for dropdown list
            var nameList = (from d in _db.Departments
                            select new SelectListItem()
                            {
                                Text = d.DepartName,
                                Value = d.Id.ToString()
                            }).ToList();
            nameList.Insert(0, new SelectListItem()
            {
                Text = "---Select---",
                Value = String.Empty
            });
            //adding department data into viewbag
            ViewBag.ListOfNames = nameList;
            empFromDB.ConfirmPassword = empFromDB.Password;
            return View(empFromDB);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            //getting all the emails from the database except the current login employee email
            var emails = DatabaseQueries.GetAllEmpEmailsExcludingOne(emp,_db);
            //checking if new emailid already exists or not in the database
            foreach (string mail in emails)
            {
                if (emp.EmailId == mail)
                {
                    //error message if  new email already exists in database
                    TempData["error"] = "Email already exists";
                    return Edit(emp.Id);
                }
            }
            //updating the data of a employee
            _db.Employees.Update(emp);
            _db.SaveChanges();
            //successfull notification
            TempData["message"] = "Updation Done Successfully";
            return RedirectToAction("AdminPage", "Employee");
        }
    }
}
