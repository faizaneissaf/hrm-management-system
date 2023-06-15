using HrManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HrManagementSystem.Controllers
{
    public class UsersController : Controller
    {

        BiitHrmDBEntities4 db=new BiitHrmDBEntities4();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        //--Login Page
        public ActionResult Login_Page()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login_Page(String email, String password)
        {
            var loginuser = db.Users.FirstOrDefault(x => x.email == email && x.password == password);
            if (loginuser != null)
            {
                //HttpContext.Session.SetInt32("u_id", loginuser.u_id);
                Session["u_id"] = loginuser.u_id;
                if (loginuser.role == "applicant")
                {
                    return RedirectToAction("Applicant_Dashboard", "Applicant");
                }
                else if (loginuser.role == "hr")
                {
                    return RedirectToAction("Hr_Dashboard", "Hr");
                }
                else if (loginuser.role == "guard")
                {
                    return RedirectToAction("Guard_Dashboard", "Guard");
                }
                else if (loginuser.role == "employee")
                {
                    return RedirectToAction("Employee_Dashboard", "Employee");
                }
                else if (loginuser.role == "hod")
                {
                    return RedirectToAction("Hod_Dashboard", "HOD");
                }
                return RedirectToAction("Signup_Page");
            }
            else
            {
                return RedirectToAction("Login_Page");
            }

        }

        //Signup
        [HttpGet]
        public ActionResult Signup_Page()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Signup_Page(User adduser)
        {
            var user = new User()
            {
                f_name = adduser.f_name,
                l_name = adduser.l_name,
                email = adduser.email,
                contact_no = adduser.contact_no,
                dob = "-",
                gender = "-",
                address = "-",
                cnic = adduser.cnic,
                role = "applicant",
                password = adduser.password,
                image = "-",
            };

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Login_Page");
        }


        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login_Page", "Users");
        }
    }
}