using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Web.Repositories
{
    public interface IWebStore<T>
    {
        ICollection<T> FindAllItems(string listName);
        void AddItem(string listName, T item);
        void RemoveItem(string listName, T item);
        void RemoveList(string listName);
    }
}
