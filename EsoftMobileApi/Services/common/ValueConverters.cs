//using ESoft.Web.Models;
using System;
using System.Globalization;
using System.Linq;
using EsoftMobileApi;

namespace ESoft.Web.Services.Common
{
    public class ValueConverters
    {
        public ValueConverters()
        {
            //
            // Add constructor logic here
            //
        }

        public static Guid StringtoGuid(string stringtoCovert)
        {
            stringtoCovert = string.IsNullOrWhiteSpace(stringtoCovert) ? Guid.Empty.ToString() : stringtoCovert;

            Guid _returnType = Guid.Empty;
            _returnType = Guid.TryParse(stringtoCovert.ToString(), out _returnType) ?
                Guid.Parse(stringtoCovert.ToString()) : Guid.Empty;
            return _returnType;
        }
        public static double StringtoDouble(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            double _returnType = 0.00;
            _returnType = double.TryParse(stringtoCovert.ToString(), out _returnType) ? double.Parse(stringtoCovert.ToString()) : 0;
            return _returnType;
        }
        public static decimal StringtoDecimal(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            decimal _returnType = 0.00M;
            _returnType = decimal.TryParse(stringtoCovert.ToString(), out _returnType) ? decimal.Parse(stringtoCovert.ToString()) : 0;
            return _returnType;
        }
        public static long StringtoLong(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            long _returnType = 0;
            _returnType = long.TryParse(stringtoCovert.ToString(), out _returnType) ? long.Parse(stringtoCovert.ToString()) : 0;
            return _returnType;
        }

        public static int StringtoInteger(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            if (stringtoCovert.Contains(".")) stringtoCovert = stringtoCovert.Substring(0, stringtoCovert.IndexOf('.'));
            int _returnType = 0;
            _returnType = int.TryParse(stringtoCovert.ToString(), out _returnType) ? int.Parse(stringtoCovert.ToString()) : 0;
            return _returnType;
        }

        public static bool StringtoBool(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            bool _returnType = false;
            _returnType = bool.TryParse(stringtoCovert.ToString(), out _returnType) ? bool.Parse(stringtoCovert.ToString()) : false;
            return _returnType;
        }

        [System.Obsolete("Use str.ConvertNullToEmptyString() instead")]
        public static String ConvertNullToEmptyString(string str)
        {
            return (string.IsNullOrEmpty(str) ? string.Empty : str).Trim();
        }

        public static double ConvertNullToDouble(double? amount)
        {
            return (double)(amount == null ? 0 : amount);
        }
        public static decimal ConvertNullToDecimal(decimal? amount)
        {
            return (decimal)(amount == null ? 0 : amount);
        }
        public static decimal ConvertNullToDecimal(decimal amount)
        {
            return amount;
        }

        public static int? ConvertNullToInteger(int? amount)
        {
            return (int)(amount == null ? 0 : amount);
        }
        public static bool ConvertNullToBool(bool? boolItem)
        {
            return (bool)(boolItem == null ? false : boolItem);
        }
        public static string ConvertBoolToSqlString(bool? boolItem)
        {
            boolItem = boolItem == null ? false : boolItem;
            return boolItem == true ? "1" : "0";
        }

        [System.Obsolete("Use dateValue.ConvertNullToDatetime() instead")]
        public static DateTime ConvertNullToDatetime(DateTime? date)
        {
            return (DateTime)((date == null || (date == DateTime.MinValue)) ? new DateTime(1900, 01, 01) : date);
        }
        public static DateTime StringToDate(string stringtoCovert)
        {
            DateTime date_new;
            DateTime.TryParse(stringtoCovert, out date_new);
            if (date_new.Year == 0001)
                date_new = new DateTime(1900, 01, 01);

            return date_new;
        }
        public static string FormatStringAsDate(string stringtoCovert, bool time = false)
        {
            DateTime date_new;
            DateTime.TryParse(stringtoCovert, out date_new);
            if (date_new.Year == 0001)
                date_new = new DateTime(1900, 01, 01);
            if (time)
            {
                return date_new.ToString("dd/MM/yyyy hh:mm:ss ");
            }
            else
            {
                return date_new.ToString("dd/MM/yyyy");
            }
        }
        public static string SqlFormatStringAsDate(string stringtoCovert, bool time = false)
        {
            DateTime date_new;
            DateTime.TryParse(stringtoCovert, out date_new);
            if (date_new.Year == 0001)
                date_new = new DateTime(1900, 01, 01);
            if (time)
            {
                return date_new.ToString("MM/dd/yyyy hh:mm:ss ");
            }
            else
            {
                return date_new.ToString("MM/dd/yyyy");
            }
        }


        public static string FormatDateAsSQlParameter(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss");
        }


