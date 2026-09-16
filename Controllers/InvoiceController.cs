using MYMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYMVC.Controllers
{
    public class InvoiceController : Controller
    {
        string cs = DBUtil.msConnStr;

        // ================= INDEX =================
        public ActionResult Index()
        {
            List<InvoiceVM> list = new List<InvoiceVM>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM [Invoice2025-26]"; // ✅ CORRECT TABLE

                SqlCommand cmd = new SqlCommand(q, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new InvoiceVM
                    {
                        InvoiceSNo = dr["InvoiceSNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["InvoiceSNo"]),

                        InvoiceDate = dr["InvoiceDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["InvoiceDate"]),
                        CustomerLastVisit = dr["CustlLastVisit"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["CustlLastVisit"]),
                        // 🔵 CUSTOMER
                        FirstName = dr["CustFName"] == DBNull.Value ? "" : dr["CustFName"].ToString(),
                        LastName = dr["CustLName"] == DBNull.Value ? "" : dr["CustLName"].ToString(),
                        MobileNo = dr["CustMobNo"] == DBNull.Value ? "" : dr["CustMobNo"].ToString(),
                        CustEmail = dr["CustEmail"] == DBNull.Value ? "" : dr["CustEmail"].ToString(),
                        CustomerType = dr["CustType"] == DBNull.Value ? "" : dr["CustType"].ToString(),
                        CustSts = dr["CustSts"] == DBNull.Value ? "" : dr["CustSts"].ToString(),

                        // 🔵 ADDRESS
                        StreetAddress = dr["CustStAddr"] == DBNull.Value ? "" : dr["CustStAddr"].ToString(),
                        AreaAddress = dr["CustArAddr"] == DBNull.Value ? "" : dr["CustArAddr"].ToString(),
                        City = dr["CustCity"] == DBNull.Value ? "" : dr["CustCity"].ToString(),
                        State = dr["CustState"] == DBNull.Value ? "" : dr["CustState"].ToString(),
                        Country = dr["CustCountry"] == DBNull.Value ? "" : dr["CustCountry"].ToString(),
                        CustPinCode = dr["CustPinCode"] == DBNull.Value ? "" : dr["CustPinCode"].ToString(),

                        // 🔵 VEHICLE
                        VehicleRegNo = dr["VehicleRegNo"] == DBNull.Value ? "" : dr["VehicleRegNo"].ToString(),
                        VehicleModel = dr["VehicleModel"] == DBNull.Value ? "" : dr["VehicleModel"].ToString(),
                        ChassisNo = dr["ChassisNo"] == DBNull.Value ? "" : dr["ChassisNo"].ToString(),
                        EngineNo = dr["EngineNo"] == DBNull.Value ? "" : dr["EngineNo"].ToString(),
                        Mileage = dr["Mileage"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mileage"]),

                        // 🔵 SERVICE
                        ServiceType = dr["ServiceType"] == DBNull.Value ? "" : dr["ServiceType"].ToString(),
                        ServiceAdvisor = dr["ServiceAssoName"] == DBNull.Value ? "" : dr["ServiceAssoName"].ToString(),
                        ServiceMobile = dr["ServiceAssoMobNo"] == DBNull.Value ? "" : dr["ServiceAssoMobNo"].ToString(),

                        // 🔵 AMOUNT
                        PartsTotal = dr["PartsTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsTotal"]),
                        LabourTotal = dr["LabourTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourTotal"]),

                        PartsCGST = dr["PartsCGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsCGSTTotal"]),
                        PartsSGST = dr["PartsSGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsSGSTTotal"]),
                        PartsIGST = dr["PartsIGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsIGSTTotal"]),

                        LabourCGST = dr["LabourCGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourCGSTTotal"]),
                        LabourSGST = dr["LabourSGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourSGSTTotal"]),
                        LabourIGST = dr["LabourIGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourIGSTTotal"]),

                        TotalTax = dr["TotalTax"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalTax"]),
                        TotalAmount = dr["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalAmount"]),
                        Discount = dr["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["DiscountAmount"]),
                        GrandTotal = dr["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrandTotal"]),

                        // 🔵 OTHER
                        Remarks = dr["InvoiceRemarks"] == DBNull.Value ? "" : dr["InvoiceRemarks"].ToString(),
                        //LastVisit = dr["Created"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["Created"])
                    });
                }
            }

            return View(list);
        }

        // ================= DELETE =================
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "DELETE FROM [Invoice2025-26] WHERE InvoiceSNo=@id";

                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // ================= DETAILS =================
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            InvoiceVM model = new InvoiceVM();
            model.Items = new List<InvoiceItem>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM [Invoice2025-26] WHERE InvoiceSNo=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.InvoiceSNo = Convert.ToInt32(dr["InvoiceSNo"]);

                    model.InvoiceDate = dr["InvoiceDate"] == DBNull.Value
                        ? DateTime.Now
                        : Convert.ToDateTime(dr["InvoiceDate"]);

                    model.CustomerLastVisit = dr["CustlLastVisit"] == DBNull.Value
                        ? DateTime.Now
                        : Convert.ToDateTime(dr["CustlLastVisit"]);

                    // CUSTOMER
                    model.FirstName = dr["CustFName"]?.ToString();
                    model.LastName = dr["CustLName"]?.ToString();
                    model.MobileNo = dr["CustMobNo"]?.ToString();
                    model.CustEmail = dr["CustEmail"]?.ToString();
                    model.CustomerType = dr["CustType"]?.ToString();
                    model.CustSts = dr["CustSts"]?.ToString();

                    // ADDRESS
                    model.StreetAddress = dr["CustStAddr"]?.ToString();
                    model.AreaAddress = dr["CustArAddr"]?.ToString();
                    model.City = dr["CustCity"]?.ToString();
                    model.State = dr["CustState"]?.ToString();
                    model.Country = dr["CustCountry"]?.ToString();
                    model.CustPinCode = dr["CustPinCode"]?.ToString();

                    // VEHICLE
                    model.VehicleRegNo = dr["VehicleRegNo"]?.ToString();
                    model.VehicleModel = dr["VehicleModel"]?.ToString();
                    model.ChassisNo = dr["ChassisNo"]?.ToString();
                    model.EngineNo = dr["EngineNo"]?.ToString();
                    model.Mileage = dr["Mileage"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mileage"]);

                    // SERVICE
                    model.ServiceType = dr["ServiceType"]?.ToString();
                    model.ServiceAdvisor = dr["ServiceAssoName"]?.ToString();
                    model.ServiceMobile = dr["ServiceAssoMobNo"]?.ToString();

                    // AMOUNT
                    model.PartsTotal = dr["PartsTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsTotal"]);
                    model.LabourTotal = dr["LabourTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourTotal"]);
                    model.TotalTax = dr["TotalTax"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalTax"]);
                    model.TotalAmount = dr["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    model.GrandTotal = dr["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrandTotal"]);

                    model.Remarks = dr["InvoiceRemarks"]?.ToString();
                }
                dr.Close();
            }

            return View(model);
        }

        // ================= EDIT GET =================
        public ActionResult Edit(int id)
        {
            InvoiceVM model = new InvoiceVM();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM [Invoice2025-26] WHERE InvoiceSNo=@id";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.InvoiceSNo = Convert.ToInt32(dr["InvoiceSNo"]);


                    model.InvoiceDate = dr["InvoiceDate"] == DBNull.Value
                        ? DateTime.Now
                        : Convert.ToDateTime(dr["InvoiceDate"]);

                    model.CustomerLastVisit = dr["CustlLastVisit"] == DBNull.Value
                        ? DateTime.Now
                        : Convert.ToDateTime(dr["CustlLastVisit"]);

                    // CUSTOMER
                    model.FirstName = dr["CustFName"]?.ToString();
                    model.LastName = dr["CustLName"]?.ToString();
                    model.MobileNo = dr["CustMobNo"]?.ToString();
                    model.CustEmail = dr["CustEmail"]?.ToString();
                    model.CustomerType = dr["CustType"]?.ToString();
                    model.CustSts = dr["CustSts"]?.ToString();

                    // ADDRESS
                    model.StreetAddress = dr["CustStAddr"]?.ToString();
                    model.AreaAddress = dr["CustArAddr"]?.ToString();
                    model.City = dr["CustCity"]?.ToString();
                    model.State = dr["CustState"]?.ToString();
                    model.Country = dr["CustCountry"]?.ToString();
                    model.CustPinCode = dr["CustPinCode"]?.ToString();

                    // VEHICLE
                    model.VehicleRegNo = dr["VehicleRegNo"]?.ToString();
                    model.VehicleModel = dr["VehicleModel"]?.ToString();
                    model.ChassisNo = dr["ChassisNo"]?.ToString();
                    model.EngineNo = dr["EngineNo"]?.ToString();
                    model.Mileage = dr["Mileage"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mileage"]);

                    // SERVICE
                    model.ServiceType = dr["ServiceType"]?.ToString();
                    model.ServiceAdvisor = dr["ServiceAssoName"]?.ToString();
                    model.ServiceMobile = dr["ServiceAssoMobNo"]?.ToString();

                    // AMOUNT
                    model.PartsTotal = dr["PartsTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsTotal"]);
                    model.LabourTotal = dr["LabourTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourTotal"]);

                    model.PartsCGST = dr["PartsCGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsCGSTTotal"]);
                    model.PartsSGST = dr["PartsSGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsSGSTTotal"]);
                    model.PartsIGST = dr["PartsIGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PartsIGSTTotal"]);

                    model.LabourCGST = dr["LabourCGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourCGSTTotal"]);
                    model.LabourSGST = dr["LabourSGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourSGSTTotal"]);
                    model.LabourIGST = dr["LabourIGSTTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LabourIGSTTotal"]);

                    model.TotalTax = dr["TotalTax"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalTax"]);
                    model.TotalAmount = dr["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    model.Discount = dr["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["DiscountAmount"]);
                    model.GrandTotal = dr["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GrandTotal"]);

                    // OTHER
                    model.Remarks = dr["InvoiceRemarks"]?.ToString();
                }
            }

            return View(model);
        }

        // ================= EDIT POST =================
        [HttpPost]
        public ActionResult Edit(InvoiceVM model)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"UPDATE [Invoice2025-26]
                     SET CustFName=@fname,
                         CustLName=@lname,
                         CustMobNo=@mob,
                         CustEmail=@email,
                         CustType=@ctype,
                         CustSts=@csts,
                         InvoiceDate=@invdate,
                         CustomerLastVisit=@CustlLastVisit,

                         CustStAddr=@staddr,
                         CustArAddr=@araddr,
                         CustCity=@city,
                         CustState=@state,
                         CustCountry=@country,
                         CustPinCode=@pincode,

                         VehicleRegNo=@vehno,
                         VehicleModel=@vehmodel,
                         ChassisNo=@chassis,
                         EngineNo=@engine,
                         Mileage=@mile,

                         ServiceType=@stype,
                         ServiceAssoName=@advisor,
                         ServiceAssoMobNo=@smobile,

                         PartsTotal=@parts,
                         LabourTotal=@labour,
                         PartsCGSTTotal=@pcgst,
                         PartsSGSTTotal=@psgst,
                         PartsIGSTTotal=@pigst,
                         LabourCGSTTotal=@lcgst,
                         LabourSGSTTotal=@lsgst,
                         LabourIGSTTotal=@ligst,

                         TotalTax=@tax,
                         TotalAmount=@total,
                         DiscountAmount=@disc,
                         GrandTotal=@grand,
                         InvoiceRemarks=@remark

                     WHERE InvoiceSNo=@id";

                SqlCommand cmd = new SqlCommand(q, con);

                cmd.Parameters.AddWithValue("@invdate", model.InvoiceDate);
                cmd.Parameters.AddWithValue("@CustlLastVisit", model.CustomerLastVisit);

                cmd.Parameters.AddWithValue("@fname", model.FirstName);
                cmd.Parameters.AddWithValue("@lname", model.LastName);
                cmd.Parameters.AddWithValue("@mob", model.MobileNo);
                cmd.Parameters.AddWithValue("@email", model.CustEmail);
                cmd.Parameters.AddWithValue("@ctype", model.CustomerType);
                cmd.Parameters.AddWithValue("@CustSts", model.CustSts);

                cmd.Parameters.AddWithValue("@staddr", model.StreetAddress);
                cmd.Parameters.AddWithValue("@araddr", model.AreaAddress);
                cmd.Parameters.AddWithValue("@city", model.City);
                cmd.Parameters.AddWithValue("@state", model.State);
                cmd.Parameters.AddWithValue("@country", model.Country);
                cmd.Parameters.AddWithValue("@pincode", model.CustPinCode);

                cmd.Parameters.AddWithValue("@vehno", model.VehicleRegNo);
                cmd.Parameters.AddWithValue("@vehmodel", model.VehicleModel);
                cmd.Parameters.AddWithValue("@chassis", model.ChassisNo);
                cmd.Parameters.AddWithValue("@engine", model.EngineNo);
                cmd.Parameters.AddWithValue("@mile", model.Mileage);

                cmd.Parameters.AddWithValue("@stype", model.ServiceType);
                cmd.Parameters.AddWithValue("@advisor", model.ServiceAdvisor);
                cmd.Parameters.AddWithValue("@smobile", model.ServiceMobile);

                cmd.Parameters.AddWithValue("@parts", model.PartsTotal);
                cmd.Parameters.AddWithValue("@labour", model.LabourTotal);
                cmd.Parameters.AddWithValue("@pcgst", model.PartsCGST);
                cmd.Parameters.AddWithValue("@psgst", model.PartsSGST);
                cmd.Parameters.AddWithValue("@pigst", model.PartsIGST);
                cmd.Parameters.AddWithValue("@lcgst", model.LabourCGST);
                cmd.Parameters.AddWithValue("@lsgst", model.LabourSGST);
                cmd.Parameters.AddWithValue("@ligst", model.LabourIGST);

                cmd.Parameters.AddWithValue("@tax", model.TotalTax);
                cmd.Parameters.AddWithValue("@total", model.TotalAmount);
                cmd.Parameters.AddWithValue("@disc", model.Discount);
                cmd.Parameters.AddWithValue("@grand", model.GrandTotal);
                cmd.Parameters.AddWithValue("@remark", model.Remarks);

                cmd.Parameters.AddWithValue("@id", model.InvoiceSNo);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public string GenerateInvoiceNo()
        {
            string prefix = "MM/INV/2025-26/";

            SqlConnection con = new SqlConnection(DBUtil.msConnStr);

            SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 InnvoiceNo FROM [Invoice2025-26] ORDER BY InvoiceSNo DESC", con);

            con.Open();

            object result = cmd.ExecuteScalar();

            int nextNo = 1;

            if (result != null)
            {
                string lastInv = result.ToString(); // MM/INV/2025-26/29

                string[] parts = lastInv.Split('/');
                int lastNo = Convert.ToInt32(parts[3]);

                nextNo = lastNo + 1;
            }

            con.Close();

            return prefix + nextNo;
        }

        // ================= CREATE =================
        SqlConnection con = new SqlConnection(DBUtil.msConnStr);

        public ActionResult Create()
        {
            InvoiceVM model = new InvoiceVM();

            model.InnvoiceNo = GenerateInvoiceNo(); // 🔥 AUTO GENERATE

            return View(model);
            //return View();
        }

        //[HttpPost]
        //public ActionResult Create(InvoiceVM model, string ItemsJson)
        //{
        //    var items = JsonConvert.DeserializeObject<List<InvoiceItem>>(ItemsJson);

        //    con.Open();

        //    SqlCommand cmd = new SqlCommand(@"
        //    INSERT INTO [Invoice2025-26]
        //    (CustFName, CustLName, CustMobNo, VehicleRegNo, VehicleModel,
        //     InnvoiceNo, InvoiceDate, CustlLastVisit,
        //     PartsTotal, LabourTotal, TotalTax, GrandTotal)
        //    OUTPUT INSERTED.InvoiceSNo
        //    VALUES
        //    (@FName,@LName,@Mob,@VehNo,@Model,@InvNo,@InvDate,@CustlLastVisit,@Parts,@Labour,@Tax,@Grand)", con);

        //    cmd.Parameters.AddWithValue("@FName", model.FirstName);
        //    cmd.Parameters.AddWithValue("@LName", model.LastName);
        //    cmd.Parameters.AddWithValue("@Mob", model.MobileNo);
        //    cmd.Parameters.AddWithValue("@VehNo", model.VehicleRegNo);
        //    cmd.Parameters.AddWithValue("@Model", model.VehicleModel);
        //    cmd.Parameters.AddWithValue("@InvNo", model.InnvoiceNo);
        //    //cmd.Parameters.AddWithValue("@InvDate", model.InvoiceDate);
        //    //cmd.Parameters.AddWithValue("@CustlLastVisit", model.CustomerLastVisit);
        //    cmd.Parameters.AddWithValue("@InvDate",
        //        model.InvoiceDate == DateTime.MinValue ? DateTime.Now : model.InvoiceDate);

        //    cmd.Parameters.AddWithValue("@CustlLastVisit",
        //        model.CustomerLastVisit == DateTime.MinValue ? DateTime.Now : model.CustomerLastVisit);
        //    cmd.Parameters.AddWithValue("@Parts", model.PartsTotal);
        //    cmd.Parameters.AddWithValue("@Labour", model.LabourTotal);
        //    cmd.Parameters.AddWithValue("@Tax", model.TotalTax);
        //    cmd.Parameters.AddWithValue("@Grand", model.GrandTotal);

        //    int invoiceId = (int)cmd.ExecuteScalar();

        //    foreach (var item in items)
        //    {
        //        SqlCommand cmd2 = new SqlCommand(@"
        //        INSERT INTO [InvoiceItem2025-26]
        //        (InnvoiceNo, ItemDesc, ItemType, ItemPrice, Qty,
        //         SGSTAmount, CGSTAmount, IGSTAmount, TotalAmount)
        //        VALUES
        //        (@InvNo,@Desc,@Type,@Price,@Qty,@SGST,@CGST,@IGST,@Total)", con);

        //        cmd2.Parameters.AddWithValue("@InvNo", model.InnvoiceNo);
        //        cmd2.Parameters.AddWithValue("@Desc", item.ItemDesc);
        //        cmd2.Parameters.AddWithValue("@Type", item.ItemType);
        //        cmd2.Parameters.AddWithValue("@Price", item.ItemPrice);
        //        cmd2.Parameters.AddWithValue("@Qty", item.Qty);
        //        cmd2.Parameters.AddWithValue("@SGST", item.SGST);
        //        cmd2.Parameters.AddWithValue("@CGST", item.CGST);
        //        cmd2.Parameters.AddWithValue("@IGST", item.IGST);
        //        cmd2.Parameters.AddWithValue("@Total", item.TotalAmount);

        //        cmd2.ExecuteNonQuery();
        //    }

        //    con.Close();

        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult Create(string model, string ItemsJson)
        {
            var invoice = JsonConvert.DeserializeObject<InvoiceVM>(model);
            var items = JsonConvert.DeserializeObject<List<InvoiceItem>>(ItemsJson);

            using (SqlConnection con = new SqlConnection(DBUtil.msConnStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
        INSERT INTO [Invoice2025-26]
        (CustNo,CustFName,CustLName,CustEmail,CustSts,CustPinCode,
         CustMobNo,CustStAddr,CustArAddr,CustCity,CustState,CustCountry,
         VehicleRegNo,VehicleModel,ChassisNo,EngineNo,Mileage,
         ServiceType,ServiceAssoName,ServiceAssoMobNo,DiscountAmount,
         InvoiceDate,InvoiceSts,CustlLastVisit,
         PartsTotal,LabourTotal,CustType,
         PartsCGSTTotal,PartsSGSTTotal,PartsIGSTTotal,
         LabourCGSTTotal,LabourSGSTTotal,LabourIGSTTotal,
         TotalTax,TotalAmount,GrandTotal,InvoiceTotal,InvoiceRemarks,Created)
         OUTPUT INSERTED.InvoiceSNo
        VALUES
        (@CustNo,@FName,@LName,@CustEmail,@CustSts,@CustPinCode,
         @Mob,@staddr,@araddr,@city,@state,@country,
         @VehNo,@Model,@Chassis,@Engine,@Mileage,
         @ServiceType,@Advisor,@ServiceMobile,@DiscountAmount,
         @InvDate,@InvoiceSts,@CustlLastVisit,
         @Parts,@Labour,@CustType,
         @PCGST,@PSGST,@PIGST,
         @LCGST,@LSGST,@LIGST,
         @Tax,@Total,@Grand,@InvoiceTotal,@Remark,GETDATE())", con);

                //cmd.Parameters.AddWithValue("@CustNo", invoice.CustNo);
                cmd.Parameters.AddWithValue("@FName", invoice.FirstName ?? "");
                cmd.Parameters.AddWithValue("@LName", invoice.LastName ?? "");
                cmd.Parameters.AddWithValue("@CustEmail", invoice.CustEmail ?? "");
                cmd.Parameters.AddWithValue("@CustPinCode", invoice.CustPinCode ?? "");
                cmd.Parameters.AddWithValue("@CustSts", invoice.CustSts ?? "ACTIVE");
                cmd.Parameters.AddWithValue("@Mob", invoice.MobileNo ?? "");
                cmd.Parameters.AddWithValue("@staddr", invoice.StreetAddress ?? "");
                cmd.Parameters.AddWithValue("@araddr", invoice.AreaAddress ?? "");
                cmd.Parameters.AddWithValue("@city", invoice.City ?? "");
                cmd.Parameters.AddWithValue("@state", invoice.State ?? "");
                cmd.Parameters.AddWithValue("@country", invoice.Country ?? "");
                cmd.Parameters.AddWithValue("@VehNo", invoice.VehicleRegNo ?? "");
                cmd.Parameters.AddWithValue("@Model", invoice.VehicleModel ?? "");
                cmd.Parameters.AddWithValue("@CustType", invoice.CustomerType ?? ""); // FIX: was missing
                cmd.Parameters.AddWithValue("@DiscountAmount", invoice.Discount);

                cmd.Parameters.AddWithValue("@InvDate", invoice.InvoiceDate == DateTime.MinValue ? DateTime.Now : invoice.InvoiceDate);
                cmd.Parameters.AddWithValue("@InvoiceSts", "SAVED");
                cmd.Parameters.AddWithValue("@CustlLastVisit", invoice.CustomerLastVisit == DateTime.MinValue ? DateTime.Now : invoice.CustomerLastVisit);

                cmd.Parameters.AddWithValue("@CustNo", invoice.CustNo == 0 ? 0 : invoice.CustNo);
                cmd.Parameters.AddWithValue("@Parts", invoice.PartsTotal == 0 ? 0 : invoice.PartsTotal);
                cmd.Parameters.AddWithValue("@Labour", invoice.LabourTotal == 0 ? 0 : invoice.LabourTotal);   
                cmd.Parameters.AddWithValue("@Tax", invoice.TotalTax == 0 ? 0 : invoice.TotalTax); 
                cmd.Parameters.AddWithValue("@Grand", invoice.GrandTotal == 0 ? 0 : invoice.GrandTotal);

                cmd.Parameters.AddWithValue("@Chassis", invoice.ChassisNo ?? "");
                cmd.Parameters.AddWithValue("@Engine", invoice.EngineNo ?? "");
                cmd.Parameters.AddWithValue("@Mileage", invoice.Mileage);

                cmd.Parameters.AddWithValue("@ServiceType", invoice.ServiceType ?? "");
                cmd.Parameters.AddWithValue("@Advisor", invoice.ServiceAdvisor ?? "");
                cmd.Parameters.AddWithValue("@ServiceMobile", invoice.ServiceMobile ?? "");

                cmd.Parameters.AddWithValue("@PCGST", invoice.PartsCGST);
                cmd.Parameters.AddWithValue("@PSGST", invoice.PartsSGST);
                cmd.Parameters.AddWithValue("@PIGST", invoice.PartsIGST);

                cmd.Parameters.AddWithValue("@LCGST", invoice.LabourCGST);
                cmd.Parameters.AddWithValue("@LSGST", invoice.LabourSGST);
                cmd.Parameters.AddWithValue("@LIGST", invoice.LabourIGST);

                cmd.Parameters.AddWithValue("@Total", invoice.TotalAmount);
                cmd.Parameters.AddWithValue("@InvoiceTotal", invoice.GrandTotal);

                cmd.Parameters.AddWithValue("@Remark", invoice.Remarks ?? "");

                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    throw new Exception("Invoice ID not generated!");
                }

                int invoiceId = Convert.ToInt32(result);

                foreach (var item in items)
                {
                    SqlCommand cmd2 = new SqlCommand(@"
            INSERT INTO [InvoiceItem2025-26]
            (InvoiceSNo,ItemNo,ItemDesc, ItemType,ItemCatg,ItemUOM,ItemSts, ItemPrice,DiscountAmount, Qty,
             SGSTAmount, CGSTAmount, IGSTAmount, TotalAmount)
            VALUES
            (@InvoiceSNo,@ItemNo,@Desc,@Type,@Catg,@UOM,@ItemSts,@Price,@DiscountAmount,@Qty,@SGST,@CGST,@IGST,@Total)", con);

                    cmd2.Parameters.AddWithValue("@InvoiceSNo", invoiceId); // 🔥 MAIN FIX
                    cmd2.Parameters.AddWithValue("@ItemNo", item.ItemNo);
                    cmd2.Parameters.AddWithValue("@Desc", item.ItemDesc ?? "");
                    cmd2.Parameters.AddWithValue("@Type", item.ItemType ?? "");
                    cmd2.Parameters.AddWithValue("@Catg", item.ItemCategory ?? "-");
                    cmd2.Parameters.AddWithValue("@UOM", item.ItemUOM ?? "PCS");
                    cmd2.Parameters.AddWithValue("@ItemSts", "ACTIVE");
                    cmd2.Parameters.AddWithValue("@Price", item.ItemPrice);
                    cmd2.Parameters.AddWithValue("@DiscountAmount", item.Discount);
                    cmd2.Parameters.AddWithValue("@Qty", item.Qty);
                    cmd2.Parameters.AddWithValue("@SGST", item.SGST);
                    cmd2.Parameters.AddWithValue("@CGST", item.CGST);
                    cmd2.Parameters.AddWithValue("@IGST", item.IGST);
                    cmd2.Parameters.AddWithValue("@Total", item.TotalAmount);

                    cmd2.ExecuteNonQuery();
                }
            }

            return Json(new { success = true });
        }
    }
}
