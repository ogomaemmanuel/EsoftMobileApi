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
    
    public partial class tbl_PostingInvoices
    {
        public long Recno { get; set; }
        public string PostingDocid { get; set; }
        public string PostingReference { get; set; }
        public string SupplierAc { get; set; }
        public string InvoiceDescription { get; set; }
        public string InvoiceNumber { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal VAT_Amount { get; set; }
        public bool Charge_Wtax { get; set; }
        public string CreatedBy { get; set; }
        public string CreateBranch { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public string PostedBy { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<decimal> PostingLevel { get; set; }
        public Nullable<decimal> WtaxCharged { get; set; }
        public System.Guid tbl_PostingInvoicesID { get; set; }
    }
}
