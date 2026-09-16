using MYMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYMVC.Controllers
{
    public class ItemController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        //public ActionResult SearchItem(string name)
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT * FROM ItemMaster WHERE ItemDesc LIKE @n+'%'", con);
        //    cmd.Parameters.AddWithValue("@n", name);

        //    con.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    string html = "<table class='table table-bordered'>";

        //    while (dr.Read())
        //    {
        //        html += "<tr>";
        //        html += "<td>" + dr["ItemDesc"] + "</td>";
        //        html += "<td>" + dr["ItemPrice"] + "</td>";
        //        //html += "<td><button class='btn btn-success' onclick=\"SelectItem('"
        //        //        + dr["ItemDesc"] + "',"
        //        //        + dr["ItemPrice"] + ","
        //        //        + dr["IGSTRate"] + ")\">Select</button></td>";
        //        html += "<td><button class='btn btn-success' onclick=\"SelectItem('"
        //                + dr["ItemDesc"] + "',"
        //                + dr["ItemPrice"] + ","
        //                + dr["IGSTRate"] + ")\">Select</button></td>";
        //        html += "</tr>";
        //    }

        //    html += "</table>";

        //    con.Close();
        //    return Content(html);
        //}

        public ActionResult SearchItemByDesc(string name)
        {
            SqlConnection con = new SqlConnection(DBUtil.msConnStr);

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM ItemMaster WHERE ItemDesc LIKE @name", con);

            da.SelectCommand.Parameters.AddWithValue("@name", "%" + name + "%");

            DataTable dt = new DataTable();
            da.Fill(dt);

            return PartialView("SearchItemByDesc", dt);
        }

        Item lObjItem = new Item(DBUtil.msConnStr, DBUtil.msUserID);
        List<Item> lObjItems;
        // GET: Item
        public ActionResult Index()
        {
            lObjItems = lObjItem.get();
            return View(lObjItems);
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Item item = lObjItem.findById(id.Value);
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Item obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            try
            {
                bool isAdded = lObjItem.save(obj);

                if (!isAdded)
                    throw new Exception("Item not added");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Item item = lObjItem.findById(id.Value);
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Item obj)
        {
            try
            {
                bool isUpdate = lObjItem.updateById(obj);

                if (!isUpdate)
                    throw new Exception("Update failed");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            Item item = lObjItem.get().FirstOrDefault(x => x.ItemId == id);
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                bool deleted = lObjItem.delete(id);

                if (!deleted)
                    throw new Exception("Item not deleted");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
