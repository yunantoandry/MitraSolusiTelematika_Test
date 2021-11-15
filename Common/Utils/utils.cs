using Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class utils
    {
        public static string GetConnectionString()
        {
            string connectionString = string.Empty;
            using (DBHelper conn = new DBHelper())
            {
                connectionString = conn.ConnectionString;
            }
            return connectionString;
        }

        public static string AlertPopUp(string message)
        {
            string script = string.Empty;
            script = "window.onload = function(){ alert('" + message + "')};";

            return script;
        }
    }
}
