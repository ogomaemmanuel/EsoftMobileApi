using EsoftMobileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class UserAdministrationManager
    {
        private Esoft_WebEntities mainDb = new Esoft_WebEntities();

        public List<TellerAccountView> TellerAccountDetails(string userID)
        {
            List<TellerAccountView> tellerAccount = new List<TellerAccountView>();
            var teller = mainDb.tbl_TellerAccounts.FirstOrDefault(x => x.LoginCode == userID);
            if (teller != null)
            {
                tellerAccount.Add(new TellerAccountView
                {
                    LoginCode = teller.LoginCode,
                    TellerAccountNo = teller.TellerAccountNo,
                    AuthorisedBranch = teller.AuthorisedBranch
                });
            }
            return tellerAccount;
        }
    }
}