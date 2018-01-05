using ESoft.Web.Services.Common;
using EsoftMobileApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services.common
{
    public class PostTransactionsViewModel
    {
        public string AccountNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Narration { get; set; }
        public string Docid { get; set; }
        public string ReferenceNo { get; set; }
        public string LoginCode { get; set; }
        public string Machine { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public string BranchCode { get; set; }
        public DateTime AuditTime { get; set; }
        public string GlDebit { get; set; }
        public string GlCredit { get; set; }
        public string ProductCode { get; set; }
        public int Section { get; set; }
        public string TransactionId { get; set; }
        public string TableName { get; set; }
        public string ChequeNo { get; set; }
        public string Drawer { get; set; }
        public string DrawerBank { get; set; }
        public string DrawerBranch { get; set; }
        public string DrawerBankBranch { get; set; }
        public DateTime RcvdDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime ChequeDate { get; set; }
        public DateTime ChequeClearDate { get; set; }
        public string SqlStatement { get; set; }
        public string CustomerNo { get; set; }
        public string LoanTr_GroupId { get; set; }
        public string LoanReferenceNo { get; set; }
        public string Dummyfield { get; set; }
        public bool DummyLogicalField { get; set; }
        public double DummyNumericField { get; set; }
        public string GlMemSav { get; set; }
        public string Sms_Message { get; set; }
    }

    public class PostTransactions
    {
        private string loginCode = "";
        private string userMachineName = "mobile";

        private static string userBranch = "99";
        private static string transactionPostedSuccessfully = "Transactions Was Updated Successfully ";

        public static string Generate_Posting_Reference(string docid)
        {
            return GenerateReference(docid);
        }

        public string Generate_PostReference(string docid)
        {
            return GenerateReference(docid);
        }

        private static string GenerateReference(string docid)
        {
            string referenceNo = ValueConverters.RandomString(7);

            try
            {
                string query = "Exec GenerateReferenceNumber '" + ValueConverters.format_sql_string(docid) + "','" +
                    userBranch + "'";
                SqlDataReader reader = DbConnector.GetSqlReader(query);
                while (reader.Read())
                {
                    referenceNo = reader["ReferenceNumber"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ref ex);
            }
            return referenceNo;
        }

        public List<PostTransactionsViewModel> Generate_Sql_Statement
           (List<PostTransactionsViewModel> transList, string m_current_posting_reference, string sqlStatement)
        {
            if (ValueConverters.IsStringEmpty(sqlStatement))
                return transList;

            transList.Add(new PostTransactionsViewModel
            {
                TableName = "SQLSTATEMENT",
                SqlStatement = sqlStatement,
                TransactionId = m_current_posting_reference,
                TransactionDate = DateTime.Now
            });

            return transList;
        }

        public List<PostTransactionsViewModel> Generate_Savings_Transactions
            (List<PostTransactionsViewModel> transList, string m_current_posting_reference, string m_AccountNo, DateTime m_TransactionDate, string m_Narration, string m_Docid, string m_ReferenceNo,
            double m_DebitAmount, double m_CreditAmount, string m_BranchCode, string m_GlDebit, string m_GlCredit, string m_CustomerNo, string m_glMemSav, string m_Sms_message_to_send, string loginCode)
        {
            m_GlDebit = ValueConverters.ConvertNullToEmptyString(m_GlDebit);
            m_GlCredit = ValueConverters.ConvertNullToEmptyString(m_GlCredit);

            m_Sms_message_to_send = ValueConverters.format_sql_string(m_Sms_message_to_send);
            m_DebitAmount = ValueConverters.ConvertNullToDouble(m_DebitAmount);
            m_CreditAmount = ValueConverters.ConvertNullToDouble(m_CreditAmount);
            if (m_DebitAmount < 0.00)
            {
                m_CreditAmount = Math.Abs(m_DebitAmount);
                m_DebitAmount = 0.00;
            }
            if (m_CreditAmount < 0.00)
            {
                m_DebitAmount = Math.Abs(m_CreditAmount);
                m_CreditAmount = 0.00;
            }

            transList.Add(new PostTransactionsViewModel
            {
                AccountNo = m_AccountNo,
                TransactionDate = m_TransactionDate,
                Narration = m_Narration,
                Docid = m_Docid,
                ReferenceNo = m_ReferenceNo,
                LoginCode = loginCode,
                Machine = "",
                DebitAmount = m_DebitAmount,
                CreditAmount = m_CreditAmount,
                BranchCode = m_BranchCode,
                GlDebit = m_GlDebit,
                GlCredit = m_GlCredit,
                ProductCode = "",
                Section = 0,
                TransactionId = m_current_posting_reference,
                TableName = "SAVINGS",
                CustomerNo = m_CustomerNo,
                GlMemSav = m_glMemSav,
                Sms_Message = m_Sms_message_to_send

            });

            return transList;
        }

        public List<PostTransactionsViewModel> Generate_Ledger_Transactions
            (List<PostTransactionsViewModel> transList,
             string m_current_posting_reference, string m_AccountNo, DateTime m_TransactionDate, string m_Narration, string m_Docid, string m_ReferenceNo, double m_DebitAmount,
        double m_CreditAmount, string m_BranchCode, string m_GlAccount, string m_GlContra, string m_CustomerNo, string loginCode)
        {
            if (m_DebitAmount < 0.00)
            {
                m_CreditAmount = Math.Abs(m_DebitAmount);
                m_DebitAmount = 0.00;
            }
            if (m_CreditAmount < 0.00)
            {
                m_DebitAmount = Math.Abs(m_CreditAmount);
                m_CreditAmount = 0.00;
            }
            m_GlAccount = ValueConverters.ConvertNullToEmptyString(m_GlAccount);
            m_GlContra = ValueConverters.ConvertNullToEmptyString(m_GlContra);
            m_AccountNo = ValueConverters.ConvertNullToEmptyString(m_AccountNo);
            m_CustomerNo = ValueConverters.ConvertNullToEmptyString(m_CustomerNo);

            transList.Add(new PostTransactionsViewModel
            {
                AccountNo = m_AccountNo,
                TransactionDate = m_TransactionDate,
                Narration = m_Narration,
                Docid = m_Docid,
                ReferenceNo = m_ReferenceNo,
                LoginCode = loginCode,
                Machine = "",
                DebitAmount = m_DebitAmount,
                CreditAmount = m_CreditAmount,
                BranchCode = m_BranchCode,
                GlDebit = m_GlAccount,
                GlCredit = m_GlContra,
                ProductCode = "GL",
                Section = 1,
                TransactionId = m_current_posting_reference,
                TableName = "LEDGER",
                CustomerNo = m_CustomerNo
            });

            return transList;


        }

        public List<PostTransactionsViewModel> Generate_Shares_Transactions
            (List<PostTransactionsViewModel> transList, string m_current_posting_reference, string m_AccountNo, DateTime m_TransactionDate, string m_Narration,
            string m_Docid, string m_ReferenceNo, double m_DebitAmount,
    double m_CreditAmount, string m_BranchCode, string m_GlDebit, string m_GlCredit, string m_productCode, string loginCode)
        {
            if (m_DebitAmount < 0.00)
            {
                m_CreditAmount = Math.Abs(m_DebitAmount);
                m_DebitAmount = 0.00;
            }
            if (m_CreditAmount < 0.00)
            {
                m_DebitAmount = Math.Abs(m_CreditAmount);
                m_CreditAmount = 0.00;
            }
            m_GlDebit = ValueConverters.ConvertNullToEmptyString(m_GlDebit);
            m_GlCredit = ValueConverters.ConvertNullToEmptyString(m_GlCredit);

            transList.Add(new PostTransactionsViewModel
            {
                AccountNo = m_AccountNo,
                TransactionDate = m_TransactionDate,
                Narration = m_Narration,
                Docid = m_Docid,
                ReferenceNo = m_ReferenceNo,
                LoginCode = loginCode,
                Machine = "",
                DebitAmount = m_DebitAmount,
                CreditAmount = m_CreditAmount,
                BranchCode = m_BranchCode,
                GlDebit = m_GlDebit,
                GlCredit = m_GlCredit,
                ProductCode = m_productCode,
                Section = 1,
                TransactionId = m_current_posting_reference,
                TableName = "SHARES"
            });

            return transList;
        }

        public List<PostTransactionsViewModel> Generate_Loans_Transactions
           (List<PostTransactionsViewModel> transList, string m_current_posting_reference, string m_AccountNo, DateTime m_TransactionDate, string m_Narration,
           string m_Docid, string m_ReferenceNo, double m_DebitAmount,
   double m_CreditAmount, string m_BranchCode, string m_GlDebit, string m_GlCredit, string m_productCode, int loan_section, string transaction_groupid, string m_LoanReferenceNo,
            string tellerLoginCode, Esoft_WebEntities maindb = null)
        {
            if (m_DebitAmount < 0.00)
            {
                m_CreditAmount = Math.Abs(m_DebitAmount);
                m_DebitAmount = 0.00;
            }
            if (m_CreditAmount < 0.00)
            {
                m_DebitAmount = Math.Abs(m_CreditAmount);
                m_CreditAmount = 0.00;
            }
            if (ValueConverters.IsStringEmpty(m_LoanReferenceNo))
            {
                if (maindb == null)
                {
                    maindb = new Esoft_WebEntities();
                }
                var loanRecord = maindb.tbl_LoanMasterTable.FirstOrDefault(x => x.customerNo == m_AccountNo && x.LoanCode == m_productCode);
                if (loanRecord != null)
                {
                    m_LoanReferenceNo = ValueConverters.ConvertNullToEmptyString(loanRecord.ReferenceNo);
                }
            }

            m_GlDebit = ValueConverters.ConvertNullToEmptyString(m_GlDebit);
            m_GlCredit = ValueConverters.ConvertNullToEmptyString(m_GlCredit);
            transList.Add(new PostTransactionsViewModel
            {
                AccountNo = m_AccountNo,
                TransactionDate = m_TransactionDate,
                Narration = m_Narration,
                Docid = m_Docid,
                ReferenceNo = m_ReferenceNo,
                LoginCode = tellerLoginCode,
                Machine = "",
                DebitAmount = m_DebitAmount,
                CreditAmount = m_CreditAmount,
                BranchCode = m_BranchCode,
                GlDebit = m_GlDebit,
                GlCredit = m_GlCredit,
                ProductCode = m_productCode,
                Section = loan_section,
                TransactionId = m_current_posting_reference,
                TableName = "LOANS",
                LoanReferenceNo = m_LoanReferenceNo,
                LoanTr_GroupId = transaction_groupid
            });

            return transList;
        }

        public List<PostTransactionsViewModel> Log_denominations(List<PostTransactionsViewModel> transList, string m_current_posting_reference, DenominationCounter denomCounter)
        {
            string denomInsertList = "INSERT INTO Tbl_denoms(docid,REFERENCENO,X_1000,X_500,X_200,X_100,X_50,X_40,X_20,X_10,X_5,X_1,X_050,X_010,X_005,transactionamount,GLDEBIT,GLCREDIT,TransactionDate)" +
             " Values('" + ValueConverters.format_sql_string(denomCounter.Docid) + "','" + ValueConverters.format_sql_string(denomCounter.ReferenceNo) + "'," +
            denomCounter.X_1000.ToString() + "," + denomCounter.X_500.ToString() + "," + denomCounter.X_200.ToString() + "," + denomCounter.X_100.ToString() + "," + denomCounter.X_50.ToString() + "," +
            denomCounter.X_40.ToString() + "," + denomCounter.X_20.ToString() + "," + denomCounter.X_10.ToString() + "," + denomCounter.X_5.ToString() + "," +
            denomCounter.X_1.ToString() + "," + denomCounter.X_050.ToString() + "," + denomCounter.X_010.ToString() + "," + denomCounter.X_005.ToString() + "," +
            denomCounter.TransactionAmount.ToString() + ",'" + ValueConverters.format_sql_string(denomCounter.GlDebit) + "','" +
            ValueConverters.format_sql_string(denomCounter.GlCredit) + "','" + ValueConverters.FormatDateAsSQlParameter(denomCounter.TransactionDate) + "')";

            transList.Add(new PostTransactionsViewModel
            {
                TableName = "SQLSTATEMENT",
                SqlStatement = denomInsertList,
                TransactionId = m_current_posting_reference
            });


            return transList;
        }

        public string Post_Transactions(IEnumerable<PostTransactionsViewModel> transList,
            string m_current_posting_reference, bool _post_show_message, bool _dont_raise_alerts, bool summariseLedgerTransactions = false)
        {
            string result = string.Empty;

            var transactionList = from x in transList
                                  where x.TransactionId == m_current_posting_reference
                                  select x;

            //var transactionList = transList.OrderBy(x => x.TableName).Select(x => x.TransactionId == m_current_posting_reference);
            if (transactionList == null)
            {
                return result;
            }

            var all_ledgerTransactions = transList.Where(x => x.TableName.Trim() == "LEDGER" && x.TransactionId == m_current_posting_reference);

            var ledgerTransactions = all_ledgerTransactions.FirstOrDefault(x => ValueConverters.IsStringEmpty(x.GlDebit) == true);

            if (ledgerTransactions != null)
            {
                return String.Format("Some Transactions Do Not Have Ledger Accounts. Check Description {0}, Account {1}", ledgerTransactions.Narration.Trim(), ledgerTransactions.AccountNo.Trim());
            }

            double ledgerTotal = (double)all_ledgerTransactions.Sum(x => x.DebitAmount - x.CreditAmount);

            if (Math.Round(ledgerTotal, 2) != 0.00)
            {
                //ToDo Uncomment this line return String.Format("Ledger Transactions for Posted Journals Not Balancing with {0} ", ledgerTotal.ToString("N2"));
            }

            if (summariseLedgerTransactions)
            {
                transactionList = SummariseLedgerPostings(transactionList, m_current_posting_reference);
            }

            string savings_insertstatement = " INSERT INTO tbl_savings(AccountNo,TransactionDate,Narration,Docid,ReferenceNo,LoginCode,Machine," +
                "DebitAmount,CreditAmount,BranchCode,AuditTime,GlDebit,GlCredit)values(@AccountNo,@TransactionDate,@narration," +
                 "@Docid,@ReferenceNo,@LOGINCODE,@machine,@DebitAmount,@CreditAmount,@BranchCode,@AuditTime,@GlDebit,@GlCredit)";

            string shares_insertstatement = " INSERT INTO tbl_shares(AccountNo,TransactionDate,Narration,Docid,ReferenceNo,LoginCode,Machine," +
                " DebitAmount,CreditAmount,BranchCode,AuditTime,GlDebit,GlCredit,ProductCode)values(@AccountNo,@TransactionDate,@narration," +
                 "@Docid,@ReferenceNo,@LOGINCODE,@machine,@DebitAmount,@CreditAmount,@BranchCode,@AuditTime,@GlDebit,@GlCredit,@ProductCode)";

            string loans_insertstatement = " INSERT INTO tbl_loans(AccountNo,TransactionDate,Narration,Docid,ReferenceNo,LoginCode,Machine," +
                 " DebitAmount,CreditAmount,BranchCode,AuditTime,GlDebit,GlCredit,ProductCode,Section,GroupID,LoanReferenceNo)values(" +
                 " @AccountNo,@TransactionDate,@narration," +
                 " @Docid,@ReferenceNo,@LOGINCODE,@machine,@DebitAmount,@CreditAmount,@BranchCode,@AuditTime,@GlDebit,@GlCredit,@ProductCode,@section," +
                 " @LoanTr_GroupId,@LoanReferenceNo)";

            string ledger_insertstatement = " INSERT INTO tbl_Ledger(AccountNo,TransactionDate,Narration,Docid,ReferenceNo,LoginCode,Machine," +
                   " DebitAmount,CreditAmount,BranchCode,AuditTime,GlAccountNo,GlContra)values(@AccountNo,@TransactionDate,@narration," +
                    " @Docid,@ReferenceNo,@LOGINCODE,@machine,@DebitAmount,@CreditAmount,@BranchCode,@AuditTime,@GlDebit,@GlCredit)";

            string cheques_insertstatement = " INSERT INTO tbl_cheques(AccountNo,ChequeNo,ChequeAmount,ChequeDate,DrawerName,DrawerBank," +
                 " DrawerBankBranch,DateReceived,MaturityDate,PostBy,SourceBranch,CommissionCharged,ChequeType,SourceReferenceNo,GlDebit,GlCredit,DateCleared)values(" +
                 " @AccountNo,@chequeno,@DebitAmount,@ChequeDate,@drawer,@drawerbank,@DrawerBankBranch,@RcvdDate,@maturitydate,@LOGINCode," +
                 "@BranchCode,@CreditAmount,@ProductCode,@ReferenceNo,@GlDebit,@GlCredit,@DateCleared)";

            //string denomInsertList = "INSERT INTO Tbl_denoms(docid,REFERENCENO,X_1000,X_500,X_200,X_100,X_50,X_40,X_20,X_10,X_5,X_1,X_050,X_010,X_005,transactionamount,GLDEBIT,GLCREDIT,TransactionDate)";

            string mainDbConnectionstring = DbConnector.MainDbConnectionString();
            DateTime serverDate = DateTime.Now;
            string userLoginCode = String.Empty;

            //string userMachineName = userMachineName;

            using (SqlConnection sqlConnection = new SqlConnection(mainDbConnectionstring))
            {
                sqlConnection.Open();
                SqlTransaction sqlTrans = sqlConnection.BeginTransaction("Esoft_insert");

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Transaction = sqlTrans;
                    try
                    {
                        foreach (PostTransactionsViewModel tr in transactionList)
                        {
                            userLoginCode = tr.LoginCode ?? string.Empty;
                            //Parallel.ForEach(transactionList, tr =>
                            //{
                            tr.AuditTime = serverDate;

                            //sqlCommand.Parameters.AddRange(GetInsertEarningsParameters());
                            sqlCommand.Parameters.Clear();

                            switch (tr.TableName)
                            {
                                case "SAVINGS":
                                    sqlCommand.Parameters.AddRange(GetSavingsInsertEarningsParameters());
                                    FillSavingsInsertParameterValues(tr, sqlCommand.Parameters, userLoginCode, userMachineName);
                                    sqlCommand.CommandText = savings_insertstatement;
                                    break;
                                case "LOANS":
                                    sqlCommand.Parameters.AddRange(GetLoansInsertEarningsParameters());
                                    FillLoansInsertParameterValues(tr, sqlCommand.Parameters, userLoginCode, userMachineName);
                                    sqlCommand.CommandText = loans_insertstatement;
                                    break;
                                case "SHARES":
                                    sqlCommand.Parameters.AddRange(GetSharesInsertEarningsParameters());
                                    FillSharesInsertParameterValues(tr, sqlCommand.Parameters, userLoginCode, userMachineName);
                                    sqlCommand.CommandText = shares_insertstatement;
                                    break;
                                case "LEDGER":
                                    sqlCommand.Parameters.AddRange(GetLedgerInsertEarningsParameters());
                                    FillLedgerInsertParameterValues(tr, sqlCommand.Parameters, userLoginCode, userMachineName);
                                    sqlCommand.CommandText = ledger_insertstatement;
                                    break;
                                case "CHEQUE":
                                    sqlCommand.Parameters.AddRange(GetChequeInsertEarningsParameters());
                                    FillChequeInsertParameterValues(tr, sqlCommand.Parameters, userLoginCode, userMachineName);
                                    sqlCommand.CommandText = cheques_insertstatement;
                                    break;
                                case "SQLSTATEMENT":
                                    if (string.IsNullOrEmpty(tr.SqlStatement))
                                        continue;//return;
                                    sqlCommand.CommandText = tr.SqlStatement;
                                    //Utility.WriteErrorLog(tr.SqlStatement);
                                    break;
                                case "SMSMESSAGE":
                                    if (string.IsNullOrEmpty(tr.SqlStatement))
                                        continue;//return;
                                    sqlCommand.CommandText = tr.SqlStatement;
                                    break;
                                default:
                                    //    return;
                                    continue;
                            }
                            sqlCommand.ExecuteNonQuery();
                        }

                        sqlTrans.Commit();
                        result = transactionPostedSuccessfully;
                    }
                    catch (Exception trans_expection)
                    {
                        result = "Transactions Could not Be Updated";
                        sqlTrans.Rollback();
                        if (sqlConnection.State == System.Data.ConnectionState.Open)
                            sqlConnection.Close();
                        Utility.WriteErrorLog(ref trans_expection);
                        if (Debugger.IsAttached == true)
                        {
                            Utility.WriteErrorLog(sqlCommand.CommandText);
                        }
                    }
                }
            }

            return result;
        }
        private static SqlParameter[] GetSavingsInsertEarningsParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Narration", SqlDbType.VarChar, 100));
            sqlParameters.Add(new SqlParameter("@Docid", SqlDbType.Char, 4));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@LoginCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Machine", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@AuditTime", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));

            return sqlParameters.ToArray();
        }

        private static void FillSavingsInsertParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {
            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@TransactionDate"].Value = singleTr.TransactionDate;
            parameters["@Narration"].Value = singleTr.Narration;
            parameters["@Docid"].Value = singleTr.Docid;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@LoginCode"].Value = userLoginCode;
            parameters["@Machine"].Value = userMachineName;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@AuditTime"].Value = singleTr.AuditTime;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
        }


        private static SqlParameter[] GetSharesInsertEarningsParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Narration", SqlDbType.VarChar, 100));
            sqlParameters.Add(new SqlParameter("@Docid", SqlDbType.Char, 4));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@LoginCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Machine", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@AuditTime", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@ProductCode", SqlDbType.Char, 3));
            return sqlParameters.ToArray();
        }

        private static void FillSharesInsertParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {
            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@TransactionDate"].Value = singleTr.TransactionDate;
            parameters["@Narration"].Value = singleTr.Narration;
            parameters["@Docid"].Value = singleTr.Docid;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@LoginCode"].Value = userLoginCode;
            parameters["@Machine"].Value = userMachineName;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@AuditTime"].Value = singleTr.AuditTime;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
            parameters["@ProductCode"].Value = singleTr.ProductCode;
        }


        private static SqlParameter[] GetLoansInsertEarningsParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Narration", SqlDbType.VarChar, 100));
            sqlParameters.Add(new SqlParameter("@Docid", SqlDbType.Char, 4));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@LoginCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Machine", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@AuditTime", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@ProductCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Section", SqlDbType.Int));
            sqlParameters.Add(new SqlParameter("@LoanTr_GroupId", SqlDbType.VarChar, 3));
            sqlParameters.Add(new SqlParameter("@LoanReferenceNo", SqlDbType.VarChar, 7));

            return sqlParameters.ToArray();
        }

        private static void FillLoansInsertParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {
            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@TransactionDate"].Value = singleTr.TransactionDate;
            parameters["@Narration"].Value = singleTr.Narration;
            parameters["@Docid"].Value = singleTr.Docid;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@LoginCode"].Value = userLoginCode;
            parameters["@Machine"].Value = userMachineName;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@AuditTime"].Value = singleTr.AuditTime;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
            parameters["@ProductCode"].Value = singleTr.ProductCode;
            parameters["@Section"].Value = singleTr.Section;
            parameters["@LoanTr_GroupId"].Value = singleTr.LoanTr_GroupId ?? "";
            parameters["@LoanReferenceNo"].Value = singleTr.LoanReferenceNo ?? "";
        }

        private static SqlParameter[] GetLedgerInsertEarningsParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Narration", SqlDbType.VarChar, 100));
            sqlParameters.Add(new SqlParameter("@Docid", SqlDbType.Char, 4));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@LoginCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Machine", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@AuditTime", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));

            return sqlParameters.ToArray();
        }

        private static void FillLedgerInsertParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {
            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@TransactionDate"].Value = singleTr.TransactionDate;
            parameters["@Narration"].Value = singleTr.Narration;
            parameters["@Docid"].Value = singleTr.Docid;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@LoginCode"].Value = userLoginCode;
            parameters["@Machine"].Value = userMachineName;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@AuditTime"].Value = singleTr.AuditTime;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
        }

        private static SqlParameter[] GetChequeInsertEarningsParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@ChequeNo", SqlDbType.VarChar, 6));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@ChequeDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Drawer", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBank", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBranch", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBankBranch", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@MaturityDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@LOGINCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@ProductCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@RcvdDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@DateCleared", SqlDbType.DateTime));

            return sqlParameters.ToArray();
        }

        private static void FillChequeInsertParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {

            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@ChequeNo"].Value = singleTr.ChequeNo;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@ChequeDate"].Value = singleTr.ChequeDate;
            parameters["@Drawer"].Value = singleTr.Drawer;
            parameters["@DrawerBank"].Value = singleTr.DrawerBank;
            parameters["@DrawerBranch"].Value = singleTr.DrawerBranch;
            parameters["@DrawerBankBranch"].Value = singleTr.DrawerBankBranch;
            parameters["@MaturityDate"].Value = singleTr.MaturityDate;
            parameters["@LOGINCode"].Value = userLoginCode;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@ProductCode"].Value = singleTr.ProductCode;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
            parameters["@RcvdDate"].Value = singleTr.RcvdDate;
            parameters["@DateCleared"].Value = ValueConverters.ConvertNullToDatetime(singleTr.ChequeClearDate);

        }

        private static SqlParameter[] GetLog_Denominations_Parameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@docid", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@REFERENCENO", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@X_1000", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_500", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_200", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_100", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_50", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_40", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_20", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_10", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_5", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_1", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_050", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_010", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@X_005", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@transactionamount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@GLDEBIT", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GLCREDIT", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));

            return sqlParameters.ToArray();
        }

        private static void FillLog_Denominations_Parameters(DenominationCounter denomCounter, SqlParameterCollection parameters)
        {
            parameters["@docid"].Value = denomCounter.Docid;
            parameters["@REFERENCENO"].Value = denomCounter.ReferenceNo;
            parameters["@X_1000"].Value = denomCounter.X_1000;
            parameters["@X_500"].Value = denomCounter.X_500;
            parameters["@X_200"].Value = denomCounter.X_200;
            parameters["@X_100"].Value = denomCounter.X_100;
            parameters["@X_50"].Value = denomCounter.X_50;
            parameters["@X_40"].Value = denomCounter.X_40;
            parameters["@X_20"].Value = denomCounter.X_20;
            parameters["@X_10"].Value = denomCounter.X_10;
            parameters["@X_5"].Value = denomCounter.X_5;
            parameters["@X_1"].Value = denomCounter.X_1;
            parameters["@X_050"].Value = denomCounter.X_050;
            parameters["@X_010"].Value = denomCounter.X_010;
            parameters["@X_005"].Value = denomCounter.X_005;
            parameters["@transactionamount"].Value = denomCounter.TransactionAmount;
            parameters["@GLDEBIT"].Value = denomCounter.GlDebit;
            parameters["@GLCREDIT"].Value = denomCounter.GlCredit;
            parameters["@TransactionDate"].Value = denomCounter.TransactionDate;
        }

        private static void FillInsertAllParameterValues(PostTransactionsViewModel singleTr, SqlParameterCollection parameters, string userLoginCode, string userMachineName)
        {
            parameters["@AccountNo"].Value = singleTr.AccountNo;
            parameters["@TransactionDate"].Value = singleTr.TransactionDate;
            parameters["@Narration"].Value = singleTr.Narration;
            parameters["@Docid"].Value = singleTr.Docid;
            parameters["@ReferenceNo"].Value = singleTr.ReferenceNo;
            parameters["@LoginCode"].Value = userLoginCode;
            parameters["@Machine"].Value = userMachineName;
            parameters["@DebitAmount"].Value = singleTr.DebitAmount;
            parameters["@CreditAmount"].Value = singleTr.CreditAmount;
            parameters["@BranchCode"].Value = singleTr.BranchCode;
            parameters["@AuditTime"].Value = singleTr.AuditTime;
            parameters["@GlDebit"].Value = singleTr.GlDebit;
            parameters["@GlCredit"].Value = singleTr.GlCredit;
            parameters["@ProductCode"].Value = singleTr.ProductCode;
            parameters["@Section"].Value = singleTr.Section;
            parameters["@TransactionId"].Value = singleTr.TransactionId;
            parameters["@TableName"].Value = singleTr.TableName;
            parameters["@ChequeNo"].Value = singleTr.ChequeNo;
            parameters["@Drawer"].Value = singleTr.Drawer;
            parameters["@DrawerBank"].Value = singleTr.DrawerBank;
            parameters["@DrawerBranch"].Value = singleTr.DrawerBranch;
            parameters["@DrawerBankBranch"].Value = singleTr.DrawerBankBranch;
            parameters["@RcvdDate"].Value = ValueConverters.ConvertNullToDatetime(singleTr.RcvdDate);
            parameters["@MaturityDate"].Value = ValueConverters.ConvertNullToDatetime(singleTr.MaturityDate);
            parameters["@ChequeDate"].Value = ValueConverters.ConvertNullToDatetime(singleTr.ChequeDate);
            parameters["@SqlStatement"].Value = singleTr.SqlStatement;
            parameters["@CustomerNo"].Value = singleTr.CustomerNo;
            parameters["@LoanTr_GroupId"].Value = singleTr.LoanTr_GroupId;
            parameters["@LoanReferenceNo"].Value = singleTr.LoanReferenceNo;
            parameters["@Dummyfield"].Value = singleTr.Dummyfield;
            parameters["@DummyLogicalField"].Value = singleTr.DummyLogicalField;
            parameters["@DummyNumericField"].Value = singleTr.DummyNumericField;
            parameters["@GlMemSav"].Value = singleTr.GlMemSav;
            parameters["@Sms_Message"].Value = singleTr.Sms_Message;

        }


        private static SqlParameter[] GetInsertAllParameters()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@AccountNo", SqlDbType.Char, 20));
            sqlParameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@Narration", SqlDbType.VarChar, 100));
            sqlParameters.Add(new SqlParameter("@Docid", SqlDbType.Char, 4));
            sqlParameters.Add(new SqlParameter("@ReferenceNo", SqlDbType.Char, 7));
            sqlParameters.Add(new SqlParameter("@LoginCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Machine", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DebitAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@CreditAmount", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@BranchCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@AuditTime", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@GlDebit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@GlCredit", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@ProductCode", SqlDbType.Char, 3));
            sqlParameters.Add(new SqlParameter("@Section", SqlDbType.Int));
            sqlParameters.Add(new SqlParameter("@TransactionId", SqlDbType.VarChar, 10));
            sqlParameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar, 30));
            sqlParameters.Add(new SqlParameter("@ChequeNo", SqlDbType.VarChar, 6));
            sqlParameters.Add(new SqlParameter("@Drawer", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBank", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBranch", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@DrawerBankBranch", SqlDbType.VarChar, 50));
            sqlParameters.Add(new SqlParameter("@RcvdDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@MaturityDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@ChequeDate", SqlDbType.DateTime));
            sqlParameters.Add(new SqlParameter("@SqlStatement", SqlDbType.VarChar, 500));
            sqlParameters.Add(new SqlParameter("@CustomerNo", SqlDbType.VarChar, 10));
            sqlParameters.Add(new SqlParameter("@LoanTr_GroupId", SqlDbType.VarChar, 3));
            sqlParameters.Add(new SqlParameter("@LoanReferenceNo", SqlDbType.VarChar, 7));
            sqlParameters.Add(new SqlParameter("@Dummyfield", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@DummyLogicalField", SqlDbType.Bit));
            sqlParameters.Add(new SqlParameter("@DummyNumericField", SqlDbType.Float));
            sqlParameters.Add(new SqlParameter("@GlMemSav", SqlDbType.VarChar, 20));
            sqlParameters.Add(new SqlParameter("@Sms_Message", SqlDbType.VarChar, 159));
            return sqlParameters.ToArray();
        }


        public void Generate_Sms_Message_NoCharge(List<PostTransactionsViewModel> transList, string m_transactionid, string customerNo, string mobilePhoneNo, string smsMessage)
        {
            mobilePhoneNo = ValueConverters.format_sql_string(mobilePhoneNo).Trim();
            string validMobile = ValueConverters.ChrTran(mobilePhoneNo, ValueConverters.ChrTran(mobilePhoneNo, "0123456789", ""), "");
            if (mobilePhoneNo == validMobile && !ValueConverters.IsStringEmpty(smsMessage))
            {
                string _sms_STRING = " INSERT INTO EsoftMessaging.dbo.Esoft_MessageOut(CustomerNo,MobilePhoneNo,MessageOut,DateRaised,Status)" +
                             " VALUES('" + ValueConverters.format_sql_string(customerNo) + "','" + ValueConverters.format_sql_string(mobilePhoneNo) + "','" +
                             ValueConverters.format_sql_string(smsMessage) + "',GetDate(),0)";
                transList.Add(new PostTransactionsViewModel
                {
                    SqlStatement = _sms_STRING,
                    TableName = "SMSMESSAGE",
                    TransactionId = m_transactionid
                });
            }
        }

        public string CheckLongRunningProcess(string docid, string referenceNo, string postingKey)
        {
            string postedBy = string.Empty;
            try
            {
                string sqlString = "Declare @exist varchar(3)='' ";
                if (string.IsNullOrWhiteSpace(referenceNo))
                {
                    sqlString += " SELECT TOP 1 @exist= PostBy FROM tbl_LongRunningProcesses WHERE PostingDocid='" +
                        ValueConverters.format_sql_string(docid) + "' and PostingId='" + ValueConverters.format_sql_string(postingKey) + "' ";
                }
                else
                {
                    //ToDo Module to reset a Transaction to allow Reposting ... since anything on tbl_LongRunningProcesses cannot post twice
                    sqlString += " SELECT TOP 1 @exist= PostBy FROM tbl_LongRunningProcesses WHERE PostingDocid='" +
                        ValueConverters.format_sql_string(docid) + "' and PostingReferenceNo='" + ValueConverters.format_sql_string(referenceNo) + "' ";
                }

                sqlString += " if coalesce(@exist ,'')='' begin insert into tbl_LongRunningProcesses(PostingId,PostingDocid,PostingReferenceNo,PostDate,PostBy,PostAddress)" +
                         " values('" + ValueConverters.format_sql_string(postingKey) + "','" + ValueConverters.format_sql_string(docid) + "','" +
                         ValueConverters.format_sql_string(referenceNo) + "',getdate(),'" +
                        ValueConverters.format_sql_string(loginCode) + "','" +
                        ValueConverters.format_sql_string(userMachineName) + "') end " +
                " select isnull((select top 1 coalesce(fullName,@exist)  from tbl_users where LoginCode= coalesce(@exist,'')),@exist) as PostedBy";


                DbDataReader reader = DbConnector.GetSqlReader(sqlString);
                while (reader.Read())
                {
                    postedBy = ValueConverters.ConvertNullToEmptyString(reader["PostedBy"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(postedBy))
                {
                    postedBy = "Transaction Is Being/(Has Been) Posted by " + postedBy;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog("CheckLongRunningProcess", ref ex);
                postedBy = "Error Occured. Re-Try Posting Transaction";
            }
            return postedBy;
        }

        public void DeleteLongRunningProcess(string docid, string referenceNo, string postingKey)
        {
            DbConnector.ExecuteSQL_("Delete tbl_LongRunningProcesses WHERE PostingDocid='" +
                         ValueConverters.format_sql_string(docid) + "' and PostingId='" + ValueConverters.format_sql_string(postingKey) + "'");
        }

        private IEnumerable<PostTransactionsViewModel> SummariseLedgerPostings(IEnumerable<PostTransactionsViewModel> transList, string m_transactionid)
        {
            List<PostTransactionsViewModel> transListx = transList.ToList(); //IEnumerable to List
            List<PostTransactionsViewModel> ledgerTransactions = transList.Where(x => x.TableName.Trim() == "LEDGER").ToList();
            if (ledgerTransactions != null && ledgerTransactions.Count() != 0)
            {
                transListx.RemoveAll(x => x.TableName.Trim() == "LEDGER");

                var summarisedTransactions = (from transaction in ledgerTransactions
                                              group transaction by new
                                              {
                                                  TransactionDate = transaction.TransactionDate,
                                                  Narration = transaction.Narration,
                                                  Docid = transaction.Docid,
                                                  ReferenceNo = transaction.ReferenceNo,
                                                  LoginCode = transaction.LoginCode,
                                                  Machine = transaction.Machine,
                                                  BranchCode = transaction.BranchCode,
                                                  GlDebit = transaction.GlDebit,
                                                  GlCredit = transaction.GlCredit,
                                                  ProductCode = transaction.ProductCode,
                                                  Section = transaction.Section,
                                                  TransactionId = transaction.TransactionId,
                                                  TableName = transaction.TableName
                                              }
                                                  into grpTransactions
                                                  select new PostTransactionsViewModel
                                                  {
                                                      AccountNo = "VARIOUS",
                                                      TransactionDate = grpTransactions.Key.TransactionDate,
                                                      Narration = grpTransactions.Key.Narration,
                                                      Docid = grpTransactions.Key.Docid,
                                                      ReferenceNo = grpTransactions.Key.ReferenceNo,
                                                      LoginCode = grpTransactions.Key.LoginCode,
                                                      Machine = grpTransactions.Key.Machine,
                                                      BranchCode = grpTransactions.Key.BranchCode,
                                                      GlDebit = grpTransactions.Key.GlDebit,
                                                      GlCredit = grpTransactions.Key.GlCredit,
                                                      ProductCode = grpTransactions.Key.ProductCode,
                                                      Section = grpTransactions.Key.Section,
                                                      TransactionId = grpTransactions.Key.TransactionId,
                                                      TableName = grpTransactions.Key.TableName,
                                                      DebitAmount = grpTransactions.Sum(x => x.DebitAmount),
                                                      CreditAmount = grpTransactions.Sum(x => x.CreditAmount)
                                                  }).ToList();

                transListx.AddRange(summarisedTransactions);
            }

            return transListx;
        }
    }


}