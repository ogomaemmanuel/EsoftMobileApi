using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class SavingsProductManager
    {
        Esoft_WebEntities mainDb;

        public SavingsProductManager()
        {
            mainDb = new Esoft_WebEntities();
        }

        public List<tbl_accounttypes> SavingsAccountTypes(Esoft_WebEntities db)
        {
            var returnList = new List<tbl_accounttypes>();

            var data = db.tbl_accounttypes.OrderBy(p => p.act_code).ToList();
            foreach (var item in data)
            {
                item.category = item.act_code.Trim() + ": " + item.category.Trim();
                returnList.Add(item);
            }
            return returnList.ToList();
        }

    }
}