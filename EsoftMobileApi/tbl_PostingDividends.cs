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
    using System.Collections.Generic;
    
    public partial class tbl_PostingDividends
    {
        public long Recno { get; set; }
        public string PostingDocid { get; set; }
        public string PostingReference { get; set; }
        public string PostingDescription { get; set; }
        public string CreatedBy { get; set; }
        public string CreateBranch { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public string PostedBy { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<decimal> PostingLevel { get; set; }
        public string ProductCode { get; set; }
        public string ShareCapitalCode { get; set; }
        public decimal InterestRate { get; set; }
        public decimal With_HoldingTax { get; set; }
        public string GlAccount { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> rate { get; set; }
        public Nullable<System.DateTime> Year_Ending { get; set; }
        public string CommissionCode { get; set; }
        public System.Guid tbl_PostingDividendsID { get; set; }
    }
}