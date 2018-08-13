using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
        public Department()
        {
            Doctors = new List<Doctor>();
        }
    }
}