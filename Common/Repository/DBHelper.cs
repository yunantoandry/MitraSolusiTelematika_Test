using Common.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository
{
        //
        [AttributeUsage(AttributeTargets.Property)]
        public class DapperKey : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class DapperIgnore : Attribute
        {
        }

        public class DBHelper : IDBHelper, IDisposable
        {
            #region Fields

            private SqlConnection _conn = null;
            private IDbTransaction _dbTransaction = null;
            private string _ConnString = null;
            private IDbTransaction _DbTransaction = null;

            #endregion

            #region Constructors

            public DBHelper()
            {
                _ConnString = null;
                _DbTransaction = null;
                this.Open();
            }

            public DBHelper(string connString)
            {
                _ConnString = connString.Trim() as string;
                _DbTransaction = null;

                this.Open();
            }

            #endregion

            public string ConnectionString
            {
                get
                {
                    string result = (_ConnString != null) ? _ConnString : ServiceConfiguration.GetConnectionstring();
                    return result;
                }

                set { _ConnString = value; }
            }
            public IDbConnection Connection
            {
                get { return _conn; }
                set { _conn = (SqlConnection)value; }
            }

            public IDbTransaction DbTransaction
            {
                get { return _dbTransaction; }
                set { _dbTransaction = value; }
            }

            #region Methods

            private void Open()
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection(ConnectionString);

                }

                if (_conn.State == System.Data.ConnectionState.Broken) _conn.Close();
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    _conn.Open();
                }
            }

            private void Close()
            {
                if (_conn != null && _conn.State != System.Data.ConnectionState.Closed)
                    _conn.Close();
            }

            public static bool CheckConnection(string connectionString)
            {
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    conn.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    System.Diagnostics.Trace.WriteLine("Check connection failed due to " + ex.Message);
                    return false;
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }

            public IDbTransaction OpenTransaction()
            {
                _DbTransaction = _conn.BeginTransaction(IsolationLevel.RepeatableRead);
                return _DbTransaction;
            }

            private IDbCommand GetSqlCommand()
            {
                if (Connection.State == ConnectionState.Closed) Open();
                IDbCommand cmd = Connection.CreateCommand();
                return cmd;
            }

            private IDbCommand CreateCommand(string sql)
            {
                IDbCommand cmd = GetSqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                return cmd;
            }
            private IDbCommand CreateCommand(string sql, CommandType cmdType)
            {
                IDbCommand cmd = GetSqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                return cmd;
            }

            public IDbCommand CreateCommand(string cmdText, CommandType cmdType, params object[] obj)
            {
                IDbCommand cmd = GetSqlCommand();
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                // Add parameters
                for (int i = 0; i < obj.Length; i++)
                    cmd.Parameters.Add(obj[i]);
                //cmd.ExecuteNonQuery();
                return cmd;
            }

            public object DoExecuteNonQuery(string sql, CommandType cmdType)
            {
                IDbCommand cmd = CreateCommand(sql, cmdType);
                return cmd.ExecuteNonQuery();
            }


            public object DoExecuteScalar(string sql)
            {
                IDbCommand cmd = CreateCommand(sql);
                return cmd.ExecuteScalar();
            }

            public IDataReader DoExecuteReader(string sql)
            {
                IDbCommand cmd = CreateCommand(sql);
                return cmd.ExecuteReader();
            }


            public IDbCommand DoProcCommand(string procName, params object[] obj)
            {
                return CreateCommand(procName, CommandType.StoredProcedure, obj);
            }

            public DataTable DoStoredTable(string procName, params IDbDataParameter[] parameters)
            {
                IDbCommand cmd = DoProcCommand(procName, parameters);
                SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }

            public IEnumerable<T> Select<T>(Dictionary<string, object> args)
            {
                var typeName = typeof(T).Name;
                string inital = typeName.Substring(0, 1);
                var sql = string.Empty;

                if (inital == "M")
                    sql = string.Format("SELECT * FROM MASTER_{0}", typeof(T).Name);
                else
                    sql = string.Format("SELECT * FROM {0}", typeof(T).Name);

                return GetItems<T>(CommandType.Text, CreateSQlStatement(sql, args), null);
            }

            public IEnumerable<T> Select<T>(string args, object parameters = null)
            {
                var typeName = typeof(T).Name;
                string inital = typeName.Substring(0, 1);
                var sql = string.Empty;

                if (inital == "M")
                    sql = string.Format("SELECT * FROM MASTER_{0} {1}", typeof(T).Name, args);
                else
                    sql = string.Format("SELECT * FROM {0} {1}", typeof(T).Name, args);
                return GetItems<T>(CommandType.Text, sql, parameters);
            }


            public dynamic ToExpandoObject(object value)
            {
                IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

                IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();

                foreach (KeyValuePair<string, object> property in dapperRowProperties)
                    expando.Add(property.Key, property.Value);

                return expando as System.Dynamic.ExpandoObject;
            }

            private string CreateSQlStatement(string sql, Dictionary<string, object> args)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(sql);
                sb.Append(" WHERE 1=1");

                if (args != null)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    foreach (var pair in args)
                    {
                        var type = pair.Value.GetType();
                        if (pair.Value != null)
                        {
                            if (type == typeof(int) || type == typeof(double) || type == typeof(decimal) || type == typeof(float))
                                sb.AppendFormat(" AND {0}={1} ", pair.Key, pair.Value);
                            if (type == typeof(DateTime))
                            {
                                if (Convert.ToDateTime(pair.Value).Year != 1)
                                    sb.AppendFormat(" AND {0}='{1}' ", pair.Key, Convert.ToDateTime(pair.Value).ToString("yyyy-MM-dd"));
                            }
                            else
                                sb.AppendFormat(" AND {0}='{1}' ", pair.Key, pair.Value);
                        }
                    }

                }
                return sb.ToString();
            }
            public int Insert<T>(T obj) // DB Master
            {
                var typeName = typeof(T).Name;
                string inital = typeName.Substring(0, 1);
                int rowsAffected = 0;
                var sql = string.Empty;
                try
                {

                    var propertyContainer = ParseProperties(obj);

                    if (inital == "M")
                        sql = string.Format("INSERT INTO MASTER_{0} ({1}) VALUES (@{2})", typeof(T).Name,
                        string.Join(", ", propertyContainer.ValueNames),
                        string.Join(", @", propertyContainer.ValueNames));
                    else
                        sql = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", typeof(T).Name,
                        string.Join(", ", propertyContainer.ValueNames),
                        string.Join(", @", propertyContainer.ValueNames));

                    rowsAffected = Connection.Execute(sql, propertyContainer.ValuePairs, commandType: CommandType.Text);
                }
                catch
                {
                    throw;
                }
                return rowsAffected;
            }

            public int Update<T>(T obj, Dictionary<string, object> args)
            {
                var typeName = typeof(T).Name;
                string inital = typeName.Substring(0, 1);
                int rowsAffected = 0;
                var sql = string.Empty;

                try
                {
                    var propertyContainer = ParseProperties(obj);
                    var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
                    var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);

                    if (!string.IsNullOrEmpty(sqlIdPairs))
                    {
                        if (inital == "M")
                            sql = CreateSQlStatement(string.Format("UPDATE MASTER_{0} SET {1} ", typeof(T).Name, sqlValuePairs), args);
                        else
                        {
                            sql = CreateSQlStatement(string.Format("UPDATE {0} SET {1} ", typeof(T).Name, sqlValuePairs), args);
                        }
                        sql = sql.Replace(" 1=1 AND", "");
                        rowsAffected = Connection.Execute(sql, obj);
                    }
                    else
                    {
                        sql = string.Format("UPDATE {0} SET {1} WHERE {2}", typeof(T).Name, sqlValuePairs, sqlIdPairs);
                        rowsAffected = Execute(CommandType.Text, sql, propertyContainer.IdPairs);
                    }
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                    throw;
                }
                return rowsAffected;
            }
            public int Delete<T>(T obj)
            {
                var typeName = typeof(T).Name;
                string inital = typeName.Substring(0, 1);
                int rowsAffected = 0;
                var sql = string.Empty;
                Dictionary<string, object> args = new Dictionary<string, object>();

                try
                {
                    var propertyContainer = ParseProperties(obj);
                    var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);

                    if (!string.IsNullOrEmpty(sqlIdPairs))
                    {
                        if (inital == "M")
                            sql = string.Format("DELETE FROM MASTER_{0} WHERE {1}", typeof(T).Name, sqlIdPairs);
                        else
                            sql = string.Format("DELETE FROM {0} WHERE {1}", typeof(T).Name, sqlIdPairs);
                        rowsAffected = Execute(CommandType.Text, sql, propertyContainer.IdPairs);
                    }
                    else
                    {
                        foreach (var property in propertyContainer.AllPairs)
                        {
                            if (property.Value != null)
                                args.Add(property.Key, property.Value);
                        }
                        if (inital == "M")
                            sql = CreateSQlStatement(string.Format("DELETE FROM MASTER_{0}", typeof(T).Name), args);
                        else
                            sql = CreateSQlStatement(string.Format("DELETE FROM {0}", typeof(T).Name), args);
                        sql = sql.Replace(" 1=1 AND", "");
                        rowsAffected = Execute(CommandType.Text, sql);
                    }
                }
                catch
                {
                    throw;
                }
                return rowsAffected;
            }

            public IDataReader DoExecuteDataReader(string sql)
            {
                IDbCommand cmd = CreateCommand(sql);
                IDataReader dr = cmd.ExecuteReader();
                return dr;
            }


            public DataTable DoExecuteTable(string sql)
            {
                IDbCommand cmd = CreateCommand(sql);
                SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            private PropertyContainer ParseProperties<T>(T obj)
            {
                var propertyContainer = new PropertyContainer();

                var typeName = typeof(T).Name;
                var validKeyNames = new[] { "ID",
            string.Format("{0}ID", typeName), string.Format("{0}_ID", typeName) };

                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    // Skip reference types (but still include string!)
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        continue;

                    // Skip methods without a public setter
                    if (property.GetSetMethod() == null)
                        continue;

                    // Skip methods specifically ignored
                    if (property.IsDefined(typeof(DapperIgnore), false))
                        continue;

                    var name = property.Name;
                    var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                    if (property.IsDefined(typeof(DapperKey), false) || validKeyNames.Contains(name))
                    {
                        propertyContainer.AddId(name, value);
                    }
                    else
                    {
                        propertyContainer.AddValue(name, value);
                    }
                }

                return propertyContainer;
            }

            private string GetSqlPairs(IEnumerable<string> keys, string separator = ", ")
            {
                var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
                return string.Join(separator, pairs);
            }

            public int SetId<T>(T obj, int id, IDictionary<string, object> propertyPairs)
            {
                if (propertyPairs.Count == 1)
                {
                    var propertyName = propertyPairs.Keys.First();
                    var propertyInfo = obj.GetType().GetProperty(propertyName);
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        propertyInfo.SetValue(obj, id, null);
                    }
                }

                return id;
            }

            public IEnumerable<T> GetItems<T>(CommandType commandType, string sql, object parameters = null)
            {
                return Connection.Query<T>(sql, parameters, commandType: commandType);
            }

            public int Execute(CommandType commandType, string sql, object parameters = null)
            {
                return Connection.Execute(sql, parameters, commandType: commandType);
            }

            public List<T> DataReaderMapToList<T>(IDataReader dr)
            {
                List<T> list = new List<T>();
                T obj = default(T);

                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {

                        if (!object.Equals(dr[prop.Name], DBNull.Value))

                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }

            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this); // Finalization is now unnecessary
            }

            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_conn != null)
                    {
                        Close();
                        _conn.Dispose();
                        _conn = null;
                    }

                    _dbTransaction = null;
                }
            }

            ~DBHelper()
            {
                Dispose(true);
            }
            #endregion
        }
    
}
