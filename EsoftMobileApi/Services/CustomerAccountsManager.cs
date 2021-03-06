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
using EsoftMobileApi.Services;

namespace ESoft.Web.Services.Registry
{
    public class CustomerAccountsManager
    {
        private readonly Esoft_WebEntities mainDb = new Esoft_WebEntities();
        private String customerNumberMusk = System.Configuration.ConfigurationManager.AppSettings["CustomerNoMask"].ToString();
        private List<CustomerAccountsView> customerAccounts;
        private List<tbl_accounttypes> accounttypes;
        private SavingsProductManager savProductManager;
        private CustomerManager customerManager;

        public CustomerAccountsManager()
        {
            savProductManager = new SavingsProductManager();
            accounttypes = savProductManager.SavingsAccountTypes(mainDb);
            customerAccounts = new List<CustomerAccountsView>();
            customerManager = new CustomerManager();
        }

        public List<tbl_CustomerAccounts> GetCustomerSavingsAccounts(String customerNo)
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
                        ProductCode = (item.InvestmentCode ?? string.Empty).Trim(),
                        AccountNo = (item.InvestmentCode ?? string.Empty).Trim(),
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
                        ProductCode = (item.LoanCode ?? string.Empty).Trim(),
                        ProductName = (loanName ?? string.Empty).Trim(),
                        AccountNo = (item.LoanCode ?? string.Empty).Trim()
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
                        ProductCode = (item.Ttype ?? string.Empty).Trim(),
                        ProductName = item.ProductName.Trim() + ": " + (item.Ttype ?? string.Empty).Trim(),
                        AccountNo = (item.AccountNo ?? string.Empty).Trim()
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

