using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MYMVC.Controllers
{
    public class Customer
    {
        //[Display(Name = "First Name")]
        //public string FName { get; set; }


        //[Display(Name = "Middle Name")]
        //public string MName { get; set; }

        //[Display(Name = "Last Name")]

        //public string LName { get; set; }

        public int CustNo { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter First Name")]
        public string CustFName { get; set; }



        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Last Name")]
        public string CustLName { get; set; }


        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter Mobile No")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile No not in correct format")]
        public string CustMobNo { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string CustEmail { get; set; }

        [Display(Name = "StAddr")]
        [Required(ErrorMessage = "Please enter StAddr")]
        public string CustStAddr { get; set; }

        public string mnConnStr { get; set; }
        public string msUserID { get; set; }

        public string CustState { get; set; }

        public string CustCity { get; set; }



        public Customer()
        {
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        public List<Customer> SearchCustomer(string mob)
        {
            List<Customer> list = new List<Customer>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE CustMobNo LIKE @m+'%'", con);
            cmd.Parameters.AddWithValue("@m", mob);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Customer
                {
                    CustNo = Convert.ToInt32(dr["CustNo"]),
                    CustFName = dr["CustFName"].ToString(),
                    CustMobNo = dr["CustMobNo"].ToString()
                });
            }

            con.Close();
            return list;
        }

        public Customer LoadCustomer(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE CustNo=@id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            Customer c = new Customer();

            if (dr.Read())
            {
                c.CustNo = id;
                c.CustFName = dr["CustFName"].ToString();
                c.CustLName = dr["CustLName"].ToString();
                c.CustEmail = dr["CustEmail"].ToString();
                c.CustMobNo = dr["CustMobNo"].ToString();
                c.CustState = dr["CustState"].ToString();
                c.CustCity = dr["CustCity"].ToString();
            }

            con.Close();
            return c;
        }
        public Customer(string connStr, string userId)
        {
            this.mnConnStr = connStr;
            this.msUserID = userId;
        }

        public Customer(string isFName, string isLName,
                        string isaddress, string isCustMobNo, string isEmail)
        {
            CustFName = isFName;
            CustLName = isLName;
            CustStAddr = isaddress;
            CustMobNo = isCustMobNo;
            CustEmail = isEmail;
        }

        public bool save(Customer obj)
        {
            string query = @"INSERT INTO Customer 
            (CustFName, CustLName, CustMobNo, CustEmail, CustStAddr)
            VALUES (@FName, @LName, @Mob, @Email, @Addr)";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FName", obj.CustFName);
                cmd.Parameters.AddWithValue("@LName", obj.CustLName);
                cmd.Parameters.AddWithValue("@Mob", obj.CustMobNo);
                cmd.Parameters.AddWithValue("@Email", obj.CustEmail ?? "");
                cmd.Parameters.AddWithValue("@Addr", obj.CustStAddr);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        // ================= GET =================
        public List<Customer> get()
        {
            List<Customer> list = new List<Customer>();

            string query = "SELECT * FROM Customer WHERE ISNULL(Deleted,'N')='N'";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Customer
                    {
                        CustNo = Convert.ToInt32(dr["CustNo"]),
                        CustFName = dr["CustFName"].ToString(),
                        CustLName = dr["CustLName"].ToString(),
                        CustMobNo = dr["CustMobNo"].ToString(),
                        CustEmail = dr["CustEmail"].ToString(),
                        CustStAddr = dr["CustStAddr"].ToString()
                    });
                }
            }

            return list;
        }

        // ================= FIND BY ID =================
        public Customer findById(int id)
        {
            Customer obj = null;

            string query = "SELECT * FROM Customer WHERE CustNo=@Id";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj = new Customer
                    {
                        CustNo = Convert.ToInt32(dr["CustNo"]),
                        CustFName = dr["CustFName"].ToString(),
                        CustLName = dr["CustLName"].ToString(),
                        CustMobNo = dr["CustMobNo"].ToString(),
                        CustEmail = dr["CustEmail"].ToString(),
                        CustStAddr = dr["CustStAddr"].ToString()
                    };
                }
            }

            return obj;
        }


        // ================= UPDATE =================
        public bool updateById(Customer obj)
        {
            string query = @"UPDATE Customer 
            SET CustFName=@FName, CustLName=@LName, CustMobNo=@Mob, 
                CustEmail=@Email, CustStAddr=@Addr
            WHERE CustNo=@Id";

            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", obj.CustNo);
                cmd.Parameters.AddWithValue("@FName", obj.CustFName);
                cmd.Parameters.AddWithValue("@LName", obj.CustLName);
                cmd.Parameters.AddWithValue("@Mob", obj.CustMobNo);
                cmd.Parameters.AddWithValue("@Email", obj.CustEmail ?? "");
                cmd.Parameters.AddWithValue("@Addr", obj.CustStAddr);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool delete(int id)
        {
            using (SqlConnection con = new SqlConnection(mnConnStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Customer SET Deleted='Y' WHERE CustNo=@Id", con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}