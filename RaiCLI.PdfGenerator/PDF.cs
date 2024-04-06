using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;
using System.IO;

namespace RaiCLI.PdfGenerator
{
    public class PDF
    {
        public void Build()
        {
            using (var document = new Document())
            {
                System.IO.FileStream stream = new FileStream("c:\\users\\massi\\desktop\\prova.pdf", FileMode.Create);

                var writer = PdfWriter.GetInstance(document, stream);
                writer.PageEvent = new HtmlPageEventHelper("<span>titolo doc</span><div style='height:1px;background-color:black'></div>");
                document.Open();


                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("<span>ciao <b>Max</b></span>" +
                    "<table><tr><td style='width:50%;font-size:30'>CIAO </td><td style='width:50%'>MAX<img src='a.png'/></td></tr></table>"+
                    "<div style='height:80px;'></div>"+
                    "<div style='text-align:justify'>Instead of rehashing an explanation of the example code above, see the documentation (iText removed documentation, linked to Wayback Machine) to get a better idea of why you need to setup the parser that way.\r\n\r\nAlso note:\r\n\r\nXML Worker does not support all CSS2/CSS3 properties, so you may need to experiment with what works or doesn't work with regards to how close you want the PDF to look to the HTML displayed in the browser.\r\nThe HTML snippet removed the p tag, since the style can be applied directly to the td tag.\r\nThe inline width property. If omitted the columns will be variable widths that match if the text had been rendered horizontally.</div>" +
                    "<div style='height:3px;background-color:yellow'></div>")))
                {

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, System.Text.Encoding.ASCII);

                }
                document.Close();
                writer.Close();
            }
        }
    }
    public class HtmlPageEventHelper : PdfPageEventHelper
    {
        public HtmlPageEventHelper(string html)
        {
            this.html = html;
        }
         
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            ColumnText ct = new ColumnText(writer.DirectContent);
            XMLWorkerHelper.GetInstance().ParseXHtml(new ColumnTextElementHandler(ct), new StringReader(html));
            ct.SetSimpleColumn(document.Left, document.Top, document.Right, document.GetTop(-20), 10, Element.ALIGN_MIDDLE);
            ct.Go();
        }

        string html = null;
    }
    public class ColumnTextElementHandler : IElementHandler
    {
        public ColumnTextElementHandler(ColumnText ct)
        {
            this.ct = ct;
        }

        ColumnText ct = null;

        public void Add(IWritable w)
        {
            if (w is WritableElement)
            {
                foreach (IElement e in ((WritableElement)w).Elements())
                {
                    ct.AddElement(e);
                }
            }
        }
    }
}
