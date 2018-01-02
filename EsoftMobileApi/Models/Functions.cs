
using ESoft.Web.Services.Common;
using Newtonsoft.Json;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace EsoftMobileApi.Models
{

    public static class Functions
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            var table = new DataTable();

            int i = 0;
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                table.Columns.Add(new DataColumn(prop.Name, prop.PropertyType));
                ++i;
            }

            foreach (var item in source)
            {
                var values = new object[i];
                i = 0;
                foreach (var prop in props)
                    values[i++] = prop.GetValue(item);
                table.Rows.Add(values);
            }

            return table;
        }
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {
            List<IDataRecord> list = data.Cast<IDataRecord>().ToList();

            PropertyDescriptorCollection props = null;
            DataTable table = new DataTable();
            if (list != null && list.Count > 0)
            {
                props = TypeDescriptor.GetProperties(list[0]);
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
            if (props != null)
            {
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item) ?? DBNull.Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }
        public static string GetToken()
        {
            var accessToken = "";
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = null;// new Uri(GlobalSettings.Service);
                    var values = new List<KeyValuePair<string, string>>();
                    values.Add(new KeyValuePair<string, string>("grant_type", "password"));

                    values.Add(new KeyValuePair<string, string>("username", "parking@nairobi.go.ke"));
                    //values.Add(new KeyValuePair<string, string>("username", "teller1.branch1@agency.com"));
                    values.Add(new KeyValuePair<string, string>("password", "p@ssw0rd"));
                    var content = new FormUrlEncodedContent(values.ToArray());
                    var result = client.PostAsync("JambopayServices/Token", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                    accessToken = data["access_token"];
                }
            }
            catch (Exception)
            {

                throw;
            }
            return accessToken;
        }
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            DataTable table = new DataTable();
            try
            {
                PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));

                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteErrorLog("ToDataTable", ref ex);
            }
            return table;
        }

        //public static void ExportToExcel<T>(List<T> listData, string fileName)
        //{
        //    try
        //    {
        //        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //        PropertyDescriptor prop;
        //        List<int> guidsColumns = new List<int>();
        //        for (int i = 0; i < properties.Count; i++)
        //        {
        //            prop = properties[i];
        //            string dataType = prop.PropertyType.ToString();
        //            if (dataType.ToUpper().Contains("GUID"))
        //            {
        //                guidsColumns.Add(Convert.ToChar(i + 1));
        //            }
        //        }

        //        fileName = ValueConverters.MakeValidFileName(fileName);
        //        ExcelPackage excel = new ExcelPackage(); //install-package epplus
        //        var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

        //        workSheet.Cells[1, 1].LoadFromCollection(listData, true);
        //        workSheet.Cells.AutoFitColumns();
        //        if (guidsColumns.Count != 0)
        //        {
        //            foreach (var item in guidsColumns)
        //            {
        //                workSheet.DeleteColumn(item);
        //            }
        //        }

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            System.Web.HttpContext.Current.Response.ClearContent();
        //            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + ".xlsx");
        //            excel.SaveAs(memoryStream);
        //            memoryStream.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
        //            System.Web.HttpContext.Current.Response.Flush();
        //            System.Web.HttpContext.Current.Response.End();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.WriteErrorLog("ExportToExcel_" + fileName.Trim(), ref ex);
        //    }

        //}

        //public static void ExportToExcel(DbDataReader reader, string fileName)
        //{
        //    try
        //    {
        //        ////integer (not really needed unless you need to round numbers, Excel with use default cell properties)
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "0";
        //        ////integer without displaying the number 0 in the cell
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "#";
        //        ////number with 1 decimal place
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "0.0";
        //        ////number with 2 decimal places
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "0.00";
        //        ////number with 2 decimal places and thousand separator
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "#,##0.00";
        //        ////number with 2 decimal places and thousand separator and money symbol
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "€#,##0.00";
        //        ////percentage (1 = 100%, 0.01 = 1%)
        //        //ws.Cells["A1:A25"].Style.Numberformat.Format = "0%";
        //        ////default DateTime pattern
        //        //worksheet.Cells["A1:A25"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
        //        ////custom DateTime pattern
        //        //worksheet.Cells["A1:A25"].Style.Numberformat.Format = "dd-MM-yyyy HH:mm";

        //        Dictionary<int, string> fieldTypes = new Dictionary<int, string>();
        //        if (reader.HasRows)
        //        {
        //            for (int i = 0; i < reader.FieldCount; i++)
        //            {
        //                var columnName = reader.GetName(i);
        //                var dotNetType = reader.GetFieldType(i).ToString();
        //                fieldTypes.Add(i, dotNetType.ToString());
        //            }
        //        }

        //        fileName = ValueConverters.MakeValidFileName(fileName);
        //        ExcelPackage excel = new ExcelPackage(); //install-package epplus
        //        var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

        //        workSheet.Cells[1, 1].LoadFromDataReader(reader, true);

        //        foreach (var item in fieldTypes)
        //        {
        //            if (item.Value.ToUpper().Contains("DECIMAL") || item.Value.ToUpper().Contains("DOUBLE"))
        //            {
        //                workSheet.Column(item.Key).Style.Numberformat.Format = "#,##0.00";
        //            }
        //            if (item.Value.ToUpper().Contains("DATE"))
        //            {
        //                workSheet.Column(item.Key).Style.Numberformat.Format = "dd-MM-yyyy HH:mm";
        //            }
        //        }

        //        workSheet.Cells.AutoFitColumns();

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            System.Web.HttpContext.Current.Response.ClearContent();
        //            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + ".xlsx");
        //            excel.SaveAs(memoryStream);
        //            memoryStream.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
        //            System.Web.HttpContext.Current.Response.Flush();
        //            System.Web.HttpContext.Current.Response.End();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.WriteErrorLog("ExportToExcel_" + fileName.Trim(), ref ex);
        //    }

        //}
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);

            if (dr != null)
            {
                if (!((System.Data.Common.DbDataReader)dr).HasRows)
                {
                    return list;
                }
                var dataReader_fieldNames = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i).ToUpper().Trim()).ToArray();

                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (dataReader_fieldNames.Contains(prop.Name.ToUpper().Trim()))
                        {
                            if (!object.Equals(dr[prop.Name], DBNull.Value))
                            {
                                if (prop.PropertyType.Name.ToLower() == "double")
                                {
                                    prop.SetValue(obj, ValueConverters.StringtoDouble(dr[prop.Name].ToString()), null);
                                }
                                else
                                {
                                    prop.SetValue(obj, dr[prop.Name], null);
                                }
                            }
                        }
                    }
                    list.Add(obj);
                }
            }
            if (list == null)
                list = new List<T>();

            return list;
        }
        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        public static DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        public class ExcelResult<Model> : ActionResult
        {
            string _fileName;
            string _viewPath;
            Model _model;
            ControllerContext _context;

            public ExcelResult(ControllerContext context, string viewPath, string fileName, Model model)
            {
                this._context = context; this._fileName = fileName;
                this._viewPath = viewPath;
                this._model = model;
            }
            protected string RenderViewToString()
            {
                using (var writer = new StringWriter())
                {
                    var view = new WebFormView(_context, _viewPath);
                    var vdd = new ViewDataDictionary<Model>(_model);
                    var viewCxt = new ViewContext(_context, view, vdd, new TempDataDictionary(), writer);
                    viewCxt.View.Render(viewCxt, writer);
                    return writer.ToString();
                }
            }
            void WriteFile(string content)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                context.Response.AddHeader("content-disposition", "attachment;filename=\"" + _fileName + "\"");
                context.Response.Charset = "";
                context.Response.ContentType = "application/ms-excel";
                context.Response.Write(content);
                context.Response.End();
            }

            public override void ExecuteResult(ControllerContext context)
            {
                string content = this.RenderViewToString();
                this.WriteFile(content);
            }
        }





        public static System.Data.Common.DbDataReader IReader { get; set; }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            return ascending
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);
        }


        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        //public static IQueryable<TSource> WhereClause<TSource>(this IQueryable<TSource> query, List<DataTableBoundColumns> datatableColumns, string searchTerm)
        //{
        //    if (datatableColumns == null || datatableColumns.Count() == 0 || string.IsNullOrWhiteSpace(searchTerm))
        //        return query;

        //    var firstColumn = datatableColumns.FirstOrDefault();

        //    var lambda = (dynamic)CreateExpression(typeof(TSource), firstColumn.ColumnName);
        //    foreach (var columnName in datatableColumns)
        //    {
        //        if (string.IsNullOrWhiteSpace(columnName.ColumnName))
        //        {
        //            return query;
        //        }
        //        lambda = (dynamic)CreateWhereExpression(typeof(TSource), columnName.ColumnName, searchTerm);

        //        lambda = lambda += ".ToString().Contains(" + searchTerm + ")";
        //        return Queryable.Where(query, lambda);
        //    }
        //    return Queryable.Where(query, lambda);
        //}
        //private static LambdaExpression CreateWhereExpression(Type type, string propertyName, string searchTerm)
        //{
        //    var param = Expression.Parameter(type, "x");
        //    Expression exp = null;
        //    var me = Expression.Property(param, propertyName);
        //    var ce = Expression.Constant(searchTerm);
        //    var call = Expression.Call(typeof(Enumerable), "Contains", new[] { me.Type }, ce, me);

        //    Expression body = param;
        //    foreach (var member in propertyName.Split('.'))
        //    {
        //        body = Expression.PropertyOrField(body, member);
        //        MemberExpression member1 = Expression.Property(param, propertyName);
        //        ConstantExpression constant = Expression.Constant(searchTerm);

        //        me = Expression.Property(param, propertyName);
        //        ce = Expression.Constant(searchTerm);
        //        call = Expression.Call(typeof(Enumerable), "Contains", new[] { me.Type }, ce, me);
        //    }
        //    return Expression.Lambda(call, param);
        //}
    }
}