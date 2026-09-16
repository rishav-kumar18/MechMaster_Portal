using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MYMVC.Models
{
    public class User
    {
        public string mnConnStr { get; set; }

        public User(string conn)
        {
            mnConnStr = conn;
        }
        public string UserID { get; set; }
        public string Pwd { get; set; }

        public bool Login()
        {
            bool lbResult = false;

            using (SqlConnection lobjConn = new SqlConnection(mnConnStr))
            {
                string lsQuery = @"SELECT UserID
                                   FROM UserDtl
                                   WHERE UserID = @UserID
                                   AND Pwd = @Pwd";

                using (SqlCommand lobjCmd = new SqlCommand(lsQuery, lobjConn))
                {
                    lobjCmd.Parameters.Add("@UserID", SqlDbType.VarChar, 100).Value = UserID ?? "";
                    lobjCmd.Parameters.Add("@Pwd", SqlDbType.VarChar, 100).Value = Pwd ?? "";

                    lobjConn.Open();
                    SqlDataReader lobjDr = lobjCmd.ExecuteReader();

                    if (lobjDr.Read())
                    {
                        lbResult = true;
                    }
                }
            }

            return lbResult;
        }
    }
}