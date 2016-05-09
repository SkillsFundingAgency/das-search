using System.Collections.Generic;

namespace Sfa.Das.Sas.Core.Collections
{
    public interface IListCollection<T>
    {
        ICollection<T> GetAllItems(string listName);
        void AddItem(string listName, T item);
        void RemoveItem(string listName, T item);
        void RemoveList(string listName);
    }
}
