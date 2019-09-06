using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher
{
    public sealed class SikaDeerLauncherException : Exception
    {
        public SikaDeerLauncherException(string message) :base(message)
        {
            
        }
    }
}
