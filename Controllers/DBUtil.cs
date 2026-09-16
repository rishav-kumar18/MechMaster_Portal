using System;
using System.Configuration;

namespace MYMVC.Controllers
{
    public class DBUtil
    {
        public static string msConnStr
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            }
        }

        public static string msUserID
        {
            get
            {
                return "Rishav";
            }
        }
    }
}