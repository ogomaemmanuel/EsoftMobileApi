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
    public class CustomerManager
    {
        string customerNoMask = System.Configuration.ConfigurationManager.AppSettings["CustomerNoMask"].ToString();

        public List<CustomerBalances> GetCustomerBalances(string customerNo, DateTime endDate, List<CustomerBalances> custBalances)
        {
            custBalances = custBalances ?? new List<CustomerBalances>();
            custBalances.Clear();
            try
            {
                string sqlstatement = " CustomerBalances '" + ValueConverters.format_sql_string(customerNo) + "','" + ValueConverters.FormatSqlDate(endDate, true) + "'";
                DbDataReader reader = DbConnector.GetSqlReader(sqlstatement);
                double balance, interest, insurance, penalty, appraisal;
                while (reader.Read())
                {

                    balance = ValueConverters.StringtoDouble(reader["Balance"].ToString());
                    interest = ValueConverters.StringtoDouble(reader["IntBalance"].ToString());
                    insurance = ValueConverters.StringtoDouble(reader["InsBalance"].ToString());
                    penalty = ValueConverters.StringtoDouble(reader["PenBalance"].ToString());
                    appraisal = ValueConverters.StringtoDouble(reader["AppBalance"].ToString());

                    custBalances.Add(new CustomerBalances
                    {
                        CustomerNo = ValueConverters.ConvertNullToEmptyString(reader["CustomerNo"].ToString()),
                        AccountNo = ValueConverters.ConvertNullToEmptyString(reader["AccountNo"].ToString()),
                        CustomerName = ValueConverters.ConvertNullToEmptyString(reader["CustomerName"].ToString()),
                        Category = ValueConverters.ConvertNullToEmptyString(reader["Category"].ToString()),
                        Ttype = ValueConverters.ConvertNullToEmptyString(reader["ttype"].ToString()),
                        ProductName = ValueConverters.ConvertNullToEmptyString(reader["ProductName"].ToString()),
                        Balance = balance,
                        IntBalance = interest,
                        InsBalance = insurance,
                        PenBalance = penalty,
                        AppBalance = appraisal,
                        TotalBalance = ValueConverters.Round05(balance + interest + insurance + penalty + appraisal),
                        OtherCharges = insurance + penalty + appraisal
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ref ex);
            }
            return custBalances;
        }

        public List<CustomerDetailsView> GetCustomerDetails(string customerNo)
        {
            List<CustomerDetailsView> custRecord = null;
            if (ValueConverters.IsStringEmpty(customerNo))
            {
                return custRecord;
            }
            customerNo = ValueConverters.PADLeft(Int32.Parse(customerNoMask), customerNo.Trim(), '0');

            try
            {
                string sqlCommand = "SELECT SigningInstructions, CustomerNo,coalesce(CustomerIdNo,'') as CustomerIdNo,coalesce(CustomerName,'') as CustomerName,Locked,AccountRemarks,AccountComments,MemberType,Branch," +
                    " AccountRemarks,AccountComments,DateClosed,coalesce(MobileNo,'') as MobileNo,EmpNumber as EmploymentNo,DateOfBirth,JoiningDate,Coalesce(EmployerCode,'') as EmployerCode," +
                    " coalesce((select name from BranchSettings where branchcode=tbl_customer.Branch),'') as BranchName, " +
                    " coalesce((select MembershipName from tbl_MembershipTypes where MembershipCode=tbl_customer.MemberType),'') as MemberTypeName, " +
                    " coalesce((select DPTNAME from tbl_Departments where DptCode=tbl_customer.EmployerCode),'') as EmployerName " +
                    " from tbl_customer WHERE CUSTOMERNO ='" + ValueConverters.format_sql_string(customerNo) + "'";

                DbDataReader reader = DbConnector.GetSqlReader(sqlCommand);

                custRecord = Functions.DataReaderMapToList<CustomerDetailsView>(reader);


            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ref ex);
            }
            if (custRecord == null || custRecord.Count < 1)
            {
                custRecord = new List<CustomerDetailsView>
                {
                    new CustomerDetailsView()
                };
                custRecord[0].CustomerName = "Customer Record Not Found ";
            }
            return custRecord;
        }

    }
}