using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services
{
    public class CompanyManager
    {
        private Esoft_WebEntities mainDb;

        public CompanyManager()
        {
            mainDb = new Esoft_WebEntities();
        }

        public Company GetCompanyDetails()
        {
            return mainDb.Companies.FirstOrDefault();
        }
    }
}