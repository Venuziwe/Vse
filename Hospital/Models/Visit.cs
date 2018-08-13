using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class Visit
    {
        public int ID { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMiddleName { get; set; }
        public DateTime PersonBirthDay { get; set; }
        public DateTime VisitTime { get; set; }
        public bool Available { get; set; }
        public string UserID { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorID { get; set; }
        public Doctor Doctor { get; set; }
    }
}