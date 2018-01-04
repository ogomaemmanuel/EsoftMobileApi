using ESoft.Web.Services.Common;
using ESoft.Web.Services.Registry;
using EsoftMobileApi.Models;
using EsoftMobileApi.Services.common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class TellerManager
    {
        private IValidationDictionary _validationDictionary;
        private PostTransactions transactionsEngine = new PostTransactions();
        private CustomerManager customerManager = new CustomerManager();
        private UserAdministrationManager userManager = new UserAdministrationManager();
        private GlAccountsManager glAccountMgr = new GlAccountsManager();
        private CompanyManager companyManager = new CompanyManager();
        private LoanProductsManager loanProductsMgr = new LoanProductsManager();
        private InvestmentsCodesManager investMgr = new InvestmentsCodesManager();
        private CustomerAccountsManager customerAccountsManager = new CustomerAccountsManager();
        private Esoft_WebEntities db;


        public TellerManager()
        {
            db = new Esoft_WebEntities();
        }

        public TellerManager(IValidationDictionary validationDictionary)
            : this()
        {
            _validationDictionary = validationDictionary;
        }

        // Post Teller Product Repayments
        public bool PostProductRepayment(TellerProductRepaymentsView custDetails, List<RepaymentView> repayments, string tellerLoginCode, string deviceInfo)
        {
            bool transactionPosted = false;

            string tellerAccount = GetTellerGlAccount(tellerLoginCode);

            double transactionAmount = repayments.Where(x => x.CustomerNo == custDetails.CustomerNo).Sum(x => x.Amount);

            Validate_Teller_Transaction("PRODUCTREPAYMENTS", tellerAccount, transactionAmount);

            if (repayments == null || repayments.Count == 0 || repayments.Sum(x => x.Amount) == 0)
            {
                _validationDictionary.AddError("customerDetails.CustomerNo", " Nothing To Post ");
            }

            string m_transactionid = ValueConverters.RandomString(10);
            DateTime trdatenow = DateTime.Now;

            string trdescpt = "Product Repayments Cash Deposit From Mobile App";
            string glaccount_db = tellerAccount;
            string glaccount_cr = string.Empty;
            string docid = "MAPP";
            string referenceNo = transactionsEngine.Generate_PostReference(docid);
            bool post_comm_to_customerBranch = true;// transactionsEngine.Get_Other_Settings("POST_TELLER_LOAN_INCOME_TO_CUSTOMER_BRANCH");
            string defaultBranch = "99";
            List<PostTransactionsViewModel> translist = new List<PostTransactionsViewModel>();

            InvestmentsCodesManager investMgr = new InvestmentsCodesManager();
            LoanProductsManager loanProductsMgr = new LoanProductsManager();


            Esoft_WebEntities db = new Esoft_WebEntities();

            List<CustomerBalances> customerBalances = new List<CustomerBalances>();
            customerBalances = customerManager.GetCustomerBalances(custDetails.CustomerNo, DateTime.Now, customerBalances);

            string income_branch = defaultBranch;
            if (post_comm_to_customerBranch)
            {
                income_branch = db.tbl_Customer.FirstOrDefault(x => x.CustomerNo == custDetails.CustomerNo).Branch;
            }
            var investmentCodes = investMgr.InvestmentsCodes(db).ToList();
            var loanCodes = loanProductsMgr.GetLoanCodes(db).ToList();
            foreach (var repayment in repayments.Where(x => x.CustomerNo == custDetails.CustomerNo))
            {
                switch (repayment.Ledger)
                {
                    case "SAVINGS":
                        trdescpt = String.Format("Cash Deposits Mobile App: {0}", referenceNo);

                        CustomerAccountsView customerDetails = customerAccountsManager.GetAccountByAccountNumber(repayment.ProductCode);

                        glaccount_cr = customerDetails.GlMemSav;

                        transactionsEngine
                            .Generate_Ledger_Transactions(translist, m_transactionid, repayment.ProductCode, trdatenow, trdescpt,
                                                          docid, referenceNo, repayment.Amount, 0, income_branch,
                                                          glaccount_db, glaccount_cr, tellerAccount, tellerLoginCode);

                        transactionsEngine
                            .Generate_Ledger_Transactions(translist, m_transactionid, repayment.ProductCode, trdatenow, trdescpt, docid, referenceNo, 0,
                       repayment.Amount, income_branch, glaccount_cr, glaccount_db, tellerAccount, tellerLoginCode);

                        transactionsEngine.Generate_Savings_Transactions(translist, m_transactionid, repayment.ProductCode, trdatenow, trdescpt, docid, referenceNo,
                       repayment.Amount, 0, income_branch, glaccount_cr, glaccount_db, repayment.CustomerNo, glaccount_cr, string.Empty, tellerLoginCode);

                        LogMobileTrail(new MobileOperatorTrail()
                        {
                            ReferenceNo = referenceNo,
                            Ledger = "S",
                            CustomerNo = custDetails.CustomerNo,
                            AccountNo = repayment.ProductCode,
                            TransactionDate = trdatenow,
                            Description = trdescpt,
                            Amount = ValueConverters.ConvertDoubleToDecimal(repayment.Amount),
                            DeviceInfo = deviceInfo ?? String.Empty,
                            LoginCode = tellerLoginCode,
                        });

                        break;
                    case "INVESTMENTS":
                        trdescpt = String.Format("Cash Deposits Mobile App: {0}", referenceNo);

                        glaccount_cr = investmentCodes.FirstOrDefault(x => x.InvestmentCode == repayment.ProductCode).PrincipalAccount.ToString();

                        transactionsEngine.Generate_Shares_Transactions(translist, m_transactionid, repayment.CustomerNo, trdatenow, trdescpt, docid, referenceNo, 0, repayment.Amount,
                            income_branch, glaccount_db, glaccount_cr, repayment.ProductCode, tellerLoginCode);

                        transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, repayment.CustomerNo, trdatenow, trdescpt, docid, referenceNo, 0,
                         repayment.Amount, income_branch, glaccount_cr, tellerAccount, repayment.CustomerNo, tellerLoginCode);

                        LogMobileTrail(new MobileOperatorTrail()
                        {
                            ReferenceNo = referenceNo,
                            Ledger = "I",
                            CustomerNo = custDetails.CustomerNo,
                            AccountNo = repayment.ProductCode,
                            TransactionDate = trdatenow,
                            Description = trdescpt,
                            Amount = ValueConverters.ConvertDoubleToDecimal(repayment.Amount),
                            DeviceInfo = deviceInfo ?? String.Empty,
                            LoginCode = tellerLoginCode,
                        });

                        break;
                    case "LOANS":
                        trdescpt = String.Format("Loan Repayment Mobile App: {0}", referenceNo);
                        var loanProduct = loanCodes.FirstOrDefault(x => x.LoanCode == repayment.ProductCode);
                        loanProductsMgr.Distribute_LoanRepayment(repayment, customerBalances, loanProduct, true);
                        loanProductsMgr.GenerateLoanRepaymentStatements(transactionsEngine, translist, m_transactionid, repayment, loanProduct, trdatenow, trdescpt, docid, referenceNo, 0, 0,
                            income_branch, tellerAccount, "1", "", false, db);

                        LogMobileTrail(new MobileOperatorTrail()
                        {
                            ReferenceNo = referenceNo,
                            Ledger = "L",
                            CustomerNo = custDetails.CustomerNo,
                            AccountNo = repayment.ProductCode,
                            TransactionDate = trdatenow,
                            Description = trdescpt,
                            Amount = ValueConverters.ConvertDoubleToDecimal(repayment.Amount),
                            DeviceInfo = deviceInfo ?? String.Empty,
                            LoginCode = tellerLoginCode,
                        });

                        break;
                    default:
                        break;
                }
            }

            // final Debit to Teller Account
            transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, custDetails.CustomerNo, trdatenow, trdescpt, docid, referenceNo, transactionAmount,
                       0, income_branch, tellerAccount, "LOAN-REP", custDetails.CustomerNo, tellerLoginCode);

            string result = transactionsEngine.Post_Transactions(translist, m_transactionid, false, false);

            if (result == "Transactions Was Updated Successfully ")
            {
                transactionPosted = true;
            }

            return transactionPosted;
        }

        public bool LogMobileTrail(MobileOperatorTrail trail)
        {
            bool insertResult = false;

            try
            {
                string insertMobileUser = "INSERT INTO Tbl_MobileOperatorTrail(ReferenceNo,Ledger,CustomerNo,AccountNo,TransactionDate,Description,Amount,DeviceInfo,LoginCode,OperatorTrailID) " +
                     " VALUES('" + trail.ReferenceNo.Format_Sql_String() + "','" +
                        trail.Ledger.Format_Sql_String() + "','" +
                        trail.CustomerNo.Format_Sql_String() + "','" +
                        trail.AccountNo.Format_Sql_String() + "','" +
                        trail.TransactionDate.ConvertNullToDatetime() + "','" +
                        trail.Description.Format_Sql_String() + "','" +
                        ValueConverters.ConvertNullToDecimal(trail.Amount) + "','" +
                        trail.DeviceInfo.Format_Sql_String() + "','" +
                        trail.LoginCode.Format_Sql_String() + "','" +
                        Guid.NewGuid().ToString() + "'); ";

                int result = db.Database.ExecuteSqlCommand(insertMobileUser);

                if (result >= 1)
                {
                    insertResult = true;
                }

            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ref ex);
            }

            return insertResult;
        }

        public string GetTellerGlAccount(string tellerLoginCode)
        {
            List<TellerAccountView> tellerAccountDetails = userManager.TellerAccountDetails(tellerLoginCode);

            if (tellerAccountDetails.Count == 1)
            {
                return tellerAccountDetails.FirstOrDefault().TellerAccountNo;
            }
            else
            {
                return string.Empty;
            }
        }

        public void Validate_Teller_Transaction(string transactionType, string tellerAccount, double transactionAmount, bool interTellerTransaction = false)
        {
            if (ValueConverters.IsStringEmpty(tellerAccount))
            {
                _validationDictionary.AddError("TransactionAmount", "You do not have a Valid Teller Account  !");
                _validationDictionary.AddError("PaidTo", "You do not have a Valid Teller Account  !");
                _validationDictionary.AddError("CustomerNo", "You do not have a Valid Teller Account  !");
                return;
            }
            if (transactionAmount <= 0.00)
            {
                _validationDictionary.AddError("TransactionAmount", "Transaction Amount is Zero/Negative");
                _validationDictionary.AddError("GlCredit", "Transaction Amount is Zero/Negative");
                _validationDictionary.AddError("CustomerNo", "Transaction Amount is Zero/Negative");
                _validationDictionary.AddError("PaidTo", "Transaction Amount is Zero/Negative");

            }

            double tellerBalance = TellerAccountBalance(tellerAccount);

            Company companyDetails = companyManager.GetCompanyDetails();
            if (interTellerTransaction || transactionType.ToUpper().In("MISCPAYMENTS", "AGENCYWITHDRAWAL", "CASHWITHDRAWAL"))
            {
                if (tellerBalance - transactionAmount < 0)
                {
                    _validationDictionary.AddError("TransactionAmount", "You Have Insufficient Funds for this Transaction !");
                    _validationDictionary.AddError("CustomerNo", "You Have Insufficient Funds for this Transaction !");
                    _validationDictionary.AddError("TrType", "You Have Insufficient Funds for this Transaction !");
                    _validationDictionary.AddError("PaidTo", "You Have Insufficient Funds for this Transaction !");
                }
            }
            else
            {
                if (!transactionType.ToUpper().In("CHEQUEDEPOSIT"))
                {
                    if ((decimal)tellerBalance > companyDetails.TellerCashInsuranceLimit)
                    {
                        if (transactionType == "PRODUCTREPAYMENTS")
                        {
                            _validationDictionary.AddError("customerDetails.CustomerNo", "Insurance Limit Has been Exceeded !");
                        }
                        else
                        {
                            _validationDictionary.AddError("TransactionAmount", "Insurance Limit Has been Exceeded !");
                            _validationDictionary.AddError("CustomerNo", "Insurance Limit Has been Exceeded !");
                            _validationDictionary.AddError("PaidTo", "You Have Insufficient Funds for this Transaction !");
                        }
                    }
                }
            }

            TellerDayClosed(tellerAccount);
        }

        public string TellerDayClosed(string tellerGlAccountNo)
        {
            string result = string.Empty;
            if (!CustomValidation.ValidateGlAccount_(tellerGlAccountNo))
            {
                result = "You Do Not Have a Valid Teller Account ";
                _validationDictionary.AddError("GLDEBIT", result);
                _validationDictionary.AddError("PaidTo", result);
            }
            else
            {
                try
                {
                    string query = "SELECT top 1 TransactionDate from tbl_Teller_Closing where TellerAccount='" +
                        tellerGlAccountNo + "' and TransactionDate>='" + ValueConverters.FormatSqlDate(DateTime.Now) + "'";

                    DbDataReader reader = DbConnector.GetSqlReader(query);

                    while (reader.Read())
                    {
                        string closedDate = reader["TransactionDate"].ToString();
                        if (ValueConverters.IsStringEmpty(closedDate) == false)
                        {
                            result = "Closing Already Done for This Teller ";
                            _validationDictionary.AddError("GLDEBIT", result);
                            _validationDictionary.AddError("PaidTo", result);
                            _validationDictionary.AddError("TransactionAmount", result);
                            _validationDictionary.AddError("CustomerNo", result);


                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Utility.WriteErrorLog(ref ex);
                    result = "Could not Check Account Status";
                    _validationDictionary.AddError("GLDEBIT", result);
                    _validationDictionary.AddError("PaidTo", result);
                }
            }
            return result;
        }

        public double TellerAccountBalance(string tellerAccount)
        {
            return glAccountMgr.GetGlBalance(tellerAccount);
        }

        public List<RepaymentView> ReadRepayment(TellerMobileDeposit tellerDeposit)
        {
            List<RepaymentView> repayments = new List<RepaymentView>();

            var loanproductNames = loanProductsMgr.GetLoanCodes(db, false).ToList();
            var investnames = investMgr.InvestmentsCodes(db).ToList();
            String ledger = tellerDeposit.ProductCode;

            if (ledger.Substring(0, 1) == "0")
            {
                ledger = "SAVINGS";
            }
            else
            {
                ledger = ledger.Substring(0, 1).In("A", "L") ? "LOANS" : "INVESTMENTS";
            }

            string productName = tellerDeposit.ProductCode;

            switch (ledger)
            {
                case "LOANS":
                    if (loanproductNames.FirstOrDefault(x => x.LoanCode == tellerDeposit.ProductCode) != null)
                    {
                        productName = ValueConverters.ConvertNullToEmptyString(
                            loanproductNames.FirstOrDefault(x => x.LoanCode == tellerDeposit.ProductCode).LoanName.ToString());
                    }
                    break;
                case "INVESTMENTS":
                    if (investnames.FirstOrDefault(x => x.InvestmentCode == tellerDeposit.ProductCode) != null)
                    {
                        productName = ValueConverters.ConvertNullToEmptyString(
                            investnames.FirstOrDefault(x => x.InvestmentCode == tellerDeposit.ProductCode).InvestmentName.ToString());
                    }
                    break;
                default:
                    break;
            }

            repayments.Add(new RepaymentView
            {
                CustomerNo = tellerDeposit.CustomerNo,
                ProductCode = tellerDeposit.ProductCode,
                ProductName = productName,
                Amount = ValueConverters.ConvertDecimaltoDouble(tellerDeposit.TrxAmount),
                Ledger = ledger,
                Section = 1
            });

            return repayments;
        }
    }
}