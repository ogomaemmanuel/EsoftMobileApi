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
    
    public partial class reco_cashbook_report_Result
    {
        public long Recno { get; set; }
        public string GlAccountNo { get; set; }
        public System.DateTime statementdate { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
        public string ChequeNo { get; set; }
        public string capturedby { get; set; }
        public System.DateTime captureddate { get; set; }
        public Nullable<System.DateTime> ReconciledDate { get; set; }
        public Nullable<bool> Reconciled { get; set; }
        public string ReconciledBy { get; set; }
        public string capturedbyname { get; set; }
        public Nullable<System.DateTime> ReconciliationPeriod { get; set; }
        public Nullable<bool> Reconciled1 { get; set; }
        public System.DateTime ImportedPeriod { get; set; }
    }
}