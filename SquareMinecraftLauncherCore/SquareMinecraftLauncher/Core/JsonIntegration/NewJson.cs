using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core
{
    internal class NewJson
    {
        internal string newJson(List<Forge.ForgeY.LibrariesItem> libraries, string Arguments, string mainClass,Forge.ForgeY.Root r)
        {
            string id = "\"id\":\"" + r.assetIndex.id;
            string size = "\"size\":\"" + r.assetIndex.size;
            string url = "\"url\":\"" + r.assetIndex.url;
            string ret = "{\"arguments\":{\"game\":[" + Arguments + "]},\"assetIndex\":{" + id +"\","+ size + "\"," + url + "\"},\"assets\":\"" + r.assets + "\",";
            ret += "\"downloads\":{\"Client\":{\"sha1\":\"" + r.downloads.client.sha1 + "\",\"size\":\"" + r.downloads.client.size + "\",\"url\":\"" + r.downloads.client.url + "\"}},";
            ret += "\"id\":\""+ r.id + "\",";
            ret += "\"libraries\":[";
            foreach (var i in libraries)//将新组成的libraries数组写成json
            {
                if (i.downloads == null)
                {
                    ret += "{\"downloads\":{\"artifact\":{\"url\":\"" + "https://libraries.minecraft.net/\"}},";
                }
                else
                {
                    ret += "{\"downloads\":{\"artifact\":{\"url\":\"" + i.downloads.artifact.url +"\"}},";
                }
                ret += "\"name\":\"" + i.name + "\"";
                if (i.natives != null)
                {
                    if (i.natives.windows != null)
                    {
                        ret += ",\"natives\":{\"windows\":\"natives - windows\"}";
                    }
                }
                ret += "},";
            }
            ret = ret.Substring(0, ret.Length - 1);
            ret += "],";
            ret += "\"mainClass\":\"" + mainClass + "\"}";
            return ret;
        }
    }
}
