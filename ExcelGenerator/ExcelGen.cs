using ClosedXML.Excel;
using CommonModels.Attributes;

namespace ExcelGenerator
{
    public interface IExcelGen
    {
        byte[] Create<T>(List<T> item, string SheetTitle);
    }
    public class ExcelGen : IExcelGen
    {
        public byte[] Create<T>(List<T> items, string SheetTitle)
        {
            var wbook = new XLWorkbook();

            var ws = wbook.Worksheets.Add(SheetTitle);

            var props = typeof(T).GetProperties();
            int col = 1;
            int row = 1;
            foreach ( var prop in props )
            {
                ws.Cell(row,col).SetValue(prop.Name);
                ws.Cell(row,col).Style.Font.Bold = true;
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
                        var attributes = prop.GetCustomAttributes(true);
                        foreach (var att in attributes)
                        {
                            DateFormatAttribute? dateFormat = att as DateFormatAttribute;
                            if (dateFormat != null)
                            {
                                ws.Cell(row, col).Value = ((DateTime)value);//.ToString(dateFormat.Format);
                            }
                        }

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
