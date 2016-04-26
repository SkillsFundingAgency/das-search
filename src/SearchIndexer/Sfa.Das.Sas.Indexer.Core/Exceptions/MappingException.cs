using System;
using System.Runtime.Serialization;

namespace Sfa.Das.Sas.Indexer.Core.Exceptions
{
    [System.Serializable]
    public class MappingException : Exception
    {
        public MappingException()
        {
        }

        public MappingException(string message)
            : base(message)
        {
        }

        public MappingException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected MappingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
