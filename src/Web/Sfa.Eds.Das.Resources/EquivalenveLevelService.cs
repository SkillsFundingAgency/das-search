namespace Sfa.Eds.Das.Resources
{
    public class EquivalenveLevelService
    {
        public static string GetFrameworkLevel(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                switch (int.Parse(@item))
                {
                    case 1:
                        return EquivalenceLevels.FirstLevel;
                    case 2:
                        return EquivalenceLevels.SecondLevel;
                    case 3:
                        return EquivalenceLevels.ThirdLevel;
                    case 4:
                        return EquivalenceLevels.FourthLevel;
                    case 5:
                        return EquivalenceLevels.FifthLevel;
                    case 6:
                        return EquivalenceLevels.SixthLevel;
                    case 7:
                        return EquivalenceLevels.SeventhLevel;
                    case 8:
                        return EquivalenceLevels.EighthLevel;
                    default: 
                        return string.Empty;
                }
            }

            return string.Empty;
        }
    }
}