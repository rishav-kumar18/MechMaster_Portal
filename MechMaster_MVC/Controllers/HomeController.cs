using MYMVC.Models;   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Login()
        {
            User lobjUser = new User(DBUtil.msConnStr); // ✅ fix
            return View(lobjUser);
        }

        // POST: form submit hone ke baad
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // login logic
            User iObjUser = new User(DBUtil.msConnStr); // 🔥 bahar declare karo

            try
            {
                iObjUser.UserID = collection["UserID"];
                iObjUser.Pwd = collection["Pwd"];

                if (iObjUser.Login())
                {
                    //Session["UserID"] = iObjUser.UserID;
                    Session["UserID"] = iObjUser.UserID;
                    Session["Pwd"] = iObjUser.Pwd;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(iObjUser);  
        }

        public ActionResult TestForm()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpPost]
        //public ActionResult TestForm(FormCollection lObjForm)
        //{
        //    var firstName = lObjForm["fName"];
        //    var mName = lObjForm["mName"];
        //    var lName = lObjForm["lName"];
        //    return View();
        //}

        public ActionResult TestForm(Customer iObjCustomer)
        {
            string lsFName;
            string lsLName;

            lsFName = iObjCustomer.CustFName;
            lsLName = iObjCustomer.CustLName;

            return View();
        }
    }
}