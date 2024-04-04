using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels.Attributes
{
    

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnOrderAttribute : Attribute
    {
        public int Order { get; set; }
        public ColumnOrderAttribute(int order)
        {
            Order = order;
        }
    }
}

