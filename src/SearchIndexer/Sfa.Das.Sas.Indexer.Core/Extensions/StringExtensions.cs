using System.IO;
using System.Text;

namespace Sfa.Das.Sas.Indexer.Core.Extensions
{
    public static class StringExtensions
    {
        public static Stream GenerateStreamFromString(this string s)
        {
            var stream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            return stream;
        }

        public static string RemoveQuotationMark(this string self)
        {
            if (self == null)
            {
                return string.Empty;
            }

            return self.Replace("\"", string.Empty);
        }
    }
}