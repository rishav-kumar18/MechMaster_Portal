using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYMVC.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult SearchCustomerByMobileNo(string isMobileNo)
        {
            SqlConnection con = new SqlConnection(DBUtil.msConnStr);

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Customer WHERE CustMobNo LIKE @mob", con);

            da.SelectCommand.Parameters.AddWithValue("@mob", "%" + isMobileNo + "%");

            DataTable dt = new DataTable();
            da.Fill(dt);

            return PartialView(dt); // ⚠️ IMPORTANT
        }

        public JsonResult LoadCustomerJSON(int inCustNo)
        {
            SqlConnection con = new SqlConnection(DBUtil.msConnStr);

            SqlCommand cmd = new SqlCommand(@"
        SELECT TOP 1 
            c.*, 
            i.InnvoiceNo, 
            i.InvoiceDate,
            c.CustLastVisit
        FROM Customer c
        LEFT JOIN [Invoice2025-26] i ON c.CustNo = i.CustNo
        WHERE c.CustNo = @id
        ORDER BY i.InvoiceDate DESC
    ", con);
            cmd.Parameters.AddWithValue("@id", inCustNo);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            object data = null;

            if (dr.Read())
            {
                data = new
                {
                    CustNo = dr["CustNo"].ToString(),
                    CustFName = dr["CustFName"].ToString(),
                    CustLName = dr["CustLName"].ToString(),
                    CustMobNo = dr["CustMobNo"].ToString(),
                    CustEmail = dr["CustEmail"].ToString(),
                    CustType = dr["CustType"].ToString(),
                    CustSts = dr["CustSts"].ToString(),
                    CustStAddr = dr["CustStAddr"].ToString(),
                    CustArAddr = dr["CustArAddr"].ToString(),
                    CustCity = dr["CustCity"].ToString(),
                    CustState = dr["CustState"].ToString(),
                    CustPinCode = dr["CustPinCode"].ToString(),
                    CustCountry = dr["CustCountry"].ToString(),
                    CustRemarks = dr["CustRemarks"].ToString(),

                    InnvoiceNo = dr["InnvoiceNo"] == DBNull.Value ? "" : dr["InnvoiceNo"].ToString(),
                    InvoiceDate = dr["InvoiceDate"] == DBNull.Value ? "" : Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd"),
                    CustlLastVisit = dr["CustLastVisit"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CustLastVisit"]).ToString("yyyy-MM-dd")

                };
            }

            con.Close();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        Customer lObjCus = new Customer(DBUtil.msConnStr, DBUtil.msUserID);
        List<Customer> lObjCustomers;
        // GET: Customer
        public ActionResult Index()
        {
            lObjCustomers = lObjCus.get();
            return View(lObjCustomers);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }


            Customer cust = lObjCus.findById(id.Value);
            return View(cust);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer lObjCustomer)
        {
            if (lObjCustomer.CustFName == "pandey")
            {
                ModelState.AddModelError("CustFName", "Pandey is Common Last Name");
                return View(lObjCustomer);
            }

            if (!ModelState.IsValid)
            {
                return View(lObjCustomer);
            }
            try
            {
                bool isAdded = lObjCus.save(lObjCustomer);

                if (!isAdded)
                {
                    throw new Exception("Customer not added");

                }

                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            Customer cust = lObjCus.findById(id);
            return View(cust);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer lObjCustomer)
        {
            try
            {
                // TODO: Add update logic here
                bool isUpdate = lObjCus.updateById(lObjCustomer);

                if (!isUpdate)
                {
                    throw new Exception("Update failed");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            Customer cust = lObjCus.get().FirstOrDefault(c => c.CustNo == id);
            return View(cust);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                bool deleted = lObjCus.delete(id);
                if (!deleted)
                {
                    throw new Exception("Customer not deleted");
                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
