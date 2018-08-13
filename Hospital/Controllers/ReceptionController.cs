using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Hospital.Controllers
{

    public class ReceptionController : Controller
    {
        HospitalContext db = new HospitalContext();

        [Authorize(Roles = "reception")]
        public ActionResult Reception()
        {
            var departments = db.Departments;
            ViewBag.Departments = departments;

            var doctors = db.Doctors;
            ViewBag.Doctors = doctors;

            var cards = db.Cards;
            ViewBag.Cards = cards;

            var startWorkTime = new DateTime(2018, 03, 08, 10, 00, 00);
            var endWorkTime = new DateTime(2018, 03, 08, 16, 00, 00);

            var timetable = new List<string>();

            while (startWorkTime.ToShortTimeString() != endWorkTime.ToShortTimeString())
            {
                timetable.Add(startWorkTime.ToShortTimeString());
                startWorkTime = startWorkTime.AddMinutes(20);
            }
            timetable.Add(startWorkTime.ToShortTimeString());

            ViewBag.Timetable = timetable;

            var today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Today = today;

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult AddDepartment(string FormDepartmentName)
        {
            if (FormDepartmentName != null)
            {
                db.Departments.Add(new Department { Name = FormDepartmentName });
                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }

        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult AddDoctor(string FormLastName, string FormFirstName, string FormMiddleName)
        {
            if ((FormLastName != null) && (FormFirstName != null) && (FormMiddleName != null))
            {
                db.Doctors.Add(new Doctor { LastName = FormLastName, FirstName = FormFirstName, MiddleName = FormMiddleName });
                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }

        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult AddDoctorToDepartment(int DepartmentID, int DoctorID)
        {
            if ((DepartmentID != 0) && (DoctorID != 0))
            {
                var doctor = db.Doctors
                    .Where(d => d.ID == DoctorID)
                    .FirstOrDefault();

                doctor.DepartmentID = DepartmentID;
                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }

        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult AddTimetable(int FormDoctorID, string FormDate, List<string> FormTimetable)
        {
            if ((FormDoctorID != 0) && (FormDate != "") && (FormTimetable.Count != 0))
            {
                if(db.Visits.ToList() != null)
                {
                    var visits = db.Visits
                        .Where(v => v.DoctorID == FormDoctorID).ToList();

                    var visits2 = visits.Where(v => v.VisitTime.Date == Convert.ToDateTime(FormDate)).ToList();

                    for (int i = 0; i < visits2.Count; i++)
                    {
                        if (visits2[i] != null)
                        {
                            db.Visits.Remove(visits2[i]);
                        }
                    }

                }

                var visitTime = "";

                for (int i = 0; i < FormTimetable.Count; i++)
                {
                    visitTime = FormDate + " " + FormTimetable[i] + ":00";
                    if (Convert.ToDateTime(visitTime) > DateTime.Now)
                    {
                        db.Visits.Add(new Visit
                        {
                            DoctorID = FormDoctorID,
                            VisitTime = Convert.ToDateTime(visitTime),
                            Available = true
                        });
                        visitTime = "";
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }
        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult AddCard(string FormLastName, string FormFirstName, string FormMiddleName, string FormBirthDay)
        {
            if ((FormLastName != "") && (FormFirstName != "") && (FormMiddleName != "") && (FormBirthDay != ""))
            {
                db.Cards.Add(new Card
                {
                    LastName = FormLastName,
                    FirstName = FormFirstName,
                    MiddleName = FormMiddleName,
                    BirthDay = Convert.ToDateTime(FormBirthDay)
                });
                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }

        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult ShowCard (int FormCardID = 0)
        {
            if (FormCardID != 0)
            {
                var card = db.Cards
                        .Where(c => c.CardID == FormCardID)
                        .FirstOrDefault();

                return View(card);
            }
            else
            {
                return RedirectToAction("Reception", "Reception");
            }
        }

        [HttpPost]
        [Authorize(Roles = "reception")]
        public ActionResult ChangeDiseases(int FormCardID, string FormDiseases)
        {
            if (FormCardID != 0)
            {
                var card = db.Cards
                    .Where(c => c.CardID == FormCardID)
                    .FirstOrDefault();

                card.Diseases = FormDiseases;

                db.SaveChanges();
            }
            return RedirectToAction("Reception", "Reception");
        }
    }
}