using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnNameAttribute :Attribute
    {
        public string Name { get; set; }
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
