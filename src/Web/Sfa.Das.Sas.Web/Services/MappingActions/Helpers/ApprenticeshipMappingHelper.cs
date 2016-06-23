using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    public static class ApprenticeshipMappingHelper
    {
        public static string GetTypicalLengthMessage(TypicalLength typicalLength)
        {
            var fromBiggerThanTo = typicalLength?.From > typicalLength?.To && typicalLength?.To > 0;
            var lengthIsZero = typicalLength?.From == 0 && typicalLength?.To == 0;
            if (typicalLength == null || fromBiggerThanTo || lengthIsZero)
            {
                return string.Empty;
            }

            if (GetSingleValue(typicalLength.From, typicalLength.To) != 0)
            {
                var value = GetSingleValue(typicalLength.From, typicalLength.To);
                return $"{value} {GetUnit(typicalLength.Unit)}";
            }

            return $"{typicalLength.From} to {typicalLength.To} {GetUnit(typicalLength.Unit)}";
        }

        private static int GetSingleValue(int from, int to)
        {
            if (from == to)
            {
                return from;
            }

            if (from > 0 && to == 0)
            {
                return from;
            }

            if (from == 0 && to > 0)
            {
                return to;
            }

            return 0;
        }

        private static string GetUnit(string unit)
        {
            if (unit == "m")
            {
                return "months";
            }

            return string.Empty;
        }
    }
}