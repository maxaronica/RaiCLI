using RaiCLI.DbContextEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RaiCLI.Core
{
    public interface ISelector
    {
        IRaiCLI? GetClass(string[] arg);
    }
    public class Selector (aspnetWebApplicationIdentityContext _dbContext) : ISelector
    {
      
        public  IRaiCLI? GetClass(string[] arg)
        {
            var tot = _dbContext.AspNetUsers.Count();
            if (!arg.Any())  arg = new string[] { "Help"};

            var classAddress = $"RaiCLI.Core.CommandClasses.{arg[0].Substring(0,1).ToUpper()}{arg[0].Substring(1).ToLower()}";
            Type? type = GetType(classAddress);

            if (type == null)  return null;

            IRaiCLI? instance = (IRaiCLI?) Activator.CreateInstance(type);
            return instance;
        }
        public  Type? GetType(string strFullyQualifiedName)
        {
            Type? type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return type;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return type;
            }
            return null;
        }
       
    }
}
