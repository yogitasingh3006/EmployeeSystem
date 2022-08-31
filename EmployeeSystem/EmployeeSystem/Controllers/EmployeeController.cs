using EmployeeSystem.Data;
using EmployeeSystem.Helper;
using EmployeeSystem.Models;
using EmployeeSystem.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeSystem.Controllers
{
    public class EmployeeController : Controller
    {
        //create reference for ApplicationDBContext
        private readonly ApplicationDbContext _db;

        //Constructor injection
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        //get
        //get method for login
        public IActionResult Login()
        {
           /* checking if the current login user details already saved in cookies
            * if (HttpContext.Request.Cookies.ContainsKey("userId") && HttpContext.Request.Cookies.ContainsKey("userPass"))
            {
                string myId =HttpContext.Request.Cookies["userId"];
                string myPass = HttpContext.Request.Cookies["userPass"];
                Employee emp = new Employee();
                emp.EmailId = myId;
                emp.Password = myPass;
               return  Login(emp);
                
            }*/
           //checking if current login user details already saved in session, if not getting the login page
             if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")) && string.IsNullOrEmpty(HttpContext.Session.GetString("userPass")))
             {
                 return View();
             }
             //if user details saved in session, calling the Login method(post) for approproate redirection of page according to the authorization
             else
             {
                //getting user id from the session
                 string myId=HttpContext.Session.GetString("userId");
                //getting user password from the session
                 string myPass = HttpContext.Session.GetString("userPass");
                //creating new object for employee
                 Employee emp = new Employee();
                //mapping user id and password from session to this object
                 emp.EmailId = myId;
                 emp.Password = myPass;
                return Login(emp);
            }
           
        }
        //post method for login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Employee obj)
        {
            //getting employee from database by its email and password
                Employee emp= DatabaseQueries.GetEmpByEmailAndPass(obj,_db);
                if(emp==null)
                {
                //if employee do not exists in the database, adding error message temporary
                TempData["error"] = "Invalid emailId or password";
                    return View();
                }
                else
                {
                //HttpContext.Response.Cookies.Append("userId", emp.EmailId);
                //HttpContext.Response.Cookies.Append("userPass", emp.Password.ToString());
                //checking if current login user details already saved in session
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")) && string.IsNullOrEmpty(HttpContext.Session.GetString("userPass")))
                {
                    //adding the current login employee emailid and passwowrd into the session
                    HttpContext.Session.SetString("userId", emp.EmailId);
                    HttpContext.Session.SetString("userPass", emp.Password.ToString());
                }
                //adding the login successfull message into temporary data
                TempData["message"] = "Login Successfully";
                //authorization
                if (emp.Role == "Admin")
                    //if user is admin, redirecting to the Admin Page
                    return RedirectToAction("AdminPage");
                else
                    //if user is employee, redirecting to the Employee Page
                    return RedirectToAction("EmployeePage", new { id = emp.Id });
                }
        }
        //get method for AdminPage
       public IActionResult AdminPage()
        {
            IEnumerable<ResultModel> employeeRecord=null;
            System.Diagnostics.Debug.WriteLine("Calling Method: "+Thread.CurrentThread.Name);
            //getting all the employees and departments from the database to display on AdminPage
            Thread child = new Thread(new ThreadStart(()=> employeeRecord = DatabaseQueries.GetAllEmployeesAndDepartments(_db)));
            //var employeeRecord = DatabaseQueries.GetAllEmployeesAndDepartments(_db);
            child.Name = "ChildThread1";
            child.Start();
            child.Join();
            System.Diagnostics.Debug.WriteLine("Calling Method again: " + Thread.CurrentThread.Name);
            return View(employeeRecord);
        }
        //get method for EmployeePage
        public IActionResult EmployeePage(int? id)
        
        {
            //if id is null or 0, returning the StatusCode 404 response
            if (id==null || id == 0)
            {
                return NotFound();
            }
            //if id is not null or 0, getting the employee details by its Id from the database to display
            var empFromDB = DatabaseQueries.GetEmpById(id,_db);
        
            return View(empFromDB);
        }
    
       

        //get
        //get method for editing the Personal Details by Employee itself
        public IActionResult EditPersonalDetails(int? id)
        {
            //if id is null or 0, returning the StatusCode 404 response
            if (id==null || id==0)
            {
                return NotFound();
            }
            //if id is not null or 0, getting the employee details by its Id from the database
            var empFromDB = DatabaseQueries.GetEmpById(id,_db);
            //if employee is not exists in the database, returning the StatusCode 404 response
            if (empFromDB==null)
            {
                return NotFound();
            }
            //if employee exists in database, displaying the pre filled Employee editing Page
            return View(empFromDB);
        }
        //Post method for editing PErsonal Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPersonalDetails(Employee emp)
        {
            //getting all the emails from the database except the current login employee email
            var emails = DatabaseQueries.GetAllEmpEmailsExcludingOne(emp,_db);
            foreach (string mail in emails)
            {
                //if new email already exists in database
                if (emp.EmailId == mail)
                {
                    //adding the error message intp temporary data
                    TempData["error"] = "Email already exists";
                    return EditPersonalDetails(emp.Id);
                }
            }
            //updating the employee details
            _db.Employees.Update(emp);
                _db.SaveChanges();
            //successfull message
            TempData["message"] = "Updation Done Successfully";
            return RedirectToAction("EmployeePage", new { id = emp.Id });
         
        }

        public IActionResult Logout()
        {
            //HttpContext.Response.Cookies.Delete("userId");
            //HttpContext.Response.Cookies.Delete("userPass");
            //removing the current logged in user details from the session
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userPass");
            //successfull message after logout to be displayed
            TempData["message"] = "Logout Successfully";
            //redirecting to home Page after logout
            return RedirectToAction("Index","Home");
        }

    }
}
