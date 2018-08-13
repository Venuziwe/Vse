using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Hospital.Models
{
    public class HospitalContext : DbContext
    {
        public HospitalContext() : base("DefaultConnection")
        { }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}