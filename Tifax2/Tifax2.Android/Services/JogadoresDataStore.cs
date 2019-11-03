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
    public class JogadoresDataStore : DataStoreBase, IDataStore<Jogador>
    {


        public async Task<bool> AddItemAsync(Jogador item)
        {
            var db =_database.Child($"jogadores/{item.Nome}");
           
            var json = JsonConvert.SerializeObject(item);

            await db.PutAsync(json);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await Task.FromResult(true);
        }

        public Task<Jogador> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Jogador>> GetItemsAsync(bool forceRefresh = false)
        {
            var response = await _database.Child("jogadores").OnceSingleAsync<Dictionary<string, Jogador>>();

            var itens = response.Where(a=> a.Key != null)
                .Select(a=> a.Value)
                .OrderBy(a => a.Nome)
                .ToArray();

            return await Task.FromResult(itens);
        }

        public void Listen(ObservableCollection<Jogador> jogadores)
        {
        }

        public Task<bool> SaveAll(IEnumerable<Jogador> items)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(Jogador item)
        {
            return await Task.FromResult(true);
        }
    }
}