//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HrManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendance
    {
        public int att_id { get; set; }
        public Nullable<int> u_id { get; set; }
        public string date { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public string status { get; set; }
    }
}
