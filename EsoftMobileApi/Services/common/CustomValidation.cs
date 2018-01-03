using ESoft.Web.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsoftMobileApi.Services.common
{
    public class CustomValidation
    {
        private static string glAccountMask = System.Configuration.ConfigurationManager.AppSettings["GlAccountMask"].ToString();

        public static bool ValidateGlAccount_(string value)
        {
            bool isValid = true;

            if (ValueConverters.IsStringEmpty(value) ||
                value.Trim().Length != glAccountMask.Trim().Length)
            {
                isValid = false;
            }

            //ToDo Confirm GlAccount Existence
            if (isValid && value.Trim().Length == glAccountMask.Trim().Length)
            {
                // check existence of account
            }
            return isValid;
        }
    }
}