using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        HospitalContext db = new HospitalContext();

        public ActionResult Index()
        {
            var departments = db.Departments;
            ViewBag.Departments = departments;

            var doctors = db.Doctors;
            ViewBag.Doctors = doctors;

            if(User.Identity.IsAuthenticated)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                
                if (db.Visits.ToList() != null)
                {
                    var visits = db.Visits
                        .Include(v => v.Doctor)
                        .ToList();

                    var visits2 = visits.Where(v => (v.VisitTime.Date > DateTime.Now) && (v.UserID == user.Id));

                    ViewBag.Visits = visits2;
                }
            }

            return View();
        }

        public ActionResult ShowDoctor(int DoctorID = 0)
        {
            if (DoctorID != 0)
            {
                var doctor = db.Doctors
                    .Where(d => d.ID == DoctorID)
                    .FirstOrDefault();

                var visits = db.Visits
                    .Where(v => (v.DoctorID == DoctorID) && (v.Available == true))
                    .ToList();

                var visits2 = visits.Where(v => v.VisitTime.Date > DateTime.Now);

                var department = db.Departments
                    .Where(d => d.ID == doctor.DepartmentID)
                    .FirstOrDefault();

                ViewBag.Doctor = doctor;

                ViewBag.Visits = visits2;
                if (department != null)
                {
                    ViewBag.Department = department.Name;
                }

            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Reservation(int VisitID = 0)
        {
            var reservation = db.Visits
                .Where(v => v.ID == VisitID)
                .FirstOrDefault();

            if ((reservation != null) && (User.Identity.IsAuthenticated))
            {
                reservation.Available = false;
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                reservation.UserID = user.Id;
                reservation.PersonLastName = user.PersonLastName;
                reservation.PersonFirstName = user.PersonFirstName;
                reservation.PersonMiddleName = user.PersonMiddleName;
                reservation.PersonBirthDay = user.PersonBirthDay;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetDoctors(int DepartmentID)
        {
            if (DepartmentID != 0)
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors = db.Doctors
                    .Where(d => d.DepartmentID == DepartmentID)
                    .ToList();

                return Json(new { doctors = doctors }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var doctors = db.Doctors.ToList();

                return Json(new { doctors = doctors }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFIO(string LastName = "", string FirstName = "", string MiddleName = "")
        {
            if ((LastName != "") && (FirstName != "") && (MiddleName != ""))
            {

                List<Doctor> doctors = new List<Doctor>();
                doctors = db.Doctors.ToList();
                for(var i = 0; i < doctors.Count(); i++)
                {
                    if ((LastName != doctors[i].LastName) || (FirstName != doctors[i].FirstName) || (MiddleName != doctors[i].MiddleName))
                    {
                        doctors.Remove(doctors[i]);
                    }
                }

                var fio = doctors;
                return Json(new { fio = fio }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var fio = new List<Doctor>();

                return Json(new { fio = fio }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}