using ClosedXML.Excel;
using CommonModels.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace ExcelGenerator
{
    public interface IExcelGen
    {
        byte[] Create<T>(List<T> item, string SheetTitle);
    }
    public class ExcelGen : IExcelGen
    {
        private string? IsColumnNameAttribute(PropertyInfo prop)
        {
            var attributes = prop.GetCustomAttributes(true);
            if (attributes.Any())
            {
                foreach (var att in attributes)
                {
                    ColumnNameAttribute? nameAttribute = att as ColumnNameAttribute;
                    if (nameAttribute != null)
                    {
                        return nameAttribute.Name;
                    }
                }
            }
            return null;
        }
        private string? IsDateFormatAttribute(PropertyInfo prop)
        {
            var attributes = prop.GetCustomAttributes(true);
            if (attributes.Any())
            {
                foreach (var att in attributes)
                {
                    DateFormatAttribute? dateFormat = att as DateFormatAttribute;
                    if (dateFormat != null)
                    {
                        return dateFormat.Format;
                    }
                }
            }
            return null;
        }
        private string[]? IsTrueFalseAttribute(PropertyInfo prop)
        {
            var attributes = prop.GetCustomAttributes(true);
            if (attributes.Any())
            {
                foreach (var att in attributes)
                {
                    TrueFalseAttribute? truefalse = att as TrueFalseAttribute;
                    if (truefalse != null)
                    {
                        return truefalse.TF.Split('_');

                    }
                }
            }
            return null;
        }
        public byte[] Create<T>(List<T> items, string SheetTitle)
        {
            var wbook = new XLWorkbook();

            var ws = wbook.Worksheets.Add(SheetTitle);

            var props = typeof(T).GetProperties();
            bool AreOrdered = props.Any(x => x.CustomAttributes.Any(z => z.AttributeType == typeof(ColumnOrderAttribute)));
            if (AreOrdered)
            {
              props  = props
                    .Select(x => new {prop=x, ord=x.GetCustomAttribute<ColumnOrderAttribute>()?.Order })
                    .OrderBy(z=>z.ord ==null).ThenBy(z=>z.ord).Select (a=>a.prop).ToArray();
            }

           
            int col = 1;
            int row = 1;
            foreach (var prop in props)
            {
                ws.Cell(row, col).SetValue(IsColumnNameAttribute(prop) ?? prop.Name);
                ws.Cell(row, col).Style.Font.Bold = true;
                ws.Cell(row, col).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                col++;
            }


            foreach (var item in items)
            {
                row++;
                col = 0;
                foreach (var prop in props)
                {
                    col++;
                    object value = item?.GetType().GetProperty(prop.Name)?.GetValue(item);
                    if (value == null || (value is string s && String.IsNullOrWhiteSpace(s)))
                    {
                        continue;
                    }
                    if (prop.CustomAttributes.Any())
                    {
                        string? dateFormat = IsDateFormatAttribute(prop);
                        if (dateFormat != null)
                        {
                            ws.Cell(row, col).Value = ((DateTime)value).ToString(dateFormat ?? "dd/MM/yyyy");
                            continue;
                        }

                        string[]? trueFalse = IsTrueFalseAttribute(prop);
                        if (trueFalse != null)
                        {
                            ws.Cell(row, col).Value = ((bool)value) == true ? trueFalse[0] : trueFalse[1];
                            continue;
                        }

                        ws.Cell(row, col).Value = (value).ToString();
                    }
                    else
                    {
                        ws.Cell(row, col).Value = value.ToString();
                    }
                }
            }

            ws.Columns().AdjustToContents();
            var M = new MemoryStream();
            wbook.SaveAs(M);
            return M.ToArray();
        }
    }
}
