using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class InvestmentsCodesManager
    {
        public IEnumerable<tbl_InvestmentCodes> InvestmentsCodes(Esoft_WebEntities db)
        {
            var returnList = new List<tbl_InvestmentCodes>();
            var data = db.tbl_InvestmentCodes.OrderBy(p => p.InvestmentCode).ToList();
            foreach (var item in data)
            {
                item.InvestmentName = item.InvestmentCode.Trim() + ": " + item.InvestmentName.Trim();
                returnList.Add(item);
            }
            return returnList.ToList();
        }

    }
}