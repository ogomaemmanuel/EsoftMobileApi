using EsoftMobileApi.Services.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Models
{

    public class AccountDetails
    {
        public String AccountName { get; set; }
        public Decimal AccountBalance { get; set; }
    }
    public class CustomerSavings
    {
        public long RECNO { get; set; }
        public string AccountNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string NARRATION { get; set; }
        public string DOCID { get; set; }
        public string ReferenceNo { get; set; }
        public string LoginCode { get; set; }
        public string Machine { get; set; }
        public decimal? DEBIT { get; set; }
        public decimal? CREDIT { get; set; }
        public decimal? balance { get; set; }
        public decimal? available { get; set; }
        public string BranchCode { get; set; }
        public DateTime? AuditTime { get; set; }
        public string ModuleName { get; set; }
        public string LoginName { get; set; }
    }

    public class CustomerLoanStatementViewModel
    {
        public string AccountNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Narration { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string ReferenceNo { get; set; }
        public string LoginName { get; set; }
        public decimal IntDr { get; set; }
        public decimal IntCr { get; set; }
        public decimal IntBal { get; set; }
        public decimal InsDr { get; set; }
        public decimal InsCr { get; set; }
        public decimal InsBal { get; set; }
        public decimal AppDb { get; set; }
        public decimal AppCr { get; set; }
        public decimal AppBal { get; set; }
        public decimal PenDr { get; set; }
        public decimal PenCr { get; set; }
        public decimal PenBal { get; set; }
        public decimal TotBalance { get; set; }

        // Financial Statement View ( Combine Penalty,Insurance,Appraisal)
        public decimal OtherDr { get; set; }
        public decimal OtherCr { get; set; }
        public decimal OtherBal { get; set; }
    }

    public class ProductsView
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Balance { get; set; }
        public string ProductType { get; set; }
        public string AccountNo { get; set; }
    }


    public class CustomerBalances
    {
        public string CustomerNo { get; set; }
        public string AccountNo { get; set; }
        public string CustomerName { get; set; }
        public string Category { get; set; }
        public string Ttype { get; set; }
        public string ProductName { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double IntBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double InsBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double PenBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double AppBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double OtherCharges { get; set; } // Sum of Insurance/Penalty/Appraisal
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double TotalBalance { get; set; }

        //Added to handle LoanVariations view on the table
        public decimal SlineInterestAmount { get; set; }
        public decimal CurrentRepaymentAmount { get; set; }
    }

    public class CustomerInvestmentStatementViewModel
    {
        public string AccountNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Narration { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal? Balance { get; set; }
        public string ReferenceNo { get; set; }
        public string LoginName { get; set; }
    }

    public class CustomerSavingsStatementViewModel
    {
        public string AccountNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Narration { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Debit { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Credit { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Available { get; set; }
        public string ReferenceNo { get; set; }
        public string LoginName { get; set; }
        public string DOCID { get; set; }
    }

    public class Statement
    {
        public string ReferenceNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class LinkedAtmCards
    {
        public long recno { get; set; }
        public string AccountNo { get; set; }
        public string CardNumber { get; set; }
        public string Branch { get; set; }
        public string BranchName { get; set; }
        public decimal Enabled { get; set; }
        public DateTime DateLinked { get; set; }
        public string LinkedBy { get; set; }
        public decimal? verify { get; set; }
        public string AttachBy { get; set; }
        public string verifyBy { get; set; }
        public Guid tbl_LinkedAtmCardsID { get; set; }
    }

    public class MobileUsers
    {
        public string IdNo { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string Pin { get; set; }
        public Guid tbl_CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string Email { get; set; }
        public bool? Enabled { get; set; }
    }

    public class Login
    {
        public string MobileNo { get; set; }
        public int Pin { get; set; }
    }

    public class LoginInfo
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public MobileUsers User { get; set; }
    }

    public class TellerProductRepaymentsView
    {
        public CustomerAccountsView customerDetails { get; set; }
        public List<RepaymentView> Repayments { get; set; }
        public List<ProductsView> Products { get; set; }
        public List<CustomerBalances> CustomerBalances { get; set; }
        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public double Amount { get; set; }
        public Guid TransactionId { get; set; }
    }

    public class RepaymentView
    {
        public string CustomerNo { get; set; }
        public string Ledger { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Amount { get; set; }
        public double Section { get; set; }
        public int Rowid { get; set; }
        public Guid TransactionId { get; set; }
        public double LoanInt { get; set; }
        public double loan_Levy { get; set; }
        public double LoanIns { get; set; }
        public double LoanApp { get; set; }
        public double LoanPen { get; set; }
        public double LoanPrincipal { get; set; }
    }

    public class CustomerDetailsView
    {
        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        public bool Locked { get; set; }
        public string AccountRemarks { get; set; }
        public string AccountComments { get; set; }
        public string MemberType { get; set; }
        public string Branch { get; set; }
        public DateTime? DateClosed { get; set; }
        public string MobileNo { get; set; }
        [Display(Name = "Customer ID No")]
        public string CustomerIdNo { get; set; }
        public string EmploymentNo { get; set; }
        public string PinNo { get; set; }
        public string BranchName { get; set; }
        public string MemberTypeName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }
        public string EmployerCode { get; set; }
        public string EmployerName { get; set; }
        public int? SigningInstructions { get; set; }

    }

    public class AccountTypesView
    {
        public decimal? code { get; set; }
        public string act_code { get; set; }
        public string category { get; set; }
    }

    public class PostingJournalsHeaderViewModel
    {
        [Display(Name = "Module Id")]
        public string PostingDocid { get; set; }

        [Display(Name = "Reference No.")]
        public string PostingReference { get; set; }

        [Display(Name = "Journal Description")]
        [StringLength(50, MinimumLength = 10)]
        public string PostingDescription { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }

        [Display(Name = "Create Branch")]
        public string CreateBranch { get; set; }

        [Display(Name = "Create At")]
        public string CreateBranchName { get; set; }

        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        public string VerifiedBy { get; set; }

        [Display(Name = "Verified By")]
        public string VerifiedByName { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? VerifiedDate { get; set; }

        public string PostedBy { get; set; }
        [Display(Name = "Posted By")]
        public string PostedByName { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PostDate { get; set; }
        public decimal? PostingLevel { get; set; }
        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }
        public string AccountNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }
        public string Section { get; set; }
        public Guid JournalId { get; set; }
        /* Used by complete withdrawal*/
        public string FosaAccount { get; set; }

        [Display(Name = "Bank Account")]
        public string BankAccount { get; set; }

        public int PostingMode { get; set; }
        public string WithdrawalReasonCode { get; set; }
        [Display(Name = "Withdrawal Reason")]
        public string WithdrawalReasonName { get; set; }
        public string PaymentMethodName { get; set; }
        public string PayingAccount { get; set; }

        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        /* Eof Used by complete withdrawal*/

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalDebits { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalCredits { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal BatchDifference { get; set; }
        public string ModuleId { get; set; }
        public int? TotalEntries { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Branch")]
        public string CustomerBranchName { get; set; }
        public string CustomerSavingsAccountType { get; set; }
        public List<CustomerAccountsView> CustomerSavingsAccounts { get; set; }

    }
    public enum DenominationCountTypes
    {
        Count,
        Total
    }

    public class DenominationCounter
    {
        public string Docid { get; set; }
        public string ReferenceNo { get; set; }

        public DenominationCountTypes CountType { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_1000 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_500 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_200 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_100 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_50 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_40 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_20 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_10 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_5 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_1 { get; set; }
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_050 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_010 { get; set; }
        [Range(0, 1000000)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Integer")]
        public int C_005 { get; set; }

        // [Range(0, 1000000000)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_1000 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_500 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_200 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_100 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_50 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_40 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_20 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_10 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_5 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_1 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_050 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_010 { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(CustomValidation), "ValidateGreaterOrEqualToZero_Double")]
        public double X_005 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double TransactionAmount { get; set; }
        public double TotalAmount { get; set; }
        public string GlDebit { get; set; }
        public string GlCredit { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double GlDebitBalanceBefore { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]

        public double GlCreditBalanceBefore { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double GlDebitBalanceAfter { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double GlCreditBalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ChequeNo { get; set; }

        public string TrType { get; set; }
        public string Print_DescriptionLine1 { get; set; }
        public string Print_DescriptionLine2 { get; set; }

        // Close Day
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double CurrentBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Deposits { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Withdrawals { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Shortage { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Excess { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Cheques { get; set; }

        public string AmountInWords { get; set; }
    }

    public class TellerAccountView
    {
        public string TellerAccountNo { get; set; }
        public string AuthorisedBranch { get; set; }
        public string LoginCode { get; set; }
    }

    public class CustomerAccountsView
    {
        public int CustomerAccountsViewId { get; set; }
        public Guid tbl_CustomerAccountsID { get; set; }
        [Display(Name = "Customer No")]
        public string CustomerNo { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer ID No")]
        public string CustomerIdNo { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }
        [Display(Name = "Date Opened")]
        public DateTime OpenedDate { get; set; }
        [Display(Name = "Opened By")]
        public string OpenedBy { get; set; }
        [Display(Name = "Authorised Date")]
        public DateTime? AuthorisedDate { get; set; }
        [Display(Name = "Authorised By")]
        public string AuthorisedBy { get; set; }
        public bool Locked { get; set; }
        [Display(Name = "Account Remarks")]
        public string AccountRemarks { get; set; }
        [Display(Name = "Account Comments")]
        public string AccountComments { get; set; }
        [Display(Name = "Account Type")]
        public decimal AccountType { get; set; }
        public string AccountTypeName { get; set; }
        public DateTime? DateClosed { get; set; }
        public string ClosedBy { get; set; }
        public decimal? MinimumBalance { get; set; }
        public string GlMemSav { get; set; }
        public tbl_accounttypes AccountTypeSettings { get; set; }
        public string CustomerBranch { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool? Parent_Locked { get; set; }
        public string Parent_AccountRemarks { get; set; }
        public string Parent_AccountComments { get; set; }
        public string ReasonClosed { get; set; }
        // Display Purposes for Account Rejoining
        public List<PostingJournalsHeaderViewModel> ClosedAccountDetails { get; set; }

        public List<CustomerDetailsView> customerDetails { get; set; }
        public List<AccountTypesView> SavingsProducts { get; set; }
        public CustomerAccountsView()
        {
            customerDetails = new List<CustomerDetailsView>();
        }
    }

    public class TellerMobileDeposit
    {
        [Required(ErrorMessage = "CustomerNo  Required")]
        public String CustomerNo { get; set; }

        [Required(ErrorMessage = "Product Code Required")]
        public String ProductCode { get; set; }

        [Required(ErrorMessage = "Transaction Amount Required")]
        public Decimal TrxAmount { get; set; }

        [Required(ErrorMessage = "Teller Login Code Required")]
        public String TellerLoginCode { get; set; }

        public String DeviceInfo { get; set; }
    }

    public class MobileOperatorTrail
    {
        public String RecNo { get; set; }
        public String ReferenceNo { get; set; }
        public String Ledger { get; set; }
        public String CustomerNo { get; set; }
        public String AccountNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public String Description { get; set; }
        public decimal Amount { get; set; }
        public String DeviceInfo { get; set; }
        public String LoginCode { get; set; }
        public Guid OperatorTrailID { get; set; }
    }

}