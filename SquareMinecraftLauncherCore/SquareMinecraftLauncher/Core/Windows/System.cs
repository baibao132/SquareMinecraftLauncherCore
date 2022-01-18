namespace System
{
    internal class Directory
    {
        internal static string GetCurrentDirectory()
        {

            if (Files != null)
            {
                Files = Files.Replace(".minecraft", "");
                return Files;
            }
            return System.IO.Directory.GetCurrentDirectory();
        }

        internal static string Files { get; set; }
    }
}
