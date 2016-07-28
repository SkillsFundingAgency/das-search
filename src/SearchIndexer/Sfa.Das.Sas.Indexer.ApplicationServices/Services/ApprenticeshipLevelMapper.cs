namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
    using System.Collections.Generic;

    public static class ApprenticeshipLevelMapper
    {
        private static Dictionary<int, int> Dictionary => new Dictionary<int, int>
            {
                { 2, 3 },
                { 3, 2 },
                { 4, 20 },
                { 5, 21 },
                { 6, 22 },
                { 7, 23 },
                { 20, 4 },
                { 21, 5 },
                { 22, 6 },
                { 23, 7 }
            };

        public static int MapToLevel(int progType)
        {
            return Dictionary.ContainsKey(progType) ? Dictionary[progType] : 0;
        }
    }
}
