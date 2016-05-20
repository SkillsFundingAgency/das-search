using System.Collections.Generic;
using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.Collections
{
    public interface IListCollection<T>
    {
        ICollection<ShortlistedApprenticeship> GetAllItems(string listName);
        void AddItem(string listName, ShortlistedApprenticeship item);
        void RemoveApprenticeship(string listName, T item);
        void RemoveProvider(string listName, T apprenticeship, ShortlistedProvider item);
    }
}
