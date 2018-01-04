using ESoft.Web.Services.Common;
using EsoftMobileApi.Models;
using EsoftMobileApi.Services.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class LoanProductsManager
    {
        public IEnumerable<tbl_LoanCodes> GetLoanCodes(Esoft_WebEntities db, bool combineName = true)
        {
            var returnList = new List<tbl_LoanCodes>();
            var data = db.tbl_LoanCodes.OrderBy(p => p.LoanCode).ToList();
            foreach (var item in data)
            {
                if (combineName)
                {
                    item.LoanName = item.LoanCode.Trim() + ": " + item.LoanName.Trim();
                }
                returnList.Add(item);
            }
            return returnList.ToList();
        }

        public RepaymentView Distribute_LoanRepayment(RepaymentView repview, List<CustomerBalances> custBalances, tbl_LoanCodes loanproduct, bool chargeCashPayLevy)
        {
            // Used by TellerManager.PostProductRepayment
            var product = custBalances.FirstOrDefault(x => x.Ttype == repview.ProductCode);
            if (product != null)
            {
                double total_amount = repview.Amount;
                double amount;
                if (chargeCashPayLevy)
                {
                    //amount = (double)Math.Max(ValueConverters.ConvertNullToDecimal(loanproduct.CashPayment_Charge),
                    //    (decimal)ValueConverters.Round05(ValueConverters.ConvertNullToDecimal(loanproduct.CashPayment_Charge_Rate) * (decimal)repview.Amount) / 100);
                    //amount = Math.Max(amount, 0);

                    //amount = Math.Min(amount, total_amount);
                    //total_amount = total_amount - amount;
                    //repview.loan_Levy = amount;
                }
                if (product.IntBalance > 0)
                {
                    amount = Math.Min(product.IntBalance, total_amount);
                    total_amount = total_amount - amount;
                    repview.LoanInt = amount;
                }
                if (product.InsBalance > 0)
                {
                    amount = Math.Min(product.InsBalance, total_amount);
                    total_amount = total_amount - amount;
                    repview.LoanIns = amount;
                }
                if (product.AppBalance > 0)
                {
                    amount = Math.Min(product.AppBalance, total_amount);
                    total_amount = total_amount - amount;
                    repview.LoanApp = amount;
                }
                if (product.PenBalance > 0)
                {
                    amount = Math.Min(product.PenBalance, total_amount);
                    total_amount = total_amount - amount;
                    repview.LoanPen = amount;
                }
                repview.LoanPrincipal = total_amount; // remainder take to principal
            }
            return repview;
        }

        public List<PostTransactionsViewModel> GenerateLoanRepaymentStatements(PostTransactions transactionsEngine, List<PostTransactionsViewModel> translist,
                string m_transactionid, RepaymentView repview, tbl_LoanCodes loanproduct, DateTime m_TransactionDate, string m_Narration,
                string m_Docid, string m_ReferenceNo, double m_DebitAmount,
                double m_CreditAmount, string m_BranchCode, string m_GlDebit, string transaction_groupid, string m_LoanReferenceNo,
                bool raiseDebit, Esoft_WebEntities maindb = null, bool checkOffPosting = false)
        {
            string income_Account = string.Empty, accrued_Account = string.Empty;
            if (repview.LoanPrincipal > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo,
                      0, repview.LoanPrincipal, m_BranchCode, m_GlDebit, loanproduct.PrincipalAccount, repview.ProductCode, 1, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
                        repview.LoanPrincipal, m_BranchCode, loanproduct.PrincipalAccount, m_GlDebit, repview.CustomerNo, String.Empty);
                if (checkOffPosting)
                {
                    transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.LoanPrincipal, 0,
                         m_BranchCode, m_GlDebit, loanproduct.PrincipalAccount, repview.CustomerNo, String.Empty);
                }
            }
            if (repview.LoanInt > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo,
                    0, repview.LoanInt, m_BranchCode, m_GlDebit, loanproduct.InterestControlAccount, repview.ProductCode, 2, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
                        repview.LoanInt, m_BranchCode, loanproduct.InterestControlAccount, m_GlDebit, repview.CustomerNo, String.Empty);

                if (checkOffPosting)
                {
                    transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.LoanInt,
                        0, m_BranchCode, m_GlDebit, loanproduct.InterestControlAccount, repview.CustomerNo, String.Empty);
                }
                income_Account = loanproduct.InterestIncomeAccount;
                accrued_Account = loanproduct.InterestAccruedAccount;

                RaiseIncomeContraEntries(transactionsEngine, translist, m_TransactionDate, m_transactionid, repview.CustomerNo, m_Narration, m_Docid, m_ReferenceNo,
                    repview.LoanInt, accrued_Account, income_Account, m_BranchCode, repview.ProductCode, checkOffPosting);
            }
            if (repview.LoanIns > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo,
                        0, repview.LoanIns, m_BranchCode, m_GlDebit, loanproduct.InsuranceControlAccount, repview.ProductCode, 3, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
                    repview.LoanIns, m_BranchCode, loanproduct.InsuranceControlAccount, m_GlDebit, repview.CustomerNo, String.Empty);
                if (checkOffPosting)
                {
                    transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.LoanIns,
                        0, m_BranchCode, m_GlDebit, loanproduct.InsuranceControlAccount, repview.CustomerNo, String.Empty);
                }
                income_Account = loanproduct.InsuranceIncomeAccount;
                accrued_Account = loanproduct.InsuranceAccruedAccount;
                RaiseIncomeContraEntries(transactionsEngine, translist, m_TransactionDate, m_transactionid, repview.CustomerNo, m_Narration, m_Docid, m_ReferenceNo,
                    repview.LoanIns, accrued_Account, income_Account, m_BranchCode, repview.ProductCode, checkOffPosting);
            }
            if (repview.LoanApp > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo,
                        0, repview.LoanApp, m_BranchCode, m_GlDebit, loanproduct.AppraisalFeeControlAccount, repview.ProductCode, 4, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
                         repview.LoanApp, m_BranchCode, loanproduct.AppraisalFeeControlAccount, m_GlDebit, repview.CustomerNo, String.Empty);
                if (checkOffPosting)
                {
                    transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.LoanApp,
                        0, m_BranchCode, m_GlDebit, loanproduct.AppraisalFeeControlAccount, repview.CustomerNo, String.Empty);
                }

                income_Account = loanproduct.AppraisalFeeIncomeAccount;
                accrued_Account = loanproduct.AppraisalFeeAccruedAccount;
                RaiseIncomeContraEntries(transactionsEngine, translist, m_TransactionDate, m_transactionid, repview.CustomerNo, m_Narration, m_Docid, m_ReferenceNo,
                         repview.LoanApp, accrued_Account, income_Account, m_BranchCode, repview.ProductCode, checkOffPosting);
            }
            if (repview.LoanPen > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo,
                        0, repview.LoanPen, m_BranchCode, m_GlDebit, loanproduct.PenaltyControlAccount, repview.ProductCode, 4, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
              repview.LoanPen, m_BranchCode, loanproduct.PenaltyControlAccount, m_GlDebit, repview.CustomerNo, String.Empty);
                if (checkOffPosting)
                {
                    transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.LoanPen,
                        0, m_BranchCode, m_GlDebit, loanproduct.PenaltyControlAccount, repview.CustomerNo, String.Empty);
                }

                income_Account = loanproduct.PenaltyIncomeAccount;
                accrued_Account = loanproduct.PenaltyAccruedAccount;
                RaiseIncomeContraEntries(transactionsEngine, translist, m_TransactionDate, m_transactionid, repview.CustomerNo, m_Narration, m_Docid, m_ReferenceNo,
                    repview.LoanPen, accrued_Account, income_Account, m_BranchCode, repview.ProductCode, checkOffPosting);
            }
            if (repview.loan_Levy > 0)
            {
                transactionsEngine.Generate_Loans_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, "Levy " + m_Narration, m_Docid, m_ReferenceNo,
                     repview.loan_Levy, repview.loan_Levy, m_BranchCode, m_GlDebit, loanproduct.CashPayment_GlAccount, repview.ProductCode, 1, transaction_groupid, m_LoanReferenceNo, maindb);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, repview.CustomerNo, m_TransactionDate, "Levy " + m_Narration, m_Docid, m_ReferenceNo, 0,
                     repview.loan_Levy, m_BranchCode, loanproduct.CashPayment_GlAccount, m_GlDebit, repview.CustomerNo, String.Empty);
            }
            if (raiseDebit == true && checkOffPosting == false)
            {
                repview.Amount = ValueConverters.ConvertNullToDouble(repview.loan_Levy) + ValueConverters.ConvertNullToDouble(repview.LoanApp) + ValueConverters.ConvertNullToDouble(repview.LoanIns) +
                    ValueConverters.ConvertNullToDouble(repview.LoanPen) + ValueConverters.ConvertNullToDouble(repview.LoanInt) + ValueConverters.ConvertNullToDouble(repview.LoanPrincipal);

                transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? repview.ProductCode : repview.CustomerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, repview.Amount,
                                    0, m_BranchCode, m_GlDebit, "Product-Rep", repview.CustomerNo, String.Empty);
            }
            else { /* Raise Final Debit in Calling Module*/}
            return translist;
        }

        public void RaiseIncomeContraEntries(PostTransactions transactionsEngine, List<PostTransactionsViewModel> translist, DateTime m_TransactionDate, string m_transactionid, string customerNo,
            string m_Narration, string m_Docid, string m_ReferenceNo, double amount, string m_GlDebit, string m_GlCredit, string m_BranchCode, string productCode = "", bool checkOffPosting = false)
        {
            transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? productCode : customerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, 0,
             amount, m_BranchCode, m_GlCredit, m_GlDebit, customerNo, String.Empty);
            transactionsEngine.Generate_Ledger_Transactions(translist, m_transactionid, (checkOffPosting ? productCode : customerNo), m_TransactionDate, m_Narration, m_Docid, m_ReferenceNo, amount,
             0, m_BranchCode, m_GlDebit, m_GlCredit, customerNo, String.Empty);
        }
    }
}