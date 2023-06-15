using HrManagementSystem.CustomDataModels;
using HrManagementSystem.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HrManagementSystem.Controllers
{
    public class ApplicantController : Controller
    {
        BiitHrmDBEntities4 db = new BiitHrmDBEntities4();


        // GET: Applicant
        public ActionResult Applicant_Dashboard()
        {
            return View();
        }

        //--Profile
        public ActionResult Applicant_Profile()
        {
            return View();
        }
        // -- Jobs 
        public async Task<ActionResult> Applicant_JobSection(string query/*,string bestmatch*/)
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            //--Education 
            var usrid = int.Parse(Session["u_id"].ToString());
            var u_edu = db.Educations.FirstOrDefault(x => x.u_id == usrid);
            //--------------
            string current_date = DateTime.Today.ToString("yyyy-MM-dd");
            DateTime parsedCurrentDate = DateTime.Parse(current_date);

            var filteredJobswithDate = db.Jobs
                .AsEnumerable() // Switch to LINQ to Objects
                .Where(x => DateTime.Parse(x.last_date_to_apply) >= parsedCurrentDate)
                .ToList();
            var filteredJobsWithQuery = db.Jobs.Where(
                x => x.title.Contains(query) || query == null && x.qualification== u_edu.degree
                ).ToList();

            var filteredJobs = (from fwd in filteredJobswithDate
                      join fwq in filteredJobsWithQuery on fwd.job_id equals fwq.job_id
                      select fwq).ToList();

            ///---Best Match with edu
            //var filteredJobsWithEdu = (from fwd in filteredJobswithDate
            //                    join fwe in u_edu on fwd.qualification equals fwe.degree
            //                    select fwd).ToList();

            return View(filteredJobs);
        }
        //--Personal Profile
        [HttpGet]
        public async Task<ActionResult> Applicant_PersonalProfile()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var usersList = await db.Users.FirstOrDefaultAsync(x => x.u_id == usrid);

            if (usersList != null)
            {
                return View(usersList);
            }
            return RedirectToAction("Applicant_Profile");
        }

        [HttpPost]
        public async Task<ActionResult> Applicant_PersonalProfile(HttpPostedFileBase file, User model)
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

                        return RedirectToAction("Applicant_PersonalProfile");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction("Applicant_Profile");
        }

        //--Education Section
        public async Task<ActionResult> Applicant_Education()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var edu = await db.Educations.Where(x => x.u_id == usrid).ToListAsync();
            return View(edu);
        }
        //--Add Education
        public ActionResult Applicant_AddEducation()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Applicant_AddEducation(Education edu)
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            edu.u_id = usrid;
            edu.has_added_edu = "true";

            db.Educations.Add(edu);
            db.SaveChanges();
            return RedirectToAction("Applicant_Education");
        }
        //--Delete Education
        public async Task<ActionResult> Delete_Education(int id)
        {

            var ed = db.Educations.Find(id);
            if (ed != null)
            {
                db.Educations.Remove(ed);
                db.SaveChanges();
                return RedirectToAction("Applicant_Education");
            }
            return RedirectToAction("Applicant_Profile");
        }
        //--Experience Section
        public async Task<ActionResult> Applicant_Experience()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            var exp = await db.Experiences.Where(x => x.u_id == usrid).ToListAsync();
            return View(exp);
        }
        //--Add Education
        public ActionResult Applicant_AddExperience()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Applicant_AddExperience(Experience exp)
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            exp.u_id = usrid;
            exp.has_exp = "true";

            db.Experiences.Add(exp);
            db.SaveChanges();
            return RedirectToAction("Applicant_Experience");
        }
        //--Delete Education
        public async Task<ActionResult> Delete_Experience(int id)
        {

            var exp = db.Experiences.Find(id);
            if (exp != null)
            {
                db.Experiences.Remove(exp);
                db.SaveChanges();
                return RedirectToAction("Applicant_Experience");
            }
            return RedirectToAction("Applicant_Profile");
        }

        //--Apply Job
        public ActionResult Applicant_ApplyJob(HttpPostedFileBase file,int id)
        {
            if (id!=null)
            {
                var usrid = int.Parse(Session["u_id"].ToString());
                var userInfo = db.Users.Find(usrid);
                var job = db.Jobs.Find(id);
                try
                {
                    if (file!=null)
                    {
                        // Save the uploaded file to the specified folder
                        var folder = Server.MapPath("~/Resumes/");
                        file.SaveAs(Path.Combine(folder, file.FileName.ToString()));
                        //-Store data to db
                        JobApplication japp = new JobApplication();
                        japp.job_id= id;
                        japp.u_id=usrid;
                        //japp.name = userInfo.f_name + userInfo.l_name;
                        

                        //---Query to check best match
                        var bm= db.Educations.Where(x=>x.u_id==usrid).FirstOrDefault();
                        var jbtbl = db.Jobs.Where(x => x.job_id == id).FirstOrDefault();
                        if (bm.degree!=jbtbl.qualification )
                        {
                            japp.status = "rejected";
                        }
                        else
                        {
                            japp.status = "pending";
                        }
                        if (bm.institute=="BIMS"||bm.institute=="PRESTON"||bm.institute=="QUAID E AZAM UNIVERSITY")
                        {
                            TempData["ErrorMessage"] = "You cannot apply for this Post.";
                            return RedirectToAction("Applicant_JobSection");
                        }
                        japp.name = userInfo.f_name+" "+userInfo.l_name;
                        japp.title = job.title;
                        //--Search for committe
                        var match_cm = db.Committes.FirstOrDefault(x => x.comm_title == japp.title);
                        if (match_cm!=null)
                        {
                            JobApplicationsCommitte jac = new JobApplicationsCommitte();
                            var q = db.JobApplications.Select(x=>x).ToList();
                            var lastId = 0;
                            if (q.Count > 0)
                            {
                                lastId = q.OrderByDescending(x => x.jobapp_id).First().jobapp_id;
                            }
                            var totaln = lastId;
                            jac.jobapp_id = totaln+1;
                            jac.comm_id = match_cm.comm_id;
                            db.JobApplicationsCommittes.Add(jac);
                            db.SaveChanges();
                        }
                        japp.short_list = "false";
                        japp.doc_path = file.FileName.ToString();

                        db.JobApplications.Add(japp);
                        db.SaveChanges();
                        return RedirectToAction("Applicant_JobSection");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View();
        }

        //--Applied Jobs
        public async Task<ActionResult> Applicant_JobApplications()
        {
            var usrid = int.Parse(Session["u_id"].ToString());
            //var applied_jobs=db.JobApplications.Find(usrid);

            var applied_jobs = await db.JobApplications.Where(x => x.u_id == usrid).ToListAsync();
            //var jobs = db.Jobs.Find(applied_jobs.job_id);

            //UserJobApplications uja = new UserJobApplications();
            //uja.job_title = jobs.title;
            //uja.location = jobs.location;
            //uja.salary= jobs.salary;
            //uja.status = applied_jobs.status;
            return View(applied_jobs);
        }

    }
}