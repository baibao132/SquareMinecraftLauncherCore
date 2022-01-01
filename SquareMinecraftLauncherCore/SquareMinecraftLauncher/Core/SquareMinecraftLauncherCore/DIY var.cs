﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public sealed class Getlogin
    {
        public string uuid { get; internal set; }
        public string token { get; internal set; }
        public string name { get; internal set; }
    }
    public sealed class AllTheExistingVersion
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; internal set; }
        /// <summary>
        /// 版本文件名
        /// </summary>
        public string version { get; internal set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string IdVersion { get; internal set; }
    }
    public sealed class MemoryInformation
    {
        /// <summary>
        /// 总内存
        /// </summary>
        public int TotalMemory { get; internal set; }
        /// <summary>
        /// 合适内存
        /// </summary>
        public int AppropriateMemory { get; internal set; }
    }
    public sealed class Skin
    {
        /// <summary>
        /// Token
        /// </summary>
        public string accessToken { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public SkinName[] NameItem { get; internal set; }
    }
    public sealed class SkinName
    {
        /// <summary>
        /// 游戏名
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// uuid
        /// </summary>
        public string uuid { get; internal set; }
    }
    public sealed class MCDownload
    {
        /// <summary>
        /// 下载网址
        /// </summary>
        public string Url { get; internal set; }
        /// <summary>
        /// 下载路径
        /// </summary>
        public string path { get; internal set; }
        internal string name { get; set; }
        internal string mainClass { get; set; }
    }
    /// <summary>
    /// 下载源
    /// </summary>
    public enum DownloadSource
    {
        /// <summary>
        /// bmclapi下载源
        /// </summary>
        bmclapiSource,
        /// <summary>
        /// minecraft下载源
        /// </summary>
        MinecraftSource,
        /// <summary>
        /// MCBBS下载源
        /// </summary>
        MCBBSSource
    }
    public class MCVersionList
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string id { get; internal set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; internal set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string releaseTime { get; internal set; }
    }
    public class ForgeList
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; internal set; }
        /// <summary>
        /// forge版本
        /// </summary>
        public string ForgeVersion { get; internal set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string ForgeTime { get; internal set; }
    }
    public class LiteloaderList
    {
        /// <summary>
        /// Liteloader版本
        /// </summary>
        public string version { get; internal set; }
        /// <summary>
        /// mc版本
        /// </summary>
        public string mcversion { get; internal set; }
        internal Lib[] lib { get; set; }
        internal string tweakClass { get; set; }
    }
    internal class Lib
    {
        internal string name { get; set; }
    }
    public class OptiFineList
    {
        /// <summary>
        /// mc版本
        /// </summary>
        public string mcversion { get; internal set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; internal set; }
        /// <summary>
        /// 补丁号
        /// </summary>
        public string patch { get; internal set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string filename { get; internal set; }
    }
    internal class mc
    {
        public string version { get; set; }
        public string url { get; set; }
    }
    public class UnifiedPass
    {
        /// <summary>
        /// token
        /// </summary>
        public string accessToken { get; set; }
        /// <summary>
        /// uuid
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 游戏名
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 验证服务器方式
    /// </summary>
    public enum AuthenticationServerMode
    {
        /// <summary>
        /// yggdrasil
        /// </summary>
        yggdrasil,
        /// <summary>
        /// 统一通行证
        /// </summary>
        UnifiedPass
    }
    /// <summary>
    /// 统一通行证皮肤
    /// </summary>
    public class UnifiedPassesTheSkin
    {
        /// <summary>
        /// 皮肤
        /// </summary>
        public string Skin {get;set;}
        /// <summary>
        /// 披风
        /// </summary>
        public string Cape { get; set; }
    }
    public enum ExpansionPack
    {
        Forge,
        Liteloader,
        Optifine,
        Fabric
    }

    public class JavaVersion
    {
        public string Version { get; internal set; }
        public string Path{ get; internal set; }
    }
}
