//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EsoftMobileApi
{
    using System;
    
    public partial class CommiteeAllowances_Result
    {
        public string PostingReference { get; set; }
        public string PostingDescription { get; set; }
        public string CreatedBy { get; set; }
        public string CreateBranch { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public string PostedBy { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string GlAccountNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string AccountNo { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public string CommitteeName { get; set; }
        public string AllowanceName { get; set; }
    }
}