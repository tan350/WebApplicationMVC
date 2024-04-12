using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VO;

namespace WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        ValidationBL  validationBLObject =new ValidationBL();

        List<StudentVO> studentRecords = new List<StudentVO>();

        StudentVO student;

        public ActionResult Index()
        {
            if (HttpContext.Application["IsAuthenticated"] != null && (bool)HttpContext.Application["IsAuthenticated"] == true)
            {

                studentRecords = validationBLObject.ReadAllRecords();
                return View(studentRecords);
            }
            else 
            { 
                return RedirectToAction("login","UserLogin");
            }
        }

        public ActionResult New()
        {
            ViewBag.Message = "New";

            int nextSrNo = validationBLObject.GetSrNoForNewRecord();
            student = new StudentVO{ SrNo= nextSrNo };

            return View(student);
        }

        [HttpPost]
        public ActionResult New(StudentVO student)
        {
            ViewBag.Message = "New";
            
            validationBLObject.AddNewRecord(student);

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Update(int id)
        {
            ViewBag.Message = "Update";
            StudentVO studentRecord = new StudentVO();


            studentRecord = validationBLObject.GetRecordUsingSrNo(id);
            
            return View("New", studentRecord);
        }

        [HttpPost]
        public ActionResult Update(StudentVO student)
        {
            ViewBag.Message = "Update";
            validationBLObject.UpdateRecord(student.SrNo,student);

            return Json(new { success = true, message = "Data Updated Successfully!" });

        }

        public ActionResult View(int id)
        {
            ViewBag.Message = "View";
            StudentVO studentRecord = new StudentVO();


            studentRecord = validationBLObject.GetRecordUsingSrNo(id);

            return View("New", studentRecord);
        }

        public ActionResult Delete(int id)
        {
            validationBLObject.DeleteRecord(id);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [Route("Home/Index/{sortColumn}/{sortDirection}")]
        public ActionResult Index(string sortColumn, string sortDirection) 
        {
            List<StudentVO> records = validationBLObject.ReadAllRecords();

            switch (sortColumn)
            {
                case "Name":
                    records = sortDirection=="desc" ? records.OrderByDescending(x => x.Name).ToList():records.OrderBy(x => x.Name).ToList();
                    break;
                case "Course":
                    records = sortDirection=="desc" ? records.OrderByDescending(x => x.Course).ToList():records.OrderBy(x => x.Course).ToList();
                    break;
                case "College":
                    records = sortDirection == "desc" ? records.OrderByDescending(x => x.College).ToList() : records.OrderBy(x => x.College).ToList();
                    break;
                default:
                    records = records.OrderBy(x => x.SrNo).ToList();
                    break;
            }
            ViewBag.SortDirection = sortDirection == "desc" ? "asc" : "desc";

            return View(records);
        }

        public ActionResult GetAllRecords()
        {
            studentRecords = validationBLObject.ReadAllRecords();
            return Json(studentRecords,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}