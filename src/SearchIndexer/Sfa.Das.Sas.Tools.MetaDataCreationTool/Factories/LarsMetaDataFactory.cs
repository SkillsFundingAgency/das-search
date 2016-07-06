using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories
{
    public class LarsMetaDataFactory : IGenericMetaDataFactory
    {
        //private readonly Dictionary<Type, IMetaDataFactory> _metaDataFactories;

        //public LarsMetaDataFactory(IEnumerable<IMetaDataFactory> metaDataFactories)
        //{
        //    _metaDataFactories = new Dictionary<Type, IMetaDataFactory>();

        //    foreach (var factory in metaDataFactories)
        //    {
        //        _metaDataFactories.Add(factory.MetaDataType, factory);
        //    }
        //}

        public T Create<T>(IEnumerable<string> values)
            where T : class
        {
            if (values == null)
            {
                return null;
            }

            //if (!_metaDataFactories.ContainsKey(typeof(T)))
            //{
            //    return null;
            //}

            //var factory = _metaDataFactories[typeof(T)];

            //return factory.Create(values.ToList()) as T;

            if (typeof(T) == typeof(FrameworkMetaData))
            {
                return CreateFrameworkMetaData(values.ToList()) as T;
            }

            if (typeof(T) == typeof(LarsStandard))
            {
                return CreateStandard(values.ToList()) as T;
            }

            if (typeof(T) == typeof(FrameworkAimMetaData))
            {
                return CreateFrameworkAimMetaData(values.ToList()) as T;
            }

            if (typeof(T) == typeof(FrameworkComponentTypeMetaData))
            {
                return CreateFrameworkComponentTypeMetaData(values.ToList()) as T;
            }

            if (typeof(T) == typeof(LearningDeliveryMetaData))
            {
                return CreateLearningDeliveryMetaData(values.ToList()) as T;
            }

            return null;
        }

        private static FrameworkMetaData CreateFrameworkMetaData(IReadOnlyList<string> values)
        {
            if (values == null || values.Count <= 11)
            {
                return null;
            }

            var frameworkCode = TryParse(values[0]);

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkMetaData
                {
                    FworkCode = frameworkCode,
                    ProgType = TryParse(values[1]),
                    PwayCode = TryParse(values[2]),
                    PathwayName = values[3].RemoveQuotationMark(),
                    EffectiveFrom = TryGetDate(values[4].RemoveQuotationMark()) ?? DateTime.MinValue,
                    EffectiveTo = TryGetDate(values[5].RemoveQuotationMark()),
                    SectorSubjectAreaTier1 = TryParseDouble(values[6]),
                    SectorSubjectAreaTier2 = TryParseDouble(values[7]),
                    NASTitle = values[9].RemoveQuotationMark()
                };
        }

        private static LarsStandard CreateStandard(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 11)
            {
                return null;
            }

            var standardid = GetStandardId(values);

            if (standardid < 0)
            {
                return null;
            }

            return new LarsStandard
            {
                Id = standardid,
                Title = values[2].RemoveQuotationMark(),
                NotionalEndLevel = TryParse(values[4]),
                StandardUrl = values[8],
                SectorSubjectAreaTier1 = TryParseDouble(values[9]),
                SectorSubjectAreaTier2 = TryParseDouble(values[10]),
            };
        }

        private static FrameworkAimMetaData CreateFrameworkAimMetaData(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 7)
            {
                return null;
            }

            var frameworkCode = TryParse(values[0]);

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkAimMetaData
            {
                FworkCode = frameworkCode,
                ProgType = TryParse(values[1].RemoveQuotationMark()),
                PwayCode = TryParse(values[2].RemoveQuotationMark()),
                LearnAimRef = values[3].RemoveQuotationMark(),
                EffectiveFrom = TryGetDate(values[4]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[5]),
                FrameworkComponentType = TryParse(values[6])
            };
        }

        private static FrameworkComponentTypeMetaData CreateFrameworkComponentTypeMetaData(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 5)
            {
                return null;
            }

            return new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = TryParse(values[0]),
                FrameworkComponentTypeDesc = values[1].RemoveQuotationMark(),
                FrameworkComponentTypeDesc2 = values[2].RemoveQuotationMark(),
                EffectiveFrom = TryGetDate(values[3]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[4])
            };
        }

        private static LearningDeliveryMetaData CreateLearningDeliveryMetaData(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 5)
            {
                return null;
            }

            return new LearningDeliveryMetaData
            {
                LearnAimRef = values[0].RemoveQuotationMark(),
                EffectiveFrom = TryGetDate(values[1]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[2]),
                LearnAimRefTitle = values[3].RemoveQuotationMark(),
                LearnAimRefType = TryParse(values[4].RemoveQuotationMark())
            };
        }

        private static int GetStandardId(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 7)
            {
                return -1;
            }

            return TryParse(values[0]);
        }

        private static int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        private static double TryParseDouble(string s)
        {
            double i;
            if (double.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        private static DateTime? TryGetDate(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
            {
                return dateTime;
            }

            return null;
        }
    }
}
