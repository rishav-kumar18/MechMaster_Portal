using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYMVC.Models
{
    public class InvoiceItem
    {
        //public int ItemNo { get; set; }
        //public string ItemDesc { get; set; }
        //public string ItemType { get; set; }
        //public string ItemCategory { get; set; }

        //public decimal ItemPrice { get; set; }
        //public string ItemUOM { get; set; }
        //public int Qty { get; set; }

        //public decimal Price { get; set; } // 🔥 add this

        //public decimal SGST { get; set; }
        //public decimal CGST { get; set; }
        //public decimal IGST { get; set; }

        //public decimal SGSTAmount { get; set; }
        //public decimal CGSTAmount { get; set; }
        //public decimal IGSTAmount { get; set; }

        //public decimal TotalAmount { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }

        public string ItemType { get; set; }

        public string ItemCategory { get; set; }
        public string ItemUOM { get; set; }

        public decimal ItemPrice { get; set; }
        public decimal Qty { get; set; }

        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Net { get; set; }

        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
    }
}