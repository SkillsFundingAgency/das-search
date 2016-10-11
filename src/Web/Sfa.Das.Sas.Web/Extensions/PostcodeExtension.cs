namespace Sfa.Das.Sas.Web.Extensions
{
    public static class PostcodeExtension
    {
        public static string FormatPostcode(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            str = str.Replace(" ", string.Empty);
            switch (str.Length)
            {
                case 5:
                    str = str.Insert(2, " ");
                    break;
                case 6:
                    str = str.Insert(3, " ");
                    break;
                case 7:
                    str = str.Insert(4, " ");
                    break;
            }

            return str.ToUpper();
        }
    }
}