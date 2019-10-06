using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TIFA.Models;

namespace TIFA.Services
{    
    
    public interface IDataStoreList<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }

    public interface IDataStore<T>: IDataStoreList<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        
        void Listen(ObservableCollection<T> jogadores);
    }


}
