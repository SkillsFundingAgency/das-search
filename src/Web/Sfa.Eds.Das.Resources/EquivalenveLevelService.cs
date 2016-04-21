using System.Collections.Generic;

namespace Sfa.Eds.Das.Resources
{
    public static class EquivalenveLevelService
    {
        private static readonly Dictionary<int, string> _dictionary;

        static EquivalenveLevelService()
        {
            _dictionary = new Dictionary<int, string>
            {
                { 1, EquivalenceLevels.FirstLevel },
                { 2, EquivalenceLevels.SecondLevel },
                { 3, EquivalenceLevels.ThirdLevel },
                { 4, EquivalenceLevels.FourthLevel },
                { 5, EquivalenceLevels.FifthLevel },
                { 6, EquivalenceLevels.SixthLevel },
                { 7, EquivalenceLevels.SeventhLevel },
                { 8, EquivalenceLevels.EighthLevel }
            };
        }

        public static string GetFrameworkLevel(string item)
        {
            if (string.IsNullOrEmpty(item)) return string.Empty;
            return _dictionary.ContainsKey(int.Parse(item)) ? _dictionary[int.Parse(item)] : string.Empty;
        }
    }
}