using HrManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HrManagementSystem.Controllers
{
    public class HODController : Controller
    {
        BiitHrmDBEntities4 db = new BiitHrmDBEntities4();
        // GET: HOD
        public ActionResult Hod_Dashboard()
        {
            var jobs = db.Jobs.ToList();
            return View(jobs);
        }
        public ActionResult Hod_JobApplicants(int j_id)
        {
            var jobapp = db.JobApplications.Where(x => x.job_id == j_id).ToList();
            return View(jobapp);
        }
        public ActionResult Hod_committieRemarks(int japp_id)
        {
            var jobapp = db.JobApplications.Where(x => x.jobapp_id == japp_id).ToList();
            var jremarks= db.RemarksFromCommitties.Where(x=>x.jobapp_id==japp_id).ToList();

            Session["job_app_rem_id"] = japp_id;
            

            ViewData["jobapp_info"] = jobapp;
            ViewData["remarks_info"] = jremarks;
            return View();
        }
        public ActionResult Hod_Hire()
        {
            var jid = int.Parse(Session["job_app_rem_id"].ToString());
            var hire = db.JobApplications.FirstOrDefault(x => x.jobapp_id == jid);
            if (hire != null)
            {
                hire.status = "approve";
                db.SaveChanges();
            }
            return RedirectToAction("Hod_Dashboard");
        }
        public ActionResult Hod_Reject()
        {
            var jid = int.Parse(Session["job_app_rem_id"].ToString());
            var hire = db.JobApplications.FirstOrDefault(x => x.jobapp_id == jid);
            if (hire != null)
            {
                hire.status = "rejected";
                db.SaveChanges();
            }
            return RedirectToAction("Hod_Dashboard");
        }
    }
}