        public List<CustomerLoanStatementViewModel> GetSingleLoanStatement(List<CustomerLoanStatementViewModel> loanStatement,
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

        public List<CustomerInvestmentStatementViewModel> GetSingleInvestmentStatement(List<CustomerInvestmentStatementViewModel> investmentStatement,
            string customerno, string investmentCode, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (investmentStatement == null) investmentStatement = new List<CustomerInvestmentStatementViewModel>();
            if (startDate == null) startDate = new DateTime(1900, 01, 01);
            if (endDate == null || endDate.Value.Year == 1900) endDate = DateTime.Now;

            string sqlStatement = " Exec getcustomerInvestments_single '" + customerno.Format_Sql_String() + "','" +
                investmentCode.Format_Sql_String() + "','" + ValueConverters.FormatSqlDate(startDate) + "','" + ValueConverters.FormatSqlDate(endDate, true) + "'";

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


        public List<tbl_accounttypes> GetAccountTypes()
        {
            List<tbl_accounttypes> accountTypes = new List<tbl_accounttypes>();

            try
            {
                accountTypes = mainDb.tbl_accounttypes.ToList();
            }
            catch (Exception ex)
            {
                //log error;
            }

            return accountTypes;
        }

        public List<LinkedAtmCards> GetLinkedAtmCards(string customerNo, List<LinkedAtmCards> linkedAtms)
        {
            linkedAtms = linkedAtms == null ? new List<LinkedAtmCards>() : linkedAtms;

            try
            {
                List<LinkedAtmCards> cards = (from atmCards in mainDb.tbl_LinkedAtmCards
                                              join custAccounts in mainDb.tbl_CustomerAccounts on
                                              atmCards.AccountNo equals custAccounts.AccountNo
                                              where custAccounts.CustomerNo == customerNo
                                              join branches in mainDb.BranchSettings on atmCards.Branch equals branches.BranchCode
                                              select new LinkedAtmCards
                                              {
                                                  recno = atmCards.recno,
                                                  AccountNo = atmCards.AccountNo,
                                                  CardNumber = atmCards.CardNumber,
                                                  Branch = atmCards.Branch,
                                                  BranchName = branches.Name,
                                                  Enabled = atmCards.Enabled,
                                                  DateLinked = atmCards.DateLinked,
                                                  LinkedBy = atmCards.LinkedBy,
                                                  verify = atmCards.verify,
                                                  AttachBy = atmCards.AttachBy,
                                                  verifyBy = atmCards.verifyBy,
                                                  tbl_LinkedAtmCardsID = atmCards.tbl_LinkedAtmCardsID
                                              }).ToList();



                linkedAtms.AddRange(cards);
            }
            catch (Exception ex)
            {
                //Utility.WriteErrorLog("GetLinkedAtmCards", ref ex);
            }

            for (int i = 0; i < linkedAtms.Count(); i++)
            {
                linkedAtms[i].CardNumber = ValueConverters.FormatAtmCardNumber(linkedAtms[i].CardNumber);
            }

            return linkedAtms;
        }


        public tbl_Customer CustomerDetails(Guid id)
        {
            tbl_Customer customer = new tbl_Customer();

            if (!string.IsNullOrWhiteSpace(id.ToString()))
            {
                customer = mainDb.tbl_Customer
                    .Where(x => x.tbl_CustomerID == id)
                    .Select(x => x)
                    .FirstOrDefault();
            }
            return customer;
        }

        public MobileAppCustomer MobileAppCustomerDetails(Guid id)
        {
            MobileAppCustomer customer = new MobileAppCustomer();
            //TODO REPLACE * WITH COLUMNS

            string query = "select * from tbl_Customer c " +
                            " left join tbl_users u on ltrim(rtrim(c.CustomerNo)) = ltrim(rtrim(u.SaccoMembershipNumber)) " +
                            " left join tbl_TellerAccounts ta on ta.LoginCode = u.LoginCode " +
                            " where c.tbl_CustomerID = '" + id.ToString() + "'";

            if (!string.IsNullOrWhiteSpace(id.ToString()))
            {
                customer = mainDb.Database.SqlQuery<MobileAppCustomer>(query).FirstOrDefault();

                if (customer != null)
                {
                    if (!string.IsNullOrWhiteSpace(customer.LoginCode) &&
                        !string.IsNullOrWhiteSpace(customer.TellerAccountNo))
                    {
                        customer.IsTeller = true;
                    }
                }
            }

            return customer;
        }

        public tbl_Customer CustomerDetails(string customerNo)
        {
            tbl_Customer customer = new tbl_Customer();

            if (!string.IsNullOrWhiteSpace(customerNo.ToString()))
            {
                customerNo = ValueConverters.PADLeft(Int32.Parse(this.customerNumberMusk), customerNo, '0');

                customer = mainDb.tbl_Customer
                    .Where(x => x.CustomerNo.Trim() == customerNo.Trim())
                    .Select(x => x)
                    .FirstOrDefault();
            }

            return customer;
        }

        public bool RegisterMobileUser(MobileUsers user)
        {
            bool insertResult = false;

            try
            {
                string insertMobileUser = "INSERT INTO tbl_MobileUsers(CustomerName,MobileNo,Pin,tbl_CustomerId,CustomerNo,Email,IdNo,DateCreated,Enabled) " +
                     " VALUES('" + user.CustomerName.Format_Sql_String() + "','" +
                     user.MobileNo.Format_Sql_String() + "','" +
                     user.Pin + "','" +
                     user.tbl_CustomerId + "','" +
                     user.CustomerNo.Format_Sql_String() + "','" +
                     user.Email.Format_Sql_String() + "','" +
                     user.IdNo.Format_Sql_String() + "','" +
                     ValueConverters.FormatSqlDate(DateTime.Now)+ "','" +
                     ValueConverters.ConvertNullToBool(user.Enabled) + "'); ";

                int result = mainDb.Database.ExecuteSqlCommand(insertMobileUser);

                if (result >= 1)
                {
                    insertResult = true;
                }

            }
            catch (Exception ex)
            {
                //log ex
            }

            return insertResult;
        }

        public LoginInfo Login(Login login)
        {
            LoginInfo loginInfo = new LoginInfo();

            MobileUsers user = FindMobileAccount(login.MobileNo);

            //No user was found
            if (user == null)
            {
                loginInfo.Message = "Account not found";
                loginInfo.Status = "Failed";
                loginInfo.User = null;
            }
            else
            {
                if (user.tbl_CustomerId == Guid.Empty || user.Enabled == false)
                {
                    //account pending activation
                    loginInfo.Message = "Account pending activation";
                    loginInfo.Status = "Failed";
                    loginInfo.User = null;
                }
                else if (user.Pin.ToString().Trim() != login.Pin.ToString().Trim())
                {
                    //password not matching
                    loginInfo.Message = "Invalid account details";
                    loginInfo.Status = "Failed";
                    loginInfo.User = null;
                }
                else
                {
                    loginInfo.Message = "Welcome";
                    loginInfo.Status = "Success";
                    loginInfo.User = user;
                }

            }

            return loginInfo;
        }

        private MobileUsers FindMobileAccount(string mobileNo)
        {
            MobileUsers user = new MobileUsers();

            try
            {
                String query = @"SELECT CustomerName,MobileNo,Pin,IdNo,tbl_CustomerId,
                                CustomerNo,Email,Enabled,DateCreated 
                                FROM tbl_MobileUsers WHERE MobileNo = " + mobileNo.Format_Sql_String() + " ;";

                user = this.mainDb.Database.SqlQuery<MobileUsers>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //LOG ex
            }

            return user;
        }

        public bool ActivateMobileUser(string idNo, string customerNo, string pin)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(idNo) && !string.IsNullOrWhiteSpace(customerNo))
            {
                tbl_Customer customer = mainDb.tbl_Customer
                            .Where(x => x.CustomerNo.Trim() == customerNo.Trim()
                                  && x.CustomerIdNo.Trim() == idNo.Trim())
                            .FirstOrDefault();

                if (customer != null)
                {
                    result = ActivateUser(customer.tbl_CustomerID, customer.CustomerNo, pin);
                }
            }

            return result;
        }

