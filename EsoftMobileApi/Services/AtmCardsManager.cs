using ESoft.Web.Services.Registry;
using EsoftMobileApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class AtmCardsManager
    {
        private readonly Esoft_WebEntities mainDb;
        private CustomerAccountsManager customerAccountsManager;

        public AtmCardsManager()
        {
            mainDb = new Esoft_WebEntities();
            customerAccountsManager = new CustomerAccountsManager();
        }

        public bool BlockAtmCard(Guid atmCardId, tbl_Customer customer)
        {
            bool blockingStatus = false;

            List<LinkedAtmCards> cards = new List<LinkedAtmCards>();

            cards = customerAccountsManager.GetLinkedAtmCards(customer.CustomerNo, cards);

            LinkedAtmCards foundCard = cards.Where(x => x.tbl_LinkedAtmCardsID == atmCardId).FirstOrDefault();

            if (foundCard != null)
            {
                string docid = "503F";

                //block it
                string updateLinkProcess = "INSERT INTO tbl_LinkProcess(AccountNo,CardNumber,Branch,ENABLED,TransactionDate,LoginId,Narration,DOCID) " +
                     " VALUES('" + foundCard.AccountNo + "','" + foundCard.CardNumber + "','" + foundCard.Branch +
                     "',5,GetDate(),'APP','Stopped ATM CARD :" + foundCard.CardNumber + "','" + docid + "'); ";

                updateLinkProcess += "UPDATE tbl_LinkedAtmCards SET ENABLED=0,VERIFY=0 where tbl_LinkedAtmCardsId='" + foundCard.tbl_LinkedAtmCardsID.ToString() + "';";

                try
                {
                    //DbDataReader reader = DbConnector.GetSqlReader(updateLinkProcess);

                    int success = this.mainDb.Database.ExecuteSqlCommand(updateLinkProcess);

                    if (success >= 1)
                    {
                        blockingStatus = true;
                    }
                }
                catch (Exception ex)
                {
                    //log the exception
                    blockingStatus = false;
                }

            }

            return blockingStatus;

        }
    }
}