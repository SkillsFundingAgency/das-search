namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper
{
    using System.IO;

    public class FileHelper
    {
        public static void EnsureDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void DeleteFile(string zipFilePath)
        {
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);
        }

        public static void DeleteRecursive(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }
    }
}