using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Das.Sas.Web.Extensions;

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

        public static string FrameworkTitle(string currentTitle)
        {
            var s = currentTitle.Split(':');
            var framworkname = s.Length > 0 ? s[0] : string.Empty;
            var pathwayName = s.Length > 1 ? s[1] : string.Empty;

            if (string.IsNullOrWhiteSpace(pathwayName) || framworkname.Trim().Equals(pathwayName.Trim()))
            {
                return framworkname;
            }

            return currentTitle;
        }

        public static IEnumerable<string> GetTitlesFromJobRoles(IEnumerable<JobRoleItem> jobRoleItems)
        {
            if (!jobRoleItems.IsNullOrEmpty())
            {
                return jobRoleItems.Select(m => m.Title);
            }

            return new List<string>();
        }

        public static string GetExpirydateAsString(DateTime? date)
        {
            if (date != null && date > DateTime.MinValue)
            {
                return date.Value.AddDays(1).ToString("d MMMM yyyy");
            }

            return null;
        }

        public static string GetInformationText(string text)
        {
            return string.IsNullOrEmpty(text) ? "None specified." : text;
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