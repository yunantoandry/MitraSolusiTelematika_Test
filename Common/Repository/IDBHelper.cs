using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IDBHelper
    {
        IEnumerable<T> Select<T>(Dictionary<string, object> args);
        IEnumerable<T> Select<T>(string args, object parameters = null);
        //IDbConnection Connection();
        dynamic ToExpandoObject(object value);
        int Insert<T>(T obj);
        int Update<T>(T obj, Dictionary<string, object> args);
        int Delete<T>(T obj);
        List<T> DataReaderMapToList<T>(IDataReader dr);
        DataTable ToDataTable<T>(List<T> items);
        object DoExecuteNonQuery(string sql, CommandType cmdType);
        object DoExecuteScalar(string sql);
        IDataReader DoExecuteReader(string sql);
    }
}
