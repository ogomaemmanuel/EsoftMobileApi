using ESoft.Web.Services.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace EsoftMobileApi.Models
{
    public class DbConnector
    {
        private static SqlConnection mainDbConnection = null;

        public static string DBConStr = System.Configuration.ConfigurationManager.ConnectionStrings["Esoft_WebConnection"].ToString();

        private SqlConnection sqlConn = null;
        public DbConnector()
        {
            sqlConn = new SqlConnection(DBConStr);
        }

        public SqlConnection GetConnection
        {
            get { return sqlConn; }
        }


        public static void ExecuteSQL(string Sql)
        {
            try
            {
                if (mainDbConnection == null)
                    CheckConnectionState();
                SqlCommand Cmd = new SqlCommand(Sql, mainDbConnection);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               //Utility.WriteErrorLog(ref ex);
            }
        }
        public static bool ExecuteSQL_(string Sql)
        {
            bool success = false;
            try
            {
                CheckConnectionState();

                SqlCommand Cmd = new SqlCommand(Sql, mainDbConnection);
                Cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
              //  Utility.WriteErrorLog(ref ex);
            }
            return success;
        }

        public static void ExecuteSQL(SqlCommand cmd)
        {
            try
            {
                CheckConnectionState();

                cmd.Connection = mainDbConnection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // Utility.WriteErrorLog(ref ex);
            }
        }
        public static SqlDataReader GetSqlReader(string Sql)
        {
            try
            {
                CheckConnectionState();

                SqlCommand Cmd = new SqlCommand(Sql, mainDbConnection);
                Cmd.CommandTimeout = 0;
                return Cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
              //  Utility.WriteErrorLog(ref ex);
                return null;
            }
        }

        private static void CheckConnectionState()
        {
            while (mainDbConnection == null || mainDbConnection.State != ConnectionState.Open)
            {
                if (mainDbConnection == null)
                {
                    mainDbConnection = new SqlConnection(DBConStr);
                    mainDbConnection.Open();
                }
                if (mainDbConnection.State == ConnectionState.Closed || mainDbConnection.State == ConnectionState.Broken)
                    mainDbConnection.Open();
                if (mainDbConnection.State == ConnectionState.Open)
                {
                    break;
                }
            }
        }

        public static string MainDbConnectionString()
        {
            return DBConStr;
        }

        public DbDataReader GetDBResults(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
        {
            int i = 0;
            string SQLStatement = "";
            string DataProviderName = "System.Data.SqlClient";
            DbProviderFactory dpf = DbProviderFactories.GetFactory(DataProviderName);
            DbConnection Conn = dpf.CreateConnection();
            Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MainDbConnection"].ToString();

            try
            {
                Conn.Open();
            }
            catch (System.InvalidOperationException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            DbCommand Command = dpf.CreateCommand();
            Command.Connection = Conn;
            Command.CommandTimeout = 120;
            //SqlTransaction transaction = new SqlTransaction()
            //transaction.Connection=Conn;
            //transaction.Commit

            foreach (object o in ProcedureParameters)
            {
                if (i % 2 == 0)
                    SQLStatement = SQLStatement + (string)o + "=";
                else
                {
                    if (o.GetType() == typeof(string))
                        SQLStatement = SQLStatement + " '" + (string)o + "',";
                    else if (o.GetType() == typeof(DateTime))
                        SQLStatement = SQLStatement + " '" + o.ToString() + "',";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Double))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Decimal))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Boolean))
                    {
                        if ((bool)o == true)
                            SQLStatement = SQLStatement + " 1" + ",";
                        else
                            SQLStatement = SQLStatement + " 0" + ",";
                    }
                    else
                        SQLStatement = SQLStatement + " '" + (string)o + "',";
                }
                i = i + 1;
            }
            if (SQLStatement.Length > 1)
                SQLStatement = SQLStatement.Substring(0, SQLStatement.Length - 1);

            StoredProcedure = "EXEC " + StoredProcedure + " " + SQLStatement;

            Command.CommandType = CommandType.Text;
            Command.CommandText = StoredProcedure;
            DbDataReader ResultSet = null;
            try
            {
                ResultSet = Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            return ResultSet;
        }


    }
}