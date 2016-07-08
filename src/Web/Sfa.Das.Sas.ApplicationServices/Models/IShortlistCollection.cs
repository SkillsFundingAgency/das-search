using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public interface IShortlistCollection<T>
    {
        ICollection<ShortlistedApprenticeship> GetAllItems(string listName);
        void AddItem(string listName, ShortlistedApprenticeship item);
        void RemoveApprenticeship(string listName, T item);
        void RemoveProvider(string listName, T apprenticeship, ShortlistedProvider item);
    }
}
