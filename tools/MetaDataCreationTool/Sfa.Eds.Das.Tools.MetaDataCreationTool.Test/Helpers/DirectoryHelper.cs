namespace UnitTestProject1.Helpers
{
    using System.IO;

    public static class DirectoryHelper
    {
        public static void DeleteRecursive(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }
    }
}
