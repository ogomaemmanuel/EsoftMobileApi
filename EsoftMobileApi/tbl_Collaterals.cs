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
    
    public partial class tbl_Collaterals
    {
        public long recno { get; set; }
        public string CustomerNo { get; set; }
        public string SourceDocid { get; set; }
        public string ReferenceNo { get; set; }
        public System.DateTime DateCaptured { get; set; }
        public string doc_type { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<System.DateTime> ValuationDate { get; set; }
        public Nullable<decimal> ValuationAmount { get; set; }
        public string PlacedBy { get; set; }
        public decimal DocValue { get; set; }
        public string ProductCode { get; set; }
        public Nullable<System.DateTime> DateIssued { get; set; }
        public Nullable<decimal> AmountIssued { get; set; }
        public Nullable<System.DateTime> DateReleased { get; set; }
        public Nullable<int> Status { get; set; }
        public string LoanReferenceNo { get; set; }
        public Nullable<decimal> MarketValue { get; set; }
        public Nullable<decimal> MortgateValue { get; set; }
        public Nullable<decimal> ForcedValue { get; set; }
        public string OwnerName { get; set; }
        public string Nature { get; set; }
        public string InsuranceComapny { get; set; }
        public Nullable<System.DateTime> DateInsured { get; set; }
        public string Tracker { get; set; }
        public Nullable<System.DateTime> InsuranceExpiry { get; set; }
        public Nullable<decimal> FullyCharged { get; set; }
        public string remarks { get; set; }
        public System.Guid tbl_CollateralsID { get; set; }
    }
}
