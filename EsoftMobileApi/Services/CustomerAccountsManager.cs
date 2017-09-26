﻿using ESoft.Web.Services.Common;
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
                customerSavings = this.mainDb.Database.SqlQuery<CustomerSavings>(query).ToList();


            }
            catch (Exception e)
            {
                //log here
            }

            return customerSavings;
        }

        public List<ProductsView> GetCustomerProducts(List<ProductsView> customerProducts, string customerno,
               List<CustomerBalances> custBalances)
        {
            customerProducts = customerProducts == null ? new List<ProductsView>() : customerProducts;
            var investmentProducts = mainDb.tbl_InvestmentCodes.ToList();
            if (investmentProducts != null && investmentProducts.Count > 0)
            {
                foreach (var item in investmentProducts)
                {
                    customerProducts.Add(new ProductsView
                    {
                        ProductType = "INVESTMENTS",
                        ProductCode = item.InvestmentCode,
                        ProductName = item.InvestmentName.Trim() + ": " + item.InvestmentCode.Trim()

                    });
                }
            }
            var existingLoanCodes = mainDb.tbl_LoanMasterTable.Where(x => x.customerNo == customerno).ToList();
            var loanCodes = mainDb.tbl_LoanCodes.ToList();
            if (existingLoanCodes != null && existingLoanCodes.Count > 0)
            {
                foreach (var item in existingLoanCodes)
                {
                    string loanName = string.Empty;
                    var singleLoan = loanCodes.FirstOrDefault(x => x.LoanCode == item.LoanCode);
                    if (singleLoan != null)
                        loanName = singleLoan.LoanName.Trim() + ": " + singleLoan.LoanCode;
                    customerProducts.Add(new ProductsView
                    {
                        ProductType = "LOANS",
                        ProductCode = item.LoanCode,
                        ProductName = loanName
                    });
                }
            }

            // Add any products where customer has balance and product not in Loan Master
            if (custBalances != null && custBalances.Where(x => x.Ttype.Substring(0, 1).In("A", "L")).ToList().Count != 0)
            {
                foreach (var item in custBalances.Where(x => x.Ttype.Substring(0, 1).In("A", "L")))
                {
                    var singleLoan = customerProducts.FirstOrDefault(x => x.ProductCode == item.Ttype);
                    if (singleLoan != null) continue;

                    customerProducts.Add(new ProductsView
                    {
                        ProductType = "LOANS",
                        ProductCode = item.Ttype,
                        ProductName = item.ProductName.Trim() + ": " + item.Ttype
                    });
                }
            }

            return customerProducts.OrderBy(x => x.ProductType).OrderBy(y => y.ProductCode).ToList();
        }

        public List<AccountDetails> CustomerLoanBalances(string customerNo)
        {
            List<AccountDetails> accountsDetails = new List<AccountDetails>();

            try
            {
                //Get customer products
                List<ProductsView> products = GetCustomerProducts(new List<ProductsView>(),
                    customerNo,
                    new List<CustomerBalances>());


                products = products.Where(x => x.ProductType == "LOANS").ToList();

                // Iterate and pass them as AccountDetails
                DateTime startDate = new DateTime(1990, 1, 1);
                DateTime endDate = DateTime.Now;

                foreach (var product in products)
                {
                    List<CustomerLoanStatementViewModel> statement = GetSingleLoanStatement(
                        new List<CustomerLoanStatementViewModel>(),
                        customerNo,
                        product.ProductCode,
                        startDate, endDate
                        );

                    if (statement != null && statement.Count > 0)
                    {
                        AccountDetails singleAccountDetails = new AccountDetails();
                        singleAccountDetails.AccountBalance = statement.Sum(x => x.Balance);
                        singleAccountDetails.AccountName = statement.FirstOrDefault().ProductName;
                        accountsDetails.Add(singleAccountDetails);
                    }


                }
            }
            catch (Exception ex)
            {
                //log here
            }

            return accountsDetails;
        }

        public List<AccountDetails> CustomerShareBalances(string customerNo)
        {
            List<AccountDetails> accountsDetails = new List<AccountDetails>();

            try
            {
                //Get customer products
                List<ProductsView> products = GetCustomerProducts(new List<ProductsView>(),
                    customerNo,
                    new List<CustomerBalances>());

                products = products.Where(x => x.ProductType == "INVESTMENTS").ToList();

                // Iterate and pass them as AccountDetails
                DateTime? startDate = new DateTime(1990, 1, 1);
                DateTime? endDate = DateTime.Now;

                foreach (var product in products)
                {
                    List<CustomerInvestmentStatementViewModel> statement = GetSingleInvestmentStatement(
                        new List<CustomerInvestmentStatementViewModel>(),
                        customerNo,
                        product.ProductCode,
                        startDate, endDate
                        );

                    if (statement != null && statement.Count > 0)
                    {
                        AccountDetails singleAccountDetails = new AccountDetails();
                        singleAccountDetails.AccountBalance = statement.Sum(x => x.Balance) ?? 0;
                        singleAccountDetails.AccountName = statement.FirstOrDefault().ProductName;
                        accountsDetails.Add(singleAccountDetails);
                    }

                }
            }
            catch (Exception ex)
            {
                //log here
            }

            return accountsDetails;
        }

        private List<CustomerLoanStatementViewModel> GetSingleLoanStatement(List<CustomerLoanStatementViewModel> loanStatement,
                string customerno, string loanCode, DateTime? startDate = null, DateTime? endDate = null,
                string combinedRepayments = "")
        {
            if (loanStatement == null) loanStatement = new List<CustomerLoanStatementViewModel>();
            if (startDate == null) startDate = new DateTime(1900, 01, 01);
            if (endDate == null || endDate.Value.Year == 1900) endDate = DateTime.Now;

            string sqlStatement = " Exec getcustomerloans_Single '" + ValueConverters.format_sql_string(customerno) + "','" +
                ValueConverters.format_sql_string(loanCode) + "','" + ValueConverters.FormatSqlDate(startDate) + "','" + ValueConverters.FormatSqlDate(endDate, true) + "'";

            try
            {
                DbDataReader statement_Reader = DbConnector.GetSqlReader(sqlStatement);
                loanStatement = this.mainDb.Database.SqlQuery<CustomerLoanStatementViewModel>(sqlStatement).ToList();


                int count = 0;
                decimal balance = 0, intbalance = 0, appBalance = 0, penBalance = 0, insBalance = 0;
                foreach (var item in loanStatement)
                {
                    if (combinedRepayments == "1")
                    {
                        item.Debit = item.Debit + item.IntDr + item.PenDr + item.InsDr + item.AppDb;
                        item.Credit = item.Credit + item.IntCr + item.PenCr + item.InsCr + item.AppCr;
                        item.IntDr = item.PenDr = item.InsDr = item.AppDb = item.IntCr = item.PenCr = item.InsCr = item.AppCr = 0;
                    }

                    balance = balance + item.Debit - item.Credit;
                    intbalance = intbalance + item.IntDr - item.IntCr;
                    appBalance = appBalance + item.AppDb - item.AppCr;
                    penBalance = penBalance + item.PenDr - item.PenCr;
                    insBalance = insBalance + item.InsDr - item.InsCr;

                    loanStatement[count].OtherDr = item.AppDb + item.PenDr + item.InsDr;
                    loanStatement[count].OtherCr = item.AppCr + item.PenCr + item.InsCr;
                    loanStatement[count].OtherBal = penBalance + appBalance + insBalance;

                    loanStatement[count].Balance = balance;
                    loanStatement[count].InsBal = insBalance;
                    loanStatement[count].IntBal = intbalance;
                    loanStatement[count].AppBal = appBalance;
                    loanStatement[count].PenBal = penBalance;
                    loanStatement[count].TotBalance = (decimal)ValueConverters.Round05((double)(penBalance + appBalance + intbalance + insBalance + balance));
                    count++;
                }
            }
            catch (Exception ex)
            {

                //log here
            }
            return loanStatement;
        }

        private List<CustomerInvestmentStatementViewModel> GetSingleInvestmentStatement(List<CustomerInvestmentStatementViewModel> investmentStatement,
            string customerno, string investmentCode, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (investmentStatement == null) investmentStatement = new List<CustomerInvestmentStatementViewModel>();
            if (startDate == null) startDate = new DateTime(1900, 01, 01);
            if (endDate == null || endDate.Value.Year == 1900) endDate = DateTime.Now;

            string sqlStatement = " Exec getcustomerInvestments_single '" + ValueConverters.format_sql_string(customerno) + "','" +
                ValueConverters.format_sql_string(investmentCode) + "','" + ValueConverters.FormatSqlDate(startDate) + "','" + ValueConverters.FormatSqlDate(endDate, true) + "'";

            try
            {
                DbDataReader statement_Reader = DbConnector.GetSqlReader(sqlStatement);
                investmentStatement = this.mainDb.Database.SqlQuery<CustomerInvestmentStatementViewModel>(sqlStatement).ToList();

                int count = 0;
                decimal balance = 0;
                foreach (var item in investmentStatement)
                {
                    balance = balance + item.Credit - item.Debit;

                    investmentStatement[count].Balance = balance;
                    investmentStatement[count].ProductName = getProductName(item.ProductCode);
                    count++;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteErrorLog(ref ex);
            }


            return investmentStatement;
        }

        private string getProductName(string productCode)
        {
            string productName = "";

            if (!string.IsNullOrWhiteSpace(productCode))
            {
                var query = mainDb.tbl_InvestmentCodes.Where(x => x.InvestmentCode.Trim() == productCode.Trim()).FirstOrDefault();

                if (query != null)
                {
                    productName = query.InvestmentName;
                }
            }

            return productName;
        }

    }

}