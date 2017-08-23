using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sfa.Das.Sas.Web.Helpers
{
    public interface IStringUrlHelper
    {
        string ModifyProviderNameForUrlUsage(string providerNameToProcess);
    }

    public class StringUrlHelper : IStringUrlHelper
    {
        public string ModifyProviderNameForUrlUsage(string providerNameToProcess)
        {
            var lowerCaseAndApostrophesAndHyphensRemoved = Regex.Replace(providerNameToProcess.ToLower(),"['-]", string.Empty);
            var ampersandAndPlusReplaced = Regex.Replace(lowerCaseAndApostrophesAndHyphensRemoved, "[&+]", "and");
            var splitBySpacesAndOtherChars = Regex.Split(ampersandAndPlusReplaced, @"[\s.(),]+");
            var rebuildExcludingNoContent = string.Join(
                "-",
                splitBySpacesAndOtherChars.Except(new List<string> {string.Empty})
            );

         return Regex.Escape(rebuildExcludingNoContent);
     }
    }
}