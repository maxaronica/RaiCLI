using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DateFormatAttribute : Attribute
    {
        public string Format { get; set; }
        public DateFormatAttribute(string format)
        {
            Format = format;
        }
    }
}
