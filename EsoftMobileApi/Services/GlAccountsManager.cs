using ESoft.Web.Services.Common;
using EsoftMobileApi.Models;
using EsoftMobileApi.Services.common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class GlAccountsManager
    {

        public double GetGlBalance(string accountNo)
        {
            double glBalance = 0.00;
            try
            {
                string query = "Exec GetGlBalance '" + ValueConverters.format_sql_string(accountNo) + "'";
                DbDataReader reader = DbConnector.GetSqlReader(query);
                while (reader.Read())
                {
                    glBalance = ValueConverters.StringtoDouble(reader["balance"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ref ex);
            }

            return glBalance;
        }

    }
}