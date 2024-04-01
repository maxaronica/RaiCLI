using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaiCLI.Core
{
    public interface IRaiCLI
    {
        void Invoke(string[] args);

        string Usage();
    }
}
