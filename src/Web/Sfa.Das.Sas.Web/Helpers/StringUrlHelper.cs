using System.Text;

namespace Sfa.Das.Sas.Web.Helpers
{
    public interface IStringUrlHelper
    {
        string ModifyStringForUrlUsage(string stringToProcess);
    }

    public class StringUrlHelper : IStringUrlHelper
    {
     public string ModifyStringForUrlUsage(string stringToProcess)
        {
            var firstpass = stringToProcess.ToLower().Replace("&", "and").Replace("+", "and").Replace("("," "). Replace(")"," ").Replace(" ", "-");
            var secondpass = new StringBuilder();
            foreach (var c in firstpass)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || c == '-')
                {
                    secondpass.Append(c);
                }
            }

            var thirdpass = secondpass.ToString();

            while (thirdpass.Contains("--"))
            {
                thirdpass = thirdpass.Replace("--", "-");
            }

            return thirdpass;
        }
    }

  
}