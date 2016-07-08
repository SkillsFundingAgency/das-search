using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories
{
    public class LarsMetaDataFactory : IGenericMetaDataFactory
    {
        private readonly Dictionary<Type, IMetaDataFactory> _metaDataFactories;

        public LarsMetaDataFactory(IEnumerable<IMetaDataFactory> metaDataFactories)
        {
            _metaDataFactories = new Dictionary<Type, IMetaDataFactory>();

            foreach (var factory in metaDataFactories)
            {
                _metaDataFactories.Add(factory.MetaDataType, factory);
            }
        }

        public T Create<T>(IEnumerable<string> metaValues)
            where T : class
        {
            if (metaValues == null)
            {
                return null;
            }

            var values = metaValues.ToList();

            if (!values.Any())
            {
                return null;
            }

            if (!_metaDataFactories.ContainsKey(typeof(T)))
            {
                return null;
            }

            var factory = _metaDataFactories[typeof(T)];

            return factory.Create(values) as T;
        }
    }
}