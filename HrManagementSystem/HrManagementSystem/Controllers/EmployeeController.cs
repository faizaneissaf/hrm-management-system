using HrManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HrManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        BiitHrmDBEntities4 db = new BiitHrmDBEntities4();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employee_Dashboard()
        {
            return View();
        }
        ///---Attendce Report
        public ActionResult Employee_AttendceReport()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var attR = db.Attendances.Where(x => x.u_id == usrid).ToList();
            return View(attR);
        }
        //--Apply For Leave
        public ActionResult Employee_ApplyLeave()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Employee_ApplyLeave(LeaveApplication la)
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            if (usrid != null)
            {
                la.u_id = usrid;
                la.status = "pending";
                db.LeaveApplications.Add(la);
                db.SaveChanges();
                return RedirectToAction("Employee_Dashboard");
            }
            else
            {
                return RedirectToAction("Employee_Dashboard");
            }
        }
        ///---Leave Applications
        public ActionResult Employee_LeaveApplications()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var attR = db.LeaveApplications.Where(x => x.u_id == usrid).ToList();
            return View(attR);
        }

        public ActionResult Employee_Committies()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var addin= db.CommitteMembers.Where(x=>x.u_id == usrid).ToList();
            var result = (from addi in addin
                                join c in db.Committes on addi.comm_id equals c.comm_id
                                select c).ToList();
            return View(result);
        }
        public ActionResult Employee_AllJObs(int id)
        {
            var jobs = db.JobApplicationsCommittes.Where(x => x.comm_id == id).ToList();
            var result = (from jb in jobs
                          join jt in db.JobApplications on jb.jobapp_id equals jt.jobapp_id
                          select jt).ToList();
            return View(result);
        }

        public ActionResult Employee_AddRemarks(int jobapp_id)
        {
            //-- comittie member id 
            var usrid = int.Parse(Session["u_id"].ToString());

            //--Finding User Id
            var jobrow = db.JobApplications.FirstOrDefault(x => x.jobapp_id == jobapp_id);
            var user_id = jobrow.u_id;
            var uinfo = db.Users.Where(x => x.u_id == user_id).ToList();
            var uedu= db.Educations.Where(y=>y.u_id==user_id).ToList();
            var uexp = db.Experiences.Where(z => z.u_id == user_id).ToList();

            List<JobApplication> ja_list = new List<JobApplication>();
            ja_list.Add(jobrow);

            ViewData["job_row"] = ja_list;
            ViewData["user_pInfo"] = uinfo;
            ViewData["user_edu"] = uedu;
            ViewData["user_exp"] = uexp;
            return View();
        }
        //--Adding Remarks
        [HttpPost]
        public async Task<ActionResult> Employee_AddRemarks(RemarksFromCommittie ar)
        {
            if (ar != null)
            {
                var usrid = int.Parse(Session["u_id"].ToString());
                ar.c_mem_id = usrid;
                db.RemarksFromCommitties.Add(ar);
                db.SaveChanges();
                return RedirectToAction("Employee_Committies");
            }
            else
            {
                return RedirectToAction("Employee_Committies");
            }
        }
    }
}