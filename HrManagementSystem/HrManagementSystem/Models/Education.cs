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
    
    public partial class Education
    {
        public int edu_id { get; set; }
        public Nullable<int> u_id { get; set; }
        public string degree { get; set; }
        public string institute { get; set; }
        public string board { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string has_added_edu { get; set; }
    }
}
