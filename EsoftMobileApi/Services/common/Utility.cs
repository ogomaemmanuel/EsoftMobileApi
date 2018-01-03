using ESoft.Web.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EsoftMobileApi.Services.common
{
    public class Utility
    {
        private static string logDirectory = System.Configuration.ConfigurationManager.AppSettings["Error_Log_Directory"].ToString();

        public Utility()
        {

        }
        public static void WriteErrorLog(string source, ref Exception e)
        {
            try
            {
                CheckLogDirectory(logDirectory + @"\ErrorLogs\");
                System.IO.TextWriter ErrHan = new System.IO.StreamWriter(logDirectory + @"\ErrorLogs\" + String.Format("{0:dd MMM yyyy}", DateTime.Now) + ".Log", true);
                ErrHan.WriteLine(source);
                ErrHan.WriteLine(DateTime.Now.ToString());
                ErrHan.WriteLine(e.Message);
                ErrHan.WriteLine(e.StackTrace);
                ErrHan.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                ErrHan.WriteLine("\n\n");
                ErrHan.Flush();
                ErrHan.Close();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }

        public static void WriteErrorLog(ref Exception e)
        {

            CheckLogDirectory(logDirectory + @"\ErrorLogs\");
            try
            {
                System.IO.TextWriter ErrHan = new System.IO.StreamWriter(logDirectory + @"\ErrorLogs\" + String.Format("{0:dd MMM yyyy}", DateTime.Now) + ".Log", true);
                ErrHan.WriteLine(DateTime.Now.ToString());
                ErrHan.WriteLine(e.Message);
                ErrHan.WriteLine(e.StackTrace);
                ErrHan.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                ErrHan.WriteLine("\n\n");
                ErrHan.Flush();
                ErrHan.Close();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }

        private static void CheckLogDirectory(string filePath)
        {
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
        }

        public static void WriteErrorLog(String e)
        {

            try
            {
                CheckLogDirectory(logDirectory + @"\ErrorLogs\");
                System.IO.TextWriter ErrHan = new System.IO.StreamWriter(logDirectory + @"\ErrorLogs\" + String.Format("{0:dd MMM yyyy}", DateTime.Now) + ".Log", true);
                ErrHan.WriteLine(DateTime.Now.ToString());
                ErrHan.WriteLine(e);
                ErrHan.WriteLine("\r\n");
                ErrHan.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                ErrHan.WriteLine("\n\n");
                ErrHan.Flush();
                ErrHan.Close();
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
        }
        public static void WriteErrorLog(String string1, string string2)
        {
            WriteErrorLog(string1.Trim() + " " + string2.Trim());
        }

        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static int CalculateAgeInYears(DateTime? birthDate)
        {
            DateTime now = DateTime.Now;
            DateTime bdate = ValueConverters.ConvertNullToDatetime(birthDate);
            int age = now.Year - bdate.Year;

            if (now.Month < bdate.Month || (now.Month == bdate.Month && now.Day < bdate.Day))
                age--;

            return age;
        }
    }
}