using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HrManagementSystem.CustomDataModels
{
    public class UpdateAttendceModel
    {
        [Key]
        public int u_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact_no { get; set; }
        public string date { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public string status { get; set; }
        public string image { get; set; }
    }
}