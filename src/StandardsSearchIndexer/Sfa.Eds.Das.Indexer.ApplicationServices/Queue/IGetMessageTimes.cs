namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System;
    using System.Collections.Generic;

    public interface IGetMessageTimes
    {
        IEnumerable<DateTime> GetInsertionTimes(string queuename);
    }
}