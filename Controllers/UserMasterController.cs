using MYMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYMVC.Controllers
{
    public class UserMasterController : Controller
    {
        UserMaster obj = new UserMaster(DBUtil.msConnStr, DBUtil.msUserID);

        // GET: UserMaster
        public ActionResult Index()
        {
            return View(obj.get());
        }

        // GET: UserMaster/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            return View(obj.findById(id));
            //id = HttpUtility.UrlDecode(id);

            //return View(obj.findById(id));
            //if (string.IsNullOrEmpty(id))
            //    return RedirectToAction("Index");

            //var data = obj.findById(id);

            //if (data == null)
            //    return RedirectToAction("Index");

            //return View(data);
        }

        public ActionResult MyProfile()
        {
            string id = Session["UserID"]?.ToString();
            return View("Details", obj.findById(id));
        }

        // GET: UserMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMaster/Create
        [HttpPost]
        public ActionResult Create(UserMaster u)
        {
            //try
            //{
            //    // TODO: Add insert logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
            obj.save(u);
            return RedirectToAction("Index");
        }

        // GET: UserMaster/Edit/5
        public ActionResult Edit(string id)
        {
            return View(obj.findById(id));
        }

        // POST: UserMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(UserMaster u)
        {
            //try
            //{
            //    // TODO: Add update logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
            obj.update(u);
            return RedirectToAction("Index");
        }

        // GET: UserMaster/Delete/5
        public ActionResult Delete(string id)
        {
            return View(obj.findById(id));
        }

        // POST: UserMaster/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection c)
        {
            //try
            //{
            //    // TODO: Add delete logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
            obj.delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPwd, string newPwd)
        {
            string id = Session["UserID"].ToString();
            var user = obj.findById(id);

            if (user.Pwd != oldPwd)
            {
                ViewBag.Error = "Wrong Old Password";
                return View();
            }

            obj.updatePassword(id, newPwd);
            ViewBag.Msg = "Password Changed";
            return View();
        }
    }
}
