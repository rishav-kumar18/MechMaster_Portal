using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYMVC.Models
{
    public class InvoiceVM
    {
        public string InnvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CustomerLastVisit { get; set; }
        public int InvoiceSNo { get; set; }
        // 🔵 CUSTOMER
        public string MobileNo { get; set; }
        public int CustNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string CustSts { get; set; }
        public string StreetAddress { get; set; }
        public string AreaAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CustPinCode { get; set; }
        public string CustEmail { get; set; }
        public string Remarks { get; set; }
        //public DateTime LastVisit { get; set; }

        // 🟢 VEHICLE
        public string VehicleRegNo { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string VehicleModel { get; set; }
        public int Mileage { get; set; }
        public string ServiceType { get; set; }
        public string ServiceAdvisor { get; set; }
        public string ServiceMobile { get; set; }

        // 🟡 BILL SUMMARY
        public decimal PartsTotal { get; set; }
        public decimal PartsCGST { get; set; }
        public decimal PartsSGST { get; set; }
        public decimal PartsIGST { get; set; }

        public decimal LabourTotal { get; set; }
        public decimal LabourCGST { get; set; }
        public decimal LabourSGST { get; set; }
        public decimal LabourIGST { get; set; }

        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }

        // 🔴 ITEMS
        public List<InvoiceItem> Items { get; set; }
    }
}