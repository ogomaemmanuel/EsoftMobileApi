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
    
    public partial class tbl_SavingsInterest
    {
        public long recno { get; set; }
        public string CustomerNo { get; set; }
        public string AccountNo { get; set; }
        public int Period { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal LeastBalance { get; set; }
        public decimal InterestEarned { get; set; }
    }
}
