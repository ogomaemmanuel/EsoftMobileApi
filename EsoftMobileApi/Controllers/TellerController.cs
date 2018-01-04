using ESoft.Web.Services.Common;
using EsoftMobileApi.Models;
using EsoftMobileApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EsoftMobileApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TellerController : ApiController
    {
        Esoft_WebEntities mainDb;
        TellerManager tellerManager;
        string customerNoMask = System.Configuration.ConfigurationManager.AppSettings["CustomerNoMask"].ToString();

        public TellerController()
        {
            mainDb = new Esoft_WebEntities();
            tellerManager = new TellerManager(new ModelStateWrapper(this.ModelState));
        }

        [Route("tellers/deposit"), HttpPost]
        public HttpResponseMessage TellerDeposit(TellerMobileDeposit tellerDeposit)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            TellerProductRepaymentsView productRepayments = new TellerProductRepaymentsView();
            TellerProductRepaymentsView custDetails = new TellerProductRepaymentsView();

            custDetails.CustomerNo = ValueConverters.PADLeft(Int32.Parse(customerNoMask), tellerDeposit.CustomerNo, '0');
            List<RepaymentView> repayments = tellerManager.ReadRepayment(tellerDeposit);

            if (!tellerManager.PostProductRepayment(custDetails, repayments, tellerDeposit.TellerLoginCode, tellerDeposit.DeviceInfo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Transaction posted successfully");
        }
    }
}
