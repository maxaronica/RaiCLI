using ClosedXML.Excel;
using CommonModels.Attributes;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace ExcelGenerator
{
   
    public class ExcelBuilder
    {
        private ExcelGen _gen;
        public ExcelBuilder()
        {
            _gen = new ExcelGen();
        }
        public ExcelBuilder WithSheetName(string name)
        {
            _gen.SheetName = name;
            return this;
        }
        public ExcelBuilder WithSheetTitle(string title)
        {
            _gen.SheetTitle  = title;
            return this;
        }
        public ExcelBuilder WithSheetSubTitle(string subtitle)
        {
            _gen.SheetSubTitle = subtitle;
            return this;
        }
        public ExcelBuilder WithExistingFile(byte[] bytes)
        {
            _gen.ExistingFile = bytes;
            return this;
        }
        public byte[] Build<T>(List<T> items)
        {
            return _gen.Create<T>(items);
        }
        private class ExcelGen
        {
            public string SheetName { get; set; }
            public string SheetTitle { get; set; }
            public string SheetSubTitle { get; set; }
            public byte[] ExistingFile {  get; set; }

            private string? HasColumnNameAttribute(PropertyInfo prop)
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
            private string? HasDateFormatAttribute(PropertyInfo prop)
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
            private string[]? HasTrueFalseAttribute(PropertyInfo prop)
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

            private PropertyInfo[] GetPropertiesOrderedByAttribute(PropertyInfo[] props)
            {
                bool AreOrdered = props.Any(x => x.CustomAttributes.Any(z => z.AttributeType == typeof(ColumnOrderAttribute)));
                if (AreOrdered)
                {
                    props = props
                          .Select(x => new { prop = x, ord = x.GetCustomAttribute<ColumnOrderAttribute>()?.Order })
                          .OrderBy(z => z.ord == null).ThenBy(z => z.ord).Select(a => a.prop).ToArray();
                }
                return props;
            }
            public byte[] Create<T>(List<T> items)
            {
                XLWorkbook wbook = new XLWorkbook();
                if (this.ExistingFile != null)
                {
                    MemoryStream Ms = new MemoryStream(this.ExistingFile);
                    wbook = new XLWorkbook(Ms);
                }

                var ws = wbook.Worksheets.Add(SheetName);
                int row = 1;
                if (!String.IsNullOrWhiteSpace(this.SheetTitle))
                {
                    ws.Cell(row, 1).SetValue(this.SheetTitle);
                    ws.Cell(row, 1).Style.Font.FontSize = 20;
                    row++;
                }
                if (!String.IsNullOrWhiteSpace(this.SheetSubTitle))
                {
                    ws.Cell(row, 1).SetValue(this.SheetSubTitle);
                    ws.Cell(row, 1).Style.Font.FontSize = 16;
                    row++;
                }
                var props = typeof(T).GetProperties();

                props = GetPropertiesOrderedByAttribute(props);


                int col = 1;
                foreach (var prop in props)
                {
                    ws.Cell(row, col).SetValue(HasColumnNameAttribute(prop) ?? prop.Name);
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
                        object? value = item?.GetType().GetProperty(prop.Name)?.GetValue(item);
                        if (value == null || (value is string s && String.IsNullOrWhiteSpace(s)))
                        {
                            continue;
                        }
                        if (prop.CustomAttributes.Any())
                        {
                            string? dateFormat = HasDateFormatAttribute(prop);
                            if (dateFormat != null)
                            {
                                ws.Cell(row, col).Value = ((DateTime)value).ToString(dateFormat ?? "dd/MM/yyyy");
                                continue;
                            }

                            string[]? trueFalse = HasTrueFalseAttribute(prop);
                            if (trueFalse != null)
                            {
                                ws.Cell(row, col).Value = ((bool)value) == true ? trueFalse[0] : trueFalse[1];
                                continue;
                            }
                        }
                        ws.Cell(row, col).Value = value.ToString();
                    }
                }

                ws.Columns().AdjustToContents();
                var M = new MemoryStream();
                wbook.SaveAs(M);
                return M.ToArray();
            }
        }
    }
   
}
