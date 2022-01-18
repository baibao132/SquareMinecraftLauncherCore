﻿using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class PossessionGame
    {
        public class SkinsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string variant { get; set; }
        }

        public class CapesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string @alias { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SkinsItem> skins { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<CapesItem> capes { get; set; }
        }
    }
}
