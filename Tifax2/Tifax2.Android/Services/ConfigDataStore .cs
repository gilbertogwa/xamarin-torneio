using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using TIFA.Models;
using TIFA.Services;

namespace TIFA.Droid.Services
{
    public class ConfigDataStore : DataStoreBase, IDataStore<Config>
    {
        private static Config _config;

        public Task<bool> AddItemAsync(Config item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Config> GetItemAsync(string id = null)
        {
            if (_config == null)
            {
                _config = await _database.Child("config").OnceSingleAsync<Config>();
            }
            return _config;
        }

        public Task<IEnumerable<Config>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public void Listen(ObservableCollection<Config> jogadores)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAll(IEnumerable<Config> items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Config item)
        {
            throw new NotImplementedException();
        }
    }
}