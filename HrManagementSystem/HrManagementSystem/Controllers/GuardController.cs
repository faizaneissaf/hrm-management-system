using HrManagementSystem.CustomDataModels;
using HrManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HrManagementSystem.Controllers
{
    public class GuardController : Controller
    {
        BiitHrmDBEntities4 db = new BiitHrmDBEntities4();
        // GET: Guard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Guard_Dashboard(string s_query)
        {
            if (s_query!=null)
            {
                var empList = db.Users.Where(x => x.role == "employee" || x.role == "hr").ToList();
                var emp = empList.Where(x =>x.f_name.Contains(s_query)).ToList();
                return View(emp);
            }
            else
            {
                var emp = db.Users.Where(x => x.role == "employee" || x.role == "hr").ToList();
                return View(emp);
            }
        }
        //--Checking Checkin-Checkout Status
        public ActionResult Guard_CheckStatus(int id)
        {
            string current_date = DateTime.Today.ToString("yyyy-MM-dd");
            var chk = db.Attendances.FirstOrDefault(x => x.u_id == id && x.checkout=="false");        
            if (chk!=null)
            {
                int a_id = chk.att_id;
                id = a_id;
                return RedirectToAction("Guard_CheckOut/" + id);
            }
            else {
                return RedirectToAction("Guard_CheckIn/" + id);
            }
        }
        //--Checkin
        public ActionResult Guard_CheckIn(int id) 
        {
            var user = db.Users.Where(x=>x.u_id==id).ToList();
            ViewData["user_data"] = user;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Guard_CheckIn(int id,Attendance att)
        {
            if (id!= null)
            {
                att.u_id = id;
                att.date = att.date;
                att.checkin = att.checkin;
                att.checkout = "false";
                att.status = "present";
                db.Attendances.Add(att);
                db.SaveChanges();

                return RedirectToAction("Guard_Dashboard");
            }
            else {
                return RedirectToAction("Guard_Dashboard");
            }
        }
        //--CheckOut
        public ActionResult Guard_CheckOut(int id)
        {
            var u = db.Attendances.FirstOrDefault(x => x.att_id == id);
            var user = db.Users.Where(x => x.u_id == u.u_id).ToList();
            ViewData["user_data"] = user;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Guard_CheckOut(int id, Attendance att)
        {
            if (id != null)
            {
                var attn = db.Attendances.FirstOrDefault(x => x.att_id == id);
                attn.date = att.date;
                attn.checkout = att.checkout;
                db.SaveChanges();
                return RedirectToAction("Guard_Dashboard");
            }
            else
            {
                return RedirectToAction("Guard_Dashboard");
            }
        }
        
        public async Task<ActionResult> Guard_Profile()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var usersList = await db.Users.FirstOrDefaultAsync(x => x.u_id == usrid);

            if (usersList != null)
            {
                return View(usersList);
            }
            return RedirectToAction("Guard_Profile");
        }
        [HttpPost]
        public async Task<ActionResult> Guard_Profile(HttpPostedFileBase file, User model)
        {
            var user = await db.Users.FindAsync(model.u_id);
            if (user != null)
            {
                try
                {
                    // Save the uploaded file to the specified folder
                    var folder = Server.MapPath("~/Profiles/");
                    file.SaveAs(Path.Combine(folder, file.FileName.ToString()));

                    if (file != null)
                    {
                        user.f_name = model.f_name;
                        user.l_name = model.l_name;
                        user.email = model.email;
                        user.contact_no = model.contact_no;
                        user.cnic = model.cnic;
                        user.dob = model.dob;
                        user.gender = model.gender;
                        user.address = model.address;
                        user.password = model.password;
                        user.role = model.role;
                        user.image = file.FileName.ToString();

                        db.SaveChanges();

                        return RedirectToAction("Guard_Profile");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction("Guard_Profile");
        }

        public ActionResult Guard_AttendceReport()
        {
            var emp = db.Users.Where(x => x.role == "employee" || x.role == "hr" || x.role == "guard").ToList();
            return View(emp);
        }

        public ActionResult Guard_Report(int id)
        {
            var attR = db.Attendances.Where(x => x.u_id == id).ToList();
            return View(attR);
        }
    }
}