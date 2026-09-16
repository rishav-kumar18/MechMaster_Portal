using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MYMVC.Models
{
    public class UserMaster
    {
        public string UserID { get; set; }
        public string Pwd { get; set; }
        public string UserName { get; set; }
        public string MobNo { get; set; }
        public string EmailID { get; set; }
        public string UserType { get; set; }

        string connStr;
        string loginUser;

        public UserMaster() { }

        public UserMaster(string con, string user)
        {
            connStr = con;
            loginUser = user;
        }

        // 🔥 GET ALL
        public List<UserMaster> get()
        {
            List<UserMaster> list = new List<UserMaster>();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM UserDtl WHERE ISNULL(Deleted, 'N') = 'N'", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new UserMaster
                    {
                        UserID = dr["UserID"].ToString(),
                        Pwd = dr["Pwd"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        MobNo = dr["MobNo"].ToString(),
                        EmailID = dr["EmailID"].ToString(),
                        UserType = dr["UserType"].ToString()
                    });
                }
            }

            return list;
        }

        // 🔥 FIND
        public UserMaster findById(string id)
        {
            return get().FirstOrDefault(x => x.UserID == id);
        }

        // 🔥 INSERT
        public bool save(UserMaster u)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"INSERT INTO UserDtl 
                (UserID,Pwd,UserName,MobNo,EmailID,UserType)
                VALUES(@id,@pwd,@name,@mob,@email,@type)";

                SqlCommand cmd = new SqlCommand(q, con);

                cmd.Parameters.AddWithValue("@id", u.UserID);
                cmd.Parameters.AddWithValue("@pwd", u.Pwd);
                cmd.Parameters.AddWithValue("@name", u.UserName);
                cmd.Parameters.AddWithValue("@mob", u.MobNo);
                cmd.Parameters.AddWithValue("@email", u.EmailID);
                cmd.Parameters.AddWithValue("@type", u.UserType);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        // 🔥 UPDATE
        public bool update(UserMaster u)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"UPDATE UserDtl SET 
                Pwd=@pwd,UserName=@name,MobNo=@mob,EmailID=@email,UserType=@type
                WHERE UserID=@id";

                SqlCommand cmd = new SqlCommand(q, con);

                cmd.Parameters.AddWithValue("@id", u.UserID);
                cmd.Parameters.AddWithValue("@pwd", u.Pwd);
                cmd.Parameters.AddWithValue("@name", u.UserName);
                cmd.Parameters.AddWithValue("@mob", u.MobNo);
                cmd.Parameters.AddWithValue("@email", u.EmailID);
                cmd.Parameters.AddWithValue("@type", u.UserType);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        // 🔥 DELETE
        public bool delete(string id)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE UserDtl SET IsDeleted = 1 WHERE UserID=@id", con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 🔥 CHANGE PASSWORD
        public bool updatePassword(string id, string newPwd)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE UserDtl SET Pwd=@pwd WHERE UserID=@id", con);

                cmd.Parameters.AddWithValue("@pwd", newPwd);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}