        private bool ActivateUser(Guid tbl_customer_id, string userMemberNo, string pinNumber)
        {
            bool activateStatus = false;

            string updateLinkProcess = "UPDATE tbl_MobileUsers SET Pin='" +
                pinNumber.Format_Sql_String() + "',tbl_CustomerId='" +
                tbl_customer_id + "',Enabled='true' WHERE CustomerNo = '" + userMemberNo.Format_Sql_String() + "';";

            try
            {
                int success = this.mainDb.Database.ExecuteSqlCommand(updateLinkProcess);

                if (success >= 1)
                {
                    activateStatus = true;
                }
            }
            catch (Exception ex)
            {
                //log the exception
                activateStatus = false;
            }

            return activateStatus;
        }

        public CustomerAccountsView GetAccountByAccountNumber(string accountNumber, bool closureDetails = false)
        {
            customerAccounts = new List<CustomerAccountsView>();


            var account = mainDb.tbl_CustomerAccounts.Where(x => x.AccountNo == accountNumber).ToList();
            if (account != null && account.Count != 0)
            {
                GetAccountDetails(account.FirstOrDefault().CustomerNo, account, false);
            }


            var customerAccountsDetails = (from accounts in account
                                           select new CustomerAccountsView
                                           {
                                               AccountNo = accounts.AccountNo,
                                               OpenedBy = accounts.OpenedBy,
                                               AccountComments = accounts.AccountComments,
                                               AccountRemarks = accounts.AccountRemarks,
                                               AccountType = accounts.AccountType,
                                               Locked = accounts.Locked,
                                               OpenedDate = accounts.OpenedDate,
                                               AuthorisedBy = accounts.AuthorisedBy,
                                               AuthorisedDate = accounts.AuthorisedDate,
                                               DateClosed = accounts.DateClosed,
                                               ClosedBy = accounts.ClosedBy,
                                               CustomerNo = accounts.CustomerNo,
                                               ReasonClosed = string.Empty
                                           }).FirstOrDefault();

            if (customerAccountsDetails != null && customerAccountsDetails.AuthorisedBy != null)
            {
                var opennedBy = mainDb.tbl_users.FirstOrDefault(x => x.LoginCode == customerAccountsDetails.AuthorisedBy);
                if (opennedBy != null)
                {
                    customerAccountsDetails.AuthorisedBy = ValueConverters.ConvertNullToEmptyString(opennedBy.LoginName);
                }
            }

            if (customerAccountsDetails != null && customerAccountsDetails.OpenedBy != null)
            {
                var opennedBy = mainDb.tbl_users.FirstOrDefault(x => x.LoginCode == customerAccountsDetails.OpenedBy);
                if (opennedBy != null)
                {
                    customerAccountsDetails.OpenedBy = ValueConverters.ConvertNullToEmptyString(opennedBy.LoginName);
                }
            }
            if (customerAccountsDetails != null)
            {
                var accountTypeDetails = accounttypes.FirstOrDefault(x => x.code == customerAccountsDetails.AccountType);
                if (accountTypeDetails == null) accountTypeDetails = new tbl_accounttypes();
                customerAccountsDetails.AccountTypeSettings = accountTypeDetails;
                customerAccountsDetails.GlMemSav = accountTypeDetails.glmemsav;
                customerAccountsDetails.MinimumBalance = accountTypeDetails.minimum_bal;

                if (closureDetails)
                {
                    GetAccountClosureHistory(customerAccountsDetails);
                    var lastTr = customerAccountsDetails.ClosedAccountDetails.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    if (lastTr != null)
                    {
                        customerAccountsDetails.ReasonClosed = ValueConverters.ConvertNullToEmptyString(lastTr.PostingDescription);
                    }
                }
            }

            customerAccountsDetails = customerAccountsDetails == null ? new CustomerAccountsView() : customerAccountsDetails;
            return customerAccountsDetails;

            //  return customerAccounts.FirstOrDefault();
        }

