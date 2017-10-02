using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EsoftMobileApi.Models;
using EsoftMobileApi.Services;
using ESoft.Web.Services.Registry;

namespace EsoftMobileApi.Controllers
{
    [EnableCors(origins: "http://localhost:8100", headers: "*", methods: "*")]
    public class AtmCardsController : ApiController
    {
        AtmCardsManager atmCardsManager;
        private CustomerAccountsManager customerAccountsManager;

        public AtmCardsController()
        {
            atmCardsManager = new AtmCardsManager();
            customerAccountsManager = new CustomerAccountsManager();
        }

        [Route("AtmCards/block"), HttpPut]
        public bool block(Guid id, Guid ?customerId)
        {
            bool result = false;

            if (string.IsNullOrEmpty(id.ToString()))
            {
                tbl_Customer customer = customerAccountsManager.CustomerDetails(customerId);

                result = atmCardsManager.BlockAtmCard(id, customer);
            }

            return result;
        }
    }
}