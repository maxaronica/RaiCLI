using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RaiCLI.Core.CommandClasses
{
    public class Help : IRaiCLI
    {
        public void Invoke(string[] args)
        {
            var a = Assembly.GetExecutingAssembly();
            var types = GetTypesInNamespace(a, "RaiCLI.Core.CommandClasses");
            foreach (var type in types)
            {
                if (type.IsDefined(typeof(CompilerGeneratedAttribute), true))
                {
                    continue;
                }
               
                IRaiCLI? instance = (IRaiCLI?)Activator.CreateInstance(type);
                if (instance != null) Console.WriteLine(instance.Usage());
            }
        }
        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
        public string Usage()
        {
            return "";
        }
    }
}
