using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EsoftMobileApi.Models;
using ESoft.Web.Services.Registry;

namespace EsoftMobileApi.Controllers
{
    [EnableCors(origins: "http://localhost:8100", headers: "*", methods: "*")]
    public class CustomerController : ApiController
    {
        Esoft_WebEntities mainDb;

        private CustomerAccountsManager customerAccountsManager;

        public CustomerController()
        {
            mainDb = new Esoft_WebEntities();
            customerAccountsManager = new CustomerAccountsManager();

        }

        [Route("customers/{id}"), HttpGet]
        public tbl_Customer GetCustomer(Guid id)
        {
            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);
            return customer;
        }

        [Route("customers/{id}/savings"), HttpGet]
        public List<AccountDetails> GetSavingBalances(Guid id)
        {
            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);
            List<AccountDetails> acccountDetails = customerAccountsManager.GetSavingsAccountBalances(customer.CustomerNo);
            return acccountDetails;
        }

        [Route("customers/{id}/shares"), HttpGet]
        public List<AccountDetails> GetShareBalances(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new List<AccountDetails>();
            }

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<AccountDetails> accountsDetails
                = customerAccountsManager.CustomerShareBalances(customer.CustomerNo);

            return accountsDetails;
        }

        [Route("customers/{id}/loans"), HttpGet]
        public List<AccountDetails> GetLoansBalances(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new List<AccountDetails>();
            }

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<AccountDetails> accountsDetails
                = customerAccountsManager.CustomerLoanBalances(customer.CustomerNo);

            return accountsDetails;
        }

        [Route("customers/{id}/savings-accounts"), HttpGet]
        public List<ProductsView> GetSavingsAccounts(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new List<ProductsView>();
            }

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<tbl_CustomerAccounts> accounts = customerAccountsManager.GetCustomerSavingsAccounts(customer.CustomerNo);
            List<tbl_accounttypes> accountTypes = customerAccountsManager.GetAccountTypes();

            List<ProductsView> savingsAccounts = (from account in accounts
                                                  join types in accountTypes on account.AccountType equals types.code
                                                  select new ProductsView
                                                  {
                                                      ProductCode = (types.act_code ?? string.Empty).Trim(),
                                                      ProductName = (types.category ?? string.Empty).Trim(),
                                                      AccountNo = (account.AccountNo ?? string.Empty).Trim(),
                                                      Balance = 0,
                                                  }).ToList();
            return savingsAccounts;
        }

        [Route("customers/{id}/loans-accounts"), HttpGet]
        public List<ProductsView> GetLoansAccounts(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new List<ProductsView>();
            }

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<ProductsView> products = customerAccountsManager.GetCustomerProducts(
                new List<ProductsView>(),
                customer.CustomerNo,
               new List<CustomerBalances>());

            products = products.Where(x => x.ProductType == "LOANS").ToList();

            return products;
        }

        [Route("customers/{id}/shares-accounts"), HttpGet]
        public List<ProductsView> GetSharesAccounts(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new List<ProductsView>();
            }

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<ProductsView> products = customerAccountsManager.GetCustomerProducts(
                new List<ProductsView>(),
                customer.CustomerNo,
               new List<CustomerBalances>());

            products = products.Where(x => x.ProductType == "INVESTMENTS").ToList();

            return products;
        }

        [Route("customers/{id}/savings-statement/{account}"), HttpGet]
        public List<Statement> GetSavingsStatement(Guid id, string account)
        {
            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            List<CustomerSavings> savings = customerAccountsManager.GetCustomerSavings(account)
                .OrderBy(x => x.TransactionDate)
                .ThenByDescending(x => x.TransactionDate)
                .Take(5)
                .ToList();

            List<Statement> statement = (from saving in savings
                                         select new Statement
                                         {
                                             ReferenceNo = saving.ReferenceNo,
                                             TransactionDate = saving.TransactionDate,
                                             Amount = (saving.DEBIT - saving.CREDIT) ?? 0
                                         }).ToList();

            return statement;
        }

        [Route("customers/{id}/loans-statement/{account}"), HttpGet]
        public List<Statement> GetLoansStatement(Guid id, string account)
        {
            List<Statement> statement = new List<Statement>();

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            DateTime startDate = new DateTime(1990, 1, 1);
            DateTime endDate = DateTime.Now;

            List<CustomerLoanStatementViewModel> statements = customerAccountsManager.GetSingleLoanStatement(
                new List<CustomerLoanStatementViewModel>(),
                customer.CustomerNo,
                account,
                startDate, endDate
                ).OrderBy(x => x.TransactionDate)
                .ThenByDescending(x => x.TransactionDate)
                .Take(5).ToList();

            statement = (from sstatement in statements
                         select new Statement
                         {
                             ReferenceNo = sstatement.ReferenceNo,
                             TransactionDate = sstatement.TransactionDate,
                             Amount = (sstatement.Debit - sstatement.Credit)
                         }).ToList();

            return statement;
        }

        [Route("customers/{id}/shares-statement/{account}"), HttpGet]
        public List<Statement> GetSharesStatement(Guid id, string account)
        {
            List<Statement> statement = new List<Statement>();

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            DateTime startDate = new DateTime(1990, 1, 1);
            DateTime endDate = DateTime.Now;

            List<CustomerInvestmentStatementViewModel> statements = customerAccountsManager.GetSingleInvestmentStatement(
                new List<CustomerInvestmentStatementViewModel>(),
                customer.CustomerNo,
                account,
                startDate, endDate
                ).OrderBy(x => x.TransactionDate)
                .ThenByDescending(x => x.TransactionDate)
                .Take(5).ToList();

            statement = (from sstatement in statements
                         select new Statement
                         {
                             ReferenceNo = sstatement.ReferenceNo,
                             TransactionDate = sstatement.TransactionDate,
                             Amount = (sstatement.Debit - sstatement.Credit)
                         }).ToList();

            return statement;
        }

        [Route("customers/{id}/atm-cards"), HttpGet]
        public List<LinkedAtmCards> GetAtmCards(Guid id)
        {
            List<LinkedAtmCards> atm_cards = new List<LinkedAtmCards>();

            tbl_Customer customer = customerAccountsManager.CustomerDetails(id);

            atm_cards = customerAccountsManager.GetLinkedAtmCards(customer.CustomerNo, atm_cards);

            return atm_cards;
        }

        [Route("customers/register"), HttpPost]
        public bool RegisterCustomer(MobileUsers user)
        {
            bool insertResult = false;

            insertResult = customerAccountsManager.RegisterMobileUser(user);

            return insertResult;
        }

        [Route("customers/login"), HttpGet]
        public LoginInfo RegisterCustomer([FromUri]Login login)
        {
            LoginInfo loginStatus = new LoginInfo();

            if (login != null)
            {
                loginStatus = customerAccountsManager.Login(login);
            }

            return loginStatus;
        }

        [Route("customers/activate"), HttpPut]
        public bool ActivateCustomer(string idNo, string customerNo, string pin)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(idNo) &&
                !string.IsNullOrWhiteSpace(customerNo) &&
                !string.IsNullOrWhiteSpace(pin)
                )
            {

                result = customerAccountsManager.ActivateMobileUser(idNo, customerNo, pin);
            }

            return result;
        }
    }
}
