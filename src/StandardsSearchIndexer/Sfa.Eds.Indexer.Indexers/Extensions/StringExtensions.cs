namespace Sfa.Eds.Das.Indexer.Common.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using LINQtoCSV;

    public static class StringExtensions
    {
        public static ICollection<T> FromCsv<T>(this string result) where T : class, new()
        {
            var cc = new CsvContext();
            var stream = GenerateStreamFromString(result);
            var reader = new StreamReader(stream);
            return cc.Read<T>(reader).ToList();
        }

        public static Stream GenerateStreamFromString(this string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}