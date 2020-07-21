using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal class Directory
    {
        internal static string GetCurrentDirectory()
        {
            if (Files != null)
            {
                return Files;
            }
            return System.IO.Directory.GetCurrentDirectory();
        }

        internal static string Files { get; set; }
    }
}
