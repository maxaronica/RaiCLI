using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels.Attributes
{
     
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TrueFalseAttribute : Attribute
    {
        public string TF { get; set; }
        public TrueFalseAttribute(string t_f)
        {
            TF = t_f;
        }
    }
}
