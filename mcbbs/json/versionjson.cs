using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace json4
{
    internal class AssetIndex
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sha1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }
    internal class LibrariesItem
    {
        public string name { get; set; }
        public downloads downloads { get; set; }
        public natives natives { get; set; }
    }
    internal class downloads
    {
        public artifact artifact { get; set; }
    }
    internal class artifact
    {
        public string url { get; set; }
    }
    internal class natives
    {
        public string windows { get; set; }
        public string osx { get; set; }
        public string linux { get; set; }
    }
    internal class File
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sha1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    internal class Root
    {
        public AssetIndex AssetIndex { get; set; }
        public string assets { get; set; }
        public string id { get; set; }
        public List<LibrariesItem> libraries { get; set; }
        public string mainClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string minecraftArguments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int minimumLauncherVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string releaseTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }
    internal class Root1
    {
        public List<LibrariesItem> libraries { get; set; }
    }
}
