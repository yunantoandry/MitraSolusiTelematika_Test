using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class ServiceConfiguration
    {
        public static string GetConnectionstring()
        {
            string connectionStringName = "ConnectionString";
            var connectionStringElement = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringElement == null)
            {
                throw new InvalidOperationException($"The Web.config is missing a connection string called '{connectionStringName}'.");
            }
            return connectionStringElement.ConnectionString;
        }

        public static string GetAppSettingValue(string paramName)
        {
            return String.Format(ConfigurationManager.AppSettings[paramName]);
        }
    }
}