        public static DateTime DayEndToday(DateTime? date)
        {
            if (date == null)
            {
                date = ConvertNullToDatetime(date.ToString());
            }

            DateTime dt = Convert.ToDateTime(date);
            return dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        public static DateTime LastDayOfLastMonth(DateTime date)
        {
            DateTime lastDayLastMonth = new DateTime(date.Year, date.Month, 1);
            lastDayLastMonth = lastDayLastMonth.AddDays(-1);

            return lastDayLastMonth;
        }

        public static DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static int DaysInMonth(DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(DateTime? value)
        {
            DateTime dt = ConvertNullToDatetime(value);
            return new DateTime(dt.Year, dt.Month, DaysInMonth(dt));
        }

        public static string FormatSqlDate(DateTime? date, bool endDate = false)
        {
            date = ConvertNullToDatetime(date);
            return FormatSqlDate((DateTime)date, endDate);
        }
        public static string FormatSqlDate(DateTime date, bool endDate = false)
        {
            if (date.Year == 1) date = new DateTime(1900, 01, 01);
            if (date == null)
            {
                date = ConvertNullToDatetime(date.ToString());
            }
            int x = date.Month;
            if (endDate)
            { //11:59:59 PM
                return date.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            { // today Midnight ("00:00:00 AM"
                return date.Date.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        public static DateTime ConvertNullToDatetime(string date)
        {
            DateTime date_new;
            DateTime.TryParse(date, out date_new);
            if (date_new.Year == 0001 && !ValueConverters.IsStringEmpty(date))
            {
                //var culture = System.Globalization.CultureInfo.CurrentCulture;
                var culture = System.Globalization.CultureInfo.InvariantCulture;
                date_new = DateTime.ParseExact(date, "d/M/yyyy", culture);
            }
            if (date_new.Year == 0001)
                date_new = new DateTime(1900, 01, 01);

            return date_new;
        }

        public static string FormatValues(double value)
        {
            return (value.ToString("N2")); // 1,234,567.00
        }

        public static string PADLeft(int padLength, string expression, char padCharacter)
        {
            string result = expression;

            if (ConvertNullToEmptyString(expression) != string.Empty)
            {
                result = format_sql_string(result); //27/03/2017
                result = expression.Trim().PadLeft(padLength, padCharacter);
            }
            return result;
        }

        public static string Right(string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }
        public static string Left(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim();
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static bool IsStringEmpty(string expression)
        {
            return string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression);
        }

        public static bool ContainsAny(string haystack, string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }


        public static bool ValidatePhoneNumber(string phoneNo)
        {
            bool validNumber;
            validNumber = false;
            phoneNo = ValueConverters.ConvertNullToEmptyString(phoneNo);
            if (phoneNo.Trim().Length == 10)
            {
                // check if user entered a duplicated number throughout
                validNumber = true;
                for (int i = 0; i < 9; i++)
                {
                    string j = PADLeft(6, i.ToString(), (char)i);
                    if (j == Right(phoneNo, 6))
                    {
                        validNumber = false;
                        break;
                    }
                }
            }
            return validNumber;
        }

        /// <summary>
        /// Use before passing any string to SQL command
        /// Prevents SQL INJECTION ATTACKS
        /// </summary>
        /// <param name="_mstring"></param>
        /// <returns></returns>
        [System.Obsolete("Use string.Format_Sql_String() instead")]
        public static string format_sql_string(string _mstring)
        {
            _mstring = ConvertNullToEmptyString(_mstring);
            _mstring = _mstring.Replace("'", "");
            _mstring = _mstring.Replace("[", "");
            _mstring = _mstring.Replace("]", "");
            _mstring = _mstring.Replace(";", "");
            _mstring = _mstring.Replace("--", "");
            _mstring = _mstring.Replace("/*", "");
            _mstring = _mstring.Replace("*/", "");
            _mstring = _mstring.Replace("xp_", "");
            return _mstring.Trim();
        }

        public static string RandomString(int length)
        {
            return (Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length));
        }

        public static double Round05(double amount)
        {
            return Math.Ceiling(amount * 2) / 2;
        }
        public static decimal Round05(decimal amount)
        {
            return Math.Ceiling(amount * 2) / 2;
        }
        public static decimal Round10(decimal amount)
        {
            return Math.Floor((Math.Ceiling(amount / 10)) * 10);
        }
        public static decimal Round100(decimal amount)
        {
            return Math.Floor((Math.Ceiling(amount / 100)) * 100);
        }
        public static decimal Roundn(decimal _figure)
        {
            decimal figure1 = _figure - Math.Floor(_figure);

            if (figure1 == 0) return _figure;

            string outPut = "0";
            int decPart = 0;
            if (_figure.ToString().Split('.').Length == 2)
            {
                outPut = _figure.ToString().Split('.')[1].Substring(0, _figure.ToString().Split('.')[1].Length);
            }
            if (outPut == "0")
            {
                decPart = 0;
            }
            else
            {
                if (outPut == "5")
                {
                    decPart = 5;
                }
                else
                {
                    decPart = StringtoInteger(outPut);
                    if (decPart >= 1 && decPart <= 4)
                    {
                        decPart = 0;
                    }
                    else
                    {
                        decPart = StringtoInteger(outPut);
                        if (decPart >= 6 && decPart <= 9)
                        {
                            decPart = 5;
                        }
                    }
                }
            }

            outPut = Math.Floor(_figure).ToString().Trim() + "." + decPart.ToString().Trim();
            return (decimal)StringtoDouble(outPut);

        }

        public static decimal SalaryAmounttoDecimal(string stringtoCovert)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            decimal _returnType = 0.00M;
            _returnType = decimal.TryParse(stringtoCovert.ToString(), out _returnType) ? decimal.Parse(stringtoCovert.ToString()) : 0;
            if (!stringtoCovert.Contains("."))
            {
                _returnType = _returnType / 100;
            }
            return _returnType;
        }

        public static decimal CheckOffAmounttoDecimal(string stringtoCovert, bool divideBy100 = false)
        {
            stringtoCovert = ConvertNullToEmptyString(stringtoCovert);
            decimal _returnType = 0.00M;
            _returnType = decimal.TryParse(stringtoCovert.ToString(), out _returnType) ? decimal.Parse(stringtoCovert.ToString()) : 0;
            if (divideBy100)
            {
                _returnType = _returnType / 100;
            }
            return _returnType;
        }

        public static decimal ConvertDoubleToDecimal(double p)
        {
            p = ConvertNullToDouble(p);
            return Convert.ToDecimal(p);
        }
        public static double ConvertDecimaltoDouble(decimal? p)
        {
            p = ConvertNullToDecimal(p);
            return Convert.ToDouble(p);
        }

        public static string ChrTran(string toBeCleaned, string changeThese, string intoThese)
        {
            string curRepl = string.Empty;
            for (int lnI = 0; lnI < changeThese.Length; lnI++)
            {
                if (lnI < intoThese.Length)
                    curRepl = intoThese.Substring(lnI, 1);
                else
                    curRepl = String.Empty;

                toBeCleaned = toBeCleaned.Replace(changeThese.Substring(lnI, 1), curRepl);
            }
            return toBeCleaned;
        }
        public static double AmortisedLoanRepayment(double principalAmount, double repayPeriod, double interestRatePerYear)
        {
            double a, b, x;
            double monthlyPayment;
            a = (1 + interestRatePerYear / 1200);
            b = repayPeriod;
            x = Math.Pow(a, b);
            x = 1 / x;
            x = 1 - x;
            monthlyPayment = (principalAmount) * (interestRatePerYear / 1200) / x;
            return (monthlyPayment);
        }

        public static string FormatAtmCardNumber(string cardNumber)
        {
            cardNumber = ConvertNullToEmptyString(cardNumber);
            if (cardNumber.Length == 16)
            {
                cardNumber = cardNumber.Substring(0, 4) + '-' + cardNumber.Substring(4, 4) + '-' + cardNumber.Substring(8, 4) + '-' + cardNumber.Substring(12, 4);
            }
            return cardNumber;
        }

        //amount converter



        //public static DateTime GetChequeDays(int? numberOfDays, DateTime dateToday, Esoft_WebEntities mainDb)
        //{
        //    if (ValueConverters.ConvertNullToInteger(numberOfDays) == 0)
        //    {
        //        return dateToday;
        //    }

        //    var publicHolidays = mainDb.PublicHolidays;

        //    DateTime nextday;
        //    int x = 1;
        //    int? p = 0;
        //    while (true)
        //    {
        //        nextday = dateToday.AddDays(x);
        //        var singleDay = publicHolidays.Where(y => y.HolidayDate.Day == nextday.Day && y.HolidayDate.Month == nextday.Month).ToList();
        //        if (singleDay == null || singleDay.Count == 0)
        //        {
        //            if (nextday.DayOfWeek.In(DayOfWeek.Sunday, DayOfWeek.Saturday))
        //            {
        //            }
        //            else
        //            {
        //                p = p + 1;
        //            }
        //        }
        //        if (p == numberOfDays)
        //        {
        //            break;
        //        }
        //        x = x + 1;
        //    }

        //    return dateToday.AddDays((double)x);
        //}

        public static string MakeValidFileName(string name)
        {
            try
            {
                string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
                string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
                name = System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
                name = name.Replace(" ", "");
                name = name.Replace(",", "");
            }
            catch (Exception ex)
            {

                name = "file_" + RandomString(10);
            }
            return name;
        }

    }
}
