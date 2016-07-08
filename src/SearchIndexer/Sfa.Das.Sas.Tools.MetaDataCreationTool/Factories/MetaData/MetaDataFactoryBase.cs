using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class MetaDataFactoryBase
    {
        protected MetaDataFactoryBase()
        {
        }

        protected static int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        protected static double TryParseDouble(string s)
        {
            double i;
            if (double.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        protected static DateTime? TryGetDate(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        protected static int GetStandardId(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 7)
            {
                return -1;
            }

            return TryParse(values[0]);
        }
    }
}
