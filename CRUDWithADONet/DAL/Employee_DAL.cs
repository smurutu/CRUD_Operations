using System.Data;
using System.Data.SqlClient;
using CRUDWithADONet.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Plugins;

namespace CRUDWithADONet.DAL
{
    public class Employee_DAL : IDisposable
	{
        public readonly IConfiguration config;
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        private SqlConnection connection;
        private string connectionstring;

        public Employee_DAL(string connstring)
        {
            connection = new SqlConnection(connstring);
            this.connection.Open();
            connectionstring = connstring;
        }

        public void Dispose()
        {
            connection.Close();
        }

        #region Databases
        public enum DataBaseObject
        {
            HostDB,
            BrokerDB
        }

        public string GetDataBaseConnection(DataBaseObject databaseobject)
        {
            string connection_string = databaseobject switch
            {
                DataBaseObject.HostDB => connectionstring,//config["ConnectionStrings:DefaultConnection"];
                _ => connectionstring,//config["ConnectionStrings:DefaultConnection"];
            };
            return connection_string;
        }
        #endregion


        //public List<Employee> GetAll()
        //{
        //	List<Employee> employeeList = new List<Employee>();
        //	using (_connection = new SqlConnection(GetConnectionString()))
        //	{
        //		_command = _connection.CreateCommand();
        //		_command.CommandType = CommandType.StoredProcedure;
        //		_command.CommandText = "get_employees";
        //		_connection.Open();

        //		SqlDataReader dr = new SqlDataReader();

        //		//SqlDataReader dr = new SqlDataReader();

        //		while (dr.Read())
        //		{
        //			Employee employee = new Employee();



        //              }
        //	}
        //}



        public bool Insert(Employee model)
        {
            Int64 i = 0;
            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using (SqlCommand cmd = new SqlCommand("insert_employee", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@first_name", model.first_name);
                        cmd.Parameters.AddWithValue("@last_name", model.last_name);
                        cmd.Parameters.AddWithValue("@date_of_birth", model.date_of_birth);
                        cmd.Parameters.AddWithValue("@email", model.email);
                        cmd.Parameters.AddWithValue("@salary", model.salary);
                        cmd.ExecuteNonQuery();
                        i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());

                        if (i > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "AddClient | Exception ->" + ex.Message);
            }


            return false;

        }

        public DataTable GetRecords(string module, string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "")
        {
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand cmd = new SqlCommand("get_records", connect);
                using SqlDataAdapter sd = new SqlDataAdapter(cmd);
                connect.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@module", module);
                if (!string.IsNullOrEmpty(param1))
                    cmd.Parameters.AddWithValue("@param1", param1);
                if (!string.IsNullOrEmpty(param2))
                    cmd.Parameters.AddWithValue("@param2", param2);
                if (!string.IsNullOrEmpty(param3))
                    cmd.Parameters.AddWithValue("@param3", param3);
                if (!string.IsNullOrEmpty(param4))
                    cmd.Parameters.AddWithValue("@param4", param4);
                if (!string.IsNullOrEmpty(param5))
                    cmd.Parameters.AddWithValue("@param5", param5);
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                //FileLogHelper.log_message_fields("ERROR", "GetRecords | Exception ->" + ex.Message);
            }

            return dt;
        }

        public DataTable GetRecordsById(string module, Int64 id, string param1 = "", string param2 = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records_by_id", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", module);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@param1", param1);
                            cmd.Parameters.AddWithValue("@param2", param2);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception occurred: " + ex.Message);
                throw;
                //FileLogHelper.log_message_fields("ERROR", "GetRecordsById | Exception ->" + ex.Message);
            }

            return dt;
        }


        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("employee_list");

                foreach (DataRow dr in dt.Rows)
                {
                    employeeList.Add(
                    new Employee
                    {
                        id = Convert.ToInt64(dr["id"]),
                        first_name = Convert.ToString(dr["first_name"]),
                        last_name = Convert.ToString(dr["last_name"]),
                        date_of_birth = Convert.ToDateTime(dr["date_of_birth"]),
                        email = Convert.ToString(dr["email"]),
                        salary = Convert.ToDouble(dr["salary"])
                    });
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            return employeeList;
        }


        public bool Delete(Int64 id)
        {
            Int64 i = 0;
            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_employee", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id",id);
                        

                        i = (Int64)cmd.ExecuteNonQuery();

                        if (i >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }

            catch (Exception)
            {
                throw;
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "AddClient | Exception ->" + ex.Message);
            }
        }
        public bool Update(Employee model)
        {
            int i = 0;
            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_employee", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", model.id);
                        cmd.Parameters.AddWithValue("@first_name", model.first_name);
                        cmd.Parameters.AddWithValue("@last_name", model.last_name);
                        cmd.Parameters.AddWithValue("@date_of_birth", model.date_of_birth);
                        cmd.Parameters.AddWithValue("@email", model.email);
                        cmd.Parameters.AddWithValue("@salary", model.salary);

                        i = (int)cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }

            catch (Exception)
            {
                throw;
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "AddClient | Exception ->" + ex.Message);
            }
        }

    }

}

