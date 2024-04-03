using AutoMapper;
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
    public class Selector (aspnetWebApplicationIdentityContext _dbContext, Mapper _mapper) : ISelector
    {
        public  IRaiCLI? GetClass(string[] arg)
        {
            var tot = _dbContext.AspNetUsers.Count();
            if (!arg.Any())  arg = new string[] { "Help"};

            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "RaiCLI.Core.CommandClasses");
            Type? typeCalled = types.FirstOrDefault(x =>string.Equals( x.Name, arg[0], StringComparison.InvariantCultureIgnoreCase));
            if (typeCalled == null) 
                return null;
            else
                return (IRaiCLI?)Activator.CreateInstance(typeCalled);
        }
        
        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
    }
}
