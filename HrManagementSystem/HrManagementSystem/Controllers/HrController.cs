using HrManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HrManagementSystem.Controllers
{
    public class HrController : Controller
    {
        BiitHrmDBEntities4 db = new BiitHrmDBEntities4();
        // GET: Hr
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Hr_Dashboard()
        {
            return View();
        }
        //--Posted Jobs
        public ActionResult Hr_PostedJobs()
        {
            var pJobs = db.Jobs.ToList();
            return View(pJobs);
        }
        public ActionResult Hr_AddJobs() 
        {
            return View();
        }
        //--Add Jobs
        [HttpPost]
        public async Task<ActionResult> Hr_AddJobs(Job addJob)
        {            
            db.Jobs.Add(addJob);
            db.SaveChanges();
            return RedirectToAction("Hr_PostedJobs");
        }

        //--Hr Profile
        public async Task<ActionResult> Hr_Profile()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var usersList = await db.Users.FirstOrDefaultAsync(x => x.u_id == usrid);

            if (usersList != null)
            {
                return View(usersList);
            }
            return RedirectToAction("Hr_Profile");
        }
        [HttpPost]
        public async Task<ActionResult> Hr_Profile(HttpPostedFileBase file, User model)
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

                        return RedirectToAction("Hr_Profile");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction("Hr_Profile");
        }

        //--Attendce Report
        public ActionResult Hr_Attendce()
        {
            var emp = db.Users.Where(x => x.role == "employee" || x.role == "hr" || x.role == "guard").ToList();
            return View(emp);
        }
        public ActionResult Hr_AttendceReport(int id)
        {
            var attR = db.Attendances.Where(x => x.u_id == id).ToList();
            return View(attR);
        }
        //--Job Applications 
        public ActionResult Hr_JobApplications(string s_app)
        {
            var jobs = db.Jobs.Where(x => x.title.Contains(s_app) || s_app == null).ToList();
            return View(jobs);
        }
        //--JOb Applicants
        public ActionResult Hr_JobApplicants(int id)
        {
            var jobapp = db.JobApplications.Where(x=>x.job_id==id).ToList();
            return View(jobapp);
        }
        //--Commettie
        public ActionResult Hr_Commettie()
        {
            var c = db.Committes.ToList();
            return View(c);
        }
        //--Select Head
        public ActionResult Hr_ShowCommettieHead()
        {
            var emp = db.Users.Where(x => x.role == "employee" || x.role == "hr").ToList();
            return View(emp);
        }
        public ActionResult Hr_AddCommettie()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Hr_AddCommettie(int id,Committe cm)
        {
            Committe com = new Committe();

            com.comm_title = cm.comm_title;
            com.comm_head = id.ToString();
            db.Committes.Add(com);
            db.SaveChanges();
            return RedirectToAction("Hr_Commettie");
        }
        //---COmmettie Members
        public ActionResult Hr_CommettieMembers(int id)
        {
            if (id != null)
            {
                Session["comm_id"] = id;
            }
            var cm = db.CommitteMembers.Where(x=>x.comm_id==id).ToList();
            var usr = from m in cm
                        join u in db.Users on m.u_id equals u.u_id
                        select u;
            List<User> user = usr.ToList();
            return View(user);
        }
        public ActionResult Hr_AddCommettieMembers()
        {
            var emp = db.Users.Where(x => x.role == "employee" || x.role == "hr").ToList();
            return View(emp);
        }
        public async Task<ActionResult> Hr_AddMembers(int m_id)
        {
            CommitteMember cm = new CommitteMember();
            var c_id = int.Parse(Session["comm_id"].ToString());
            cm.comm_id = c_id;
            cm.u_id = m_id;
            db.CommitteMembers.Add(cm);
            db.SaveChanges();
            return RedirectToAction("Hr_CommettieMembers/" + c_id);
        }
        //--Leave Management
        public ActionResult Hr_LeaveApplications()
        {
            var attR = db.LeaveApplications.Where(x=>x.status=="pending").ToList();
            return View(attR);
        }
        public ActionResult Hr_laApproved(int id)
        {
            var l = db.LeaveApplications.FirstOrDefault(x => x.leaveapp_id == id);
            l.status = "approved";
            db.SaveChanges();
            return RedirectToAction("Hr_LeaveApplications");
        }
        public ActionResult Hr_laRejected(int id)
        {
            var l = db.LeaveApplications.FirstOrDefault(x => x.leaveapp_id == id);
            l.status = "reject";
            db.SaveChanges();
            return RedirectToAction("Hr_LeaveApplications");
        }
    }
}