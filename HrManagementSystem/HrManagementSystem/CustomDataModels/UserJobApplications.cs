using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrManagementSystem.CustomDataModels
{
    public class UserJobApplications
    {
        public int job_id { get; set; }
        public int u_id { get; set; }
        public string name { get; set; }
        public string job_title { get; set; }
        public string salary { get; set; }
        public string location { get; set; }
        public string status { get; set; }
    }
}