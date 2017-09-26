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



}