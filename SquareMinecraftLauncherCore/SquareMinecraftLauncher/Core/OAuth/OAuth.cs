using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public interface OAuth
    {
        string Login();
        string GetToken(string code);
        string RefreshingTokens(string code);
    }
}
