using System;
using System.IO;
using System.Text;

namespace LARSMetaDataExplorer.Extensions
{
    public static class StringExtensions
    {
        public static Stream GenerateStreamFromString(this string str)
        {
            var stream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            return stream;
        }

        public static string RemoveQuotationMark(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            return str.Replace("\"", string.Empty);
        }

        public static int SafeParseInt(this string str)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                return i;
            }

            return -1;
        }

        public static double SafeParseDouble(this string str)
        {
            double i;
            if (double.TryParse(str, out i))
            {
                return i;
            }

            return -1;
        }

        public static DateTime? SafeParseDate(this string str)
        {
            DateTime dateTime;
            if (DateTime.TryParse(str, out dateTime))
            {
                return dateTime;
            }

            return null;
        }
    }
}