        private void GetAccountClosureHistory(CustomerAccountsView customerAccountsDetails)
        {
            customerAccountsDetails.ClosedAccountDetails = new List<PostingJournalsHeaderViewModel>();
            var closeDetails = mainDb.tbl_PostingJournals
                .Where(x => (x.PostingDocid == "ACRS" || x.PostingDocid == "ACLS")
                && x.AccountNo == customerAccountsDetails.AccountNo)
                .ToList();


            if (closeDetails != null)
            {
                foreach (var item in closeDetails)
                {
                    PostingJournalsHeaderViewModel lineItem = new PostingJournalsHeaderViewModel();

                    lineItem.PostingDescription = ValueConverters.ConvertNullToEmptyString(item.PostingDescription);
                    lineItem.PostDate = ValueConverters.ConvertNullToDatetime(item.PostDate);
                    lineItem.PostedBy = ValueConverters.ConvertNullToEmptyString(item.PostedBy);
                    lineItem.CreateDate = ValueConverters.ConvertNullToDatetime(item.CreateDate);
                    lineItem.CreatedBy = ValueConverters.ConvertNullToEmptyString(item.CreatedBy);
                    lineItem.PaymentMethodName = item.PostingDocid == "ACLS" ? "Closure" : "Rejoin";

                    var opennedBy = mainDb.tbl_users.FirstOrDefault(x => x.LoginCode == lineItem.PostedBy);
                    if (opennedBy != null)
                    {
                        lineItem.PostedByName = ValueConverters.ConvertNullToEmptyString(opennedBy.LoginName);
                    }
                    opennedBy = mainDb.tbl_users.FirstOrDefault(x => x.LoginCode == lineItem.CreatedBy);
                    if (opennedBy != null)
                    {
                        lineItem.CreatedByName = ValueConverters.ConvertNullToEmptyString(opennedBy.LoginName);
                    }
                    customerAccountsDetails.ClosedAccountDetails.Add(lineItem);
                }
            }
        }

        private void GetAccountDetails(string customerNo, List<tbl_CustomerAccounts> accounts, bool telleringOperations)
        {
            if (accounts.Count != 0 && accounts != null)
            {
                if (customerAccounts == null) customerAccounts = new List<CustomerAccountsView>();

                var customerDetails = customerManager.GetCustomerDetails(customerNo).FirstOrDefault();
                if (customerDetails == null)
                {
                    customerDetails = new CustomerDetailsView();
                }

                foreach (var account in accounts)
                {
                    var accountTypeDetails = accounttypes.FirstOrDefault(x => x.code == account.AccountType);//.ToList();
                    if (accountTypeDetails == null) accountTypeDetails = new tbl_accounttypes();

                    if (telleringOperations && ValueConverters.ConvertNullToBool(accountTypeDetails.Disallow_Teller_Transactions))
                    {
                        continue;
                    }
                    customerAccounts.Add(new CustomerAccountsView
                    {
                        tbl_CustomerAccountsID = account.tbl_CustomerAccountsID,
                        CustomerNo = customerNo,
                        AccountNo = account.AccountNo,
                        AccountComments = account.AccountComments,
                        AccountRemarks = account.AccountRemarks,
                        AccountType = account.AccountType,
                        AccountTypeName = accountTypeDetails.category,
                        GlMemSav = accountTypeDetails.glmemsav,
                        MinimumBalance = accountTypeDetails.minimum_bal,
                        OpenedBy = account.OpenedBy,
                        OpenedDate = account.OpenedDate,
                        AuthorisedBy = account.AuthorisedBy,
                        AuthorisedDate = account.AuthorisedDate,
                        DateClosed = account.DateClosed,
                        Locked = account.Locked,
                        CustomerBranch = customerDetails.Branch,
                        customerDetails =
                        { new CustomerDetailsView{
                            CustomerName=customerDetails.CustomerName,
                            AccountComments= customerDetails.AccountComments,
                            AccountRemarks = customerDetails.AccountRemarks,
                            Branch =  customerDetails.Branch,
                            Locked =  customerDetails.Locked,
                            CustomerIdNo =  customerDetails.CustomerIdNo,
                            DateClosed =  customerDetails.DateClosed,
                            MemberType =  customerDetails.MemberType,
                            MobileNo   =  customerDetails.MobileNo,
                            CustomerNo  = customerNo
                        }},
                        AccountTypeSettings = accountTypeDetails
                    });
                }
            }
        }
    }

}
