using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class Doctor
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public int? DepartmentID { get; set; }
        public Department Department { get; set; }

        public ICollection<Visit> Visits;

        public Doctor()
        {
            Visits = new List<Visit>();
        }
    }
}