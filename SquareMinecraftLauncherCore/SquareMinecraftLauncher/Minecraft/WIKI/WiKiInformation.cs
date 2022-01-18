using SquareMinecraftLauncher.Core.Curseforge;

namespace SquareMinecraftLauncher.Minecraft
{
    internal class WiKiInformation
    {
        public string Title { get; internal set; }

        public string introduce { get; internal set; }

        public string[] ImgUrl { get; internal set; }

        public ExternalLinkItem[] ExternalLinks { get; internal set; }

        public string[] SupportVersion { get; internal set; }

        public PreModItem[] PreMod { get; internal set; }
    }
}
