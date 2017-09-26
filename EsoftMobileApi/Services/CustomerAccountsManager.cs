using ESoft.Web.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web;
using EsoftMobileApi;
using EsoftMobileApi.Models;

namespace ESoft.Web.Services.Registry
{
    public class CustomerAccountsManager
    {
        private readonly Esoft_WebEntities mainDb = new Esoft_WebEntities();


        public CustomerAccountsManager()
        {

        }



        private List<tbl_CustomerAccounts> GetCustomerSavingsAccounts(String customerNo)
        {
            List<tbl_CustomerAccounts> accounts = mainDb.tbl_CustomerAccounts.Where(x => (x.CustomerNo ?? String.Empty) == (customerNo ?? String.Empty)).OrderBy(x => x.AccountNo).Select(x => x).ToList();
            return accounts;
        }

        public List<AccountDetails> GetSavingsAccountBalances(String customerNo)
        {
            List<AccountDetails> accountsDetails = new List<AccountDetails>();
            List<tbl_CustomerAccounts> accounts = GetCustomerSavingsAccounts(customerNo);
            foreach (var account in accounts)
            {
                AccountDetails singleAccountDetails = new AccountDetails();
                List<CustomerSavings> customerSavings = GetCustomerSavings(account.AccountNo);
                singleAccountDetails.AccountBalance = customerSavings.Sum(x => x.DEBIT - x.CREDIT) ?? 0;
                singleAccountDetails.AccountName = account.AccountNo;
                accountsDetails.Add(singleAccountDetails);
            }
            return accountsDetails;
        }


        public List<CustomerSavings> GetCustomerSavings(String accountNo)
        {
            List<CustomerSavings> customerSavings = new List<CustomerSavings>();

            try
            {
                DateTime startDate = new DateTime(1990, 1, 1);
                DateTime endDate = DateTime.Now;

                String query = "Exec getcustomersavings '" + accountNo + "' ,'" +
                               ValueConverters.FormatSqlDate(startDate) + "' ,'" +
                               ValueConverters.FormatSqlDate(endDate) + "' ";
                DbDataReader reader = DbConnector.GetSqlReader(query);
                customerSavings= this.mainDb.Database.SqlQuery<CustomerSavings>(query).ToList();


            }
            catch (Exception e)
            {
                //log here
            }

            return customerSavings;
        }



    }



}
