using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MYMVC.Models
{
    public class Item
    {
        public int ItemNo { get; set; }      // ItemNo
        public int ItemId { get; set; }      // ItemNo
        public string ItemName { get; set; } // ItemDesc

        public string ItemDesc { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; } // ItemCatg
        public decimal ItemPrice { get; set; }
        public int QtyInHand { get; set; }   // QtyHand
        public string Remarks { get; set; }

        // 🔥 NEW FIELDS
        public string ItemUOM { get; set; }
        public string ItemSts { get; set; }

        public string mnConnStr { get; set; }
        public string msUserID { get; set; }

        public Item() { }

        public Item(string connStr, string userId)
        {
            mnConnStr = connStr;
            msUserID = userId;
        }

        // 🔥 INSERT
        public bool save(Item obj)
        {
            string query = @"INSERT INTO ItemMaster
            (ItemDesc, ItemType, ItemCatg, ItemPrice, QtyHand, ItemRemarks, ItemUOM, ItemSts)
            VALUES (@Name, @Type, @Category, @Price, @Qty, @Remarks, @UOM, @Sts)";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", obj.ItemName);
                cmd.Parameters.AddWithValue("@Type", obj.ItemType);
                cmd.Parameters.AddWithValue("@Category", obj.ItemCategory);
                cmd.Parameters.AddWithValue("@Price", obj.ItemPrice);
                cmd.Parameters.AddWithValue("@Qty", obj.QtyInHand);
                cmd.Parameters.AddWithValue("@Remarks", obj.Remarks ?? "");
                cmd.Parameters.AddWithValue("@UOM", obj.ItemUOM);
                cmd.Parameters.AddWithValue("@Sts", obj.ItemSts);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        // 🔥 GET ALL
        public List<Item> get()
        {
            List<Item> list = new List<Item>();

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM ItemMaster WHERE ISNULL(Deleted, 'N') = 'N'", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Item
                    {
                        ItemId = Convert.ToInt32(dr["ItemNo"]),
                        ItemName = dr["ItemDesc"].ToString(),
                        ItemType = dr["ItemType"].ToString(),
                        ItemCategory = dr["ItemCatg"].ToString(),
                        ItemPrice = Convert.ToDecimal(dr["ItemPrice"]),
                        QtyInHand = Convert.ToInt32(dr["QtyHand"]),
                        Remarks = dr["ItemRemarks"].ToString(),
                        ItemUOM = dr["ItemUOM"].ToString(),
                        ItemSts = dr["ItemSts"].ToString()
                    });
                }
            }
            return list;
        }

        // 🔥 FIND
        public Item findById(int id)
        {
            Item obj = null;

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ItemMaster WHERE ItemNo=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj = new Item
                    {
                        ItemId = Convert.ToInt32(dr["ItemNo"]),
                        ItemName = dr["ItemDesc"].ToString(),
                        ItemType = dr["ItemType"].ToString(),
                        ItemCategory = dr["ItemCatg"].ToString(),
                        ItemPrice = Convert.ToDecimal(dr["ItemPrice"]),
                        QtyInHand = Convert.ToInt32(dr["QtyHand"]),
                        Remarks = dr["ItemRemarks"].ToString(),
                        ItemUOM = dr["ItemUOM"].ToString(),
                        ItemSts = dr["ItemSts"].ToString()
                    };
                }
            }
            return obj;
        }

        // 🔥 UPDATE
        public bool updateById(Item obj)
        {
            string query = @"UPDATE ItemMaster 
            SET ItemDesc=@Name, ItemType=@Type, ItemCatg=@Category,
                ItemPrice=@Price, QtyHand=@Qty, ItemRemarks=@Remarks,
                ItemUOM=@UOM, ItemSts=@Sts
            WHERE ItemNo=@Id";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", obj.ItemId);
                cmd.Parameters.AddWithValue("@Name", obj.ItemName);
                cmd.Parameters.AddWithValue("@Type", obj.ItemType);
                cmd.Parameters.AddWithValue("@Category", obj.ItemCategory);
                cmd.Parameters.AddWithValue("@Price", obj.ItemPrice);
                cmd.Parameters.AddWithValue("@Qty", obj.QtyInHand);
                cmd.Parameters.AddWithValue("@Remarks", obj.Remarks ?? "");
                cmd.Parameters.AddWithValue("@UOM", obj.ItemUOM);
                cmd.Parameters.AddWithValue("@Sts", obj.ItemSts);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        // 🔥 DELETE
        public bool delete(int id)
        {
            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE ItemMaster SET Deleted='Y', DeletedOn=GETDATE(), DeletedBy=@userId WHERE ItemNo=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userId", msUserID ?? "");

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}