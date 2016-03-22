namespace Sfa.Eds.Das.Indexer.Core.Extensions
{
    using System.IO;

    public static class StringExtensions
    {
        public static Stream GenerateStreamFromString(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string RemoveQuotationMark(this string self)
        {
            return self.Replace("\"", string.Empty);
        }
    }
}