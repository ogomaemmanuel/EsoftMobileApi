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
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public int Pin { get; set; }
        public Guid tbl_CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string Email { get; set; }
        public bool? Enabled { get; set; }
    }


}