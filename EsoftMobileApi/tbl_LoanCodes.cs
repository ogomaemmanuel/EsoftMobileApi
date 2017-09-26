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
    
    public partial class tbl_LoanCodes
    {
        public long recno { get; set; }
        public string LoanCode { get; set; }
        public string LoanName { get; set; }
        public Nullable<int> LoanProductType { get; set; }
        public string AppraisalCode { get; set; }
        public Nullable<decimal> AppraisalFactor { get; set; }
        public Nullable<decimal> InterestRate { get; set; }
        public Nullable<decimal> RepaymentPeriod { get; set; }
        public Nullable<decimal> GracePeriod { get; set; }
        public Nullable<decimal> MinimumLoan { get; set; }
        public Nullable<decimal> MaximumLoan { get; set; }
        public Nullable<decimal> MinimumShares { get; set; }
        public Nullable<decimal> Ignore_Cash_shares { get; set; }
        public Nullable<int> MinimumGuarantors { get; set; }
        public Nullable<int> MaximumGuarantors { get; set; }
        public Nullable<int> MinMembershipDays { get; set; }
        public Nullable<int> Maximum_Members_Age { get; set; }
        public string InterestType { get; set; }
        public Nullable<bool> BlockPosting { get; set; }
        public string PrincipalAccount { get; set; }
        public string InterestIncomeAccount { get; set; }
        public string InterestAccruedAccount { get; set; }
        public string InterestControlAccount { get; set; }
        public string PenaltyIncomeAccount { get; set; }
        public string PenaltyAccruedAccount { get; set; }
        public string PenaltyControlAccount { get; set; }
        public string InsuranceIncomeAccount { get; set; }
        public string InsuranceAccruedAccount { get; set; }
        public string InsuranceControlAccount { get; set; }
        public string AppraisalFeeIncomeAccount { get; set; }
        public string AppraisalFeeAccruedAccount { get; set; }
        public string AppraisalFeeControlAccount { get; set; }
        public string PostLoanTo { get; set; }
        public Nullable<bool> DeductInterestUpfront { get; set; }
        public Nullable<bool> ClearBalanceOnDisbursement { get; set; }
        public Nullable<bool> DoNotChargeMonthlyInterest { get; set; }
        public string BoostShareCommissionCode { get; set; }
        public string BoostShareCode { get; set; }
        public string ClearanceCommissionCode { get; set; }
        public string ClearanceInterestCode { get; set; }
        public string Commission_On_Early_Disbursement { get; set; }
        public Nullable<decimal> LedgerFee { get; set; }
        public string LedgerFee_Account { get; set; }
        public string InterestRateType { get; set; }
        public Nullable<bool> SOrder_contains_all_Deductions { get; set; }
        public Nullable<bool> AuditLoan_Before_Disbursment { get; set; }
        public Nullable<bool> Charge_Loan_Insurance { get; set; }
        public Nullable<bool> Recover_Insurance_on_Disburse { get; set; }
        public Nullable<decimal> FixeRetention { get; set; }
        public Nullable<int> LoanProcess { get; set; }
        public Nullable<bool> Do_Not_consider_income_on_Appraisal { get; set; }
        public Nullable<int> Maximum_Loans_to_Guarantee { get; set; }
        public Nullable<bool> load_First_Month_Interest_On_Issue { get; set; }
        public Nullable<decimal> InterestRate_Due_Sline { get; set; }
        public Nullable<decimal> CashPayment_Charge { get; set; }
        public Nullable<decimal> CashPayment_Charge_Rate { get; set; }
        public string CashPayment_GlAccount { get; set; }
        public string BoostShareCapital { get; set; }
        public Nullable<bool> Check_Guarantor_Validity { get; set; }
        public System.Guid tbl_LoanCodesID { get; set; }
        public Nullable<decimal> PenaltyChargeRate { get; set; }
    }
}
