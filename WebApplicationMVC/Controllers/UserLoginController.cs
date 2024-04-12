using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VO;

namespace WebApplicationMVC.Controllers
{
    public class UserLoginController : Controller
    {
        ValidationBL validationBLObject = new ValidationBL();

        // GET: Login
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(UserVO userVO)
        {
            if (validationBLObject.isUserExist(userVO))
            {
                HttpContext.Application["IsAuthenticated"] = true;
                return RedirectToAction("Index","Home");
            }
            else 
            {
                return View();
            }
        }
    }
}