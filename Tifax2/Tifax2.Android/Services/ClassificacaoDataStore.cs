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
    public class ClassificacaoDataStore : DataStoreBase, IDataStore<Classificacao>
    {


        public async Task<bool> AddItemAsync(Classificacao item)
        {
            var db =_database.Child($"classificacao/{item.Jogador}");
           
            var json = JsonConvert.SerializeObject(item);

            await db.PutAsync(json);

            return await Task.FromResult(true);
        }

        public async Task<bool> SaveAll(IEnumerable<Classificacao> classificao)
        {
            var db = _database.Child("classificacao/");

            var dic = classificao.ToDictionary(a => a.Jogador);

            var json = JsonConvert.SerializeObject(dic);

            await db.PutAsync(json);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var db = _database.Child($"classificacao/{id}");

            await db.DeleteAsync();
            return await Task.FromResult(true);
        }

        public Task<Classificacao> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Classificacao>> GetItemsAsync(bool forceRefresh = false)
        {
            var response = await _database.Child("classificacao").OnceSingleAsync<Dictionary<string,Classificacao>>();

            var itens = response.Where(a=> a.Key != null)
                .Select(a=> a.Value)
                .OrderBy(a => a.Posicao)
                .ToArray();

            return await Task.FromResult(itens);
        }

        public void Listen(ObservableCollection<Classificacao> jogadores)
        {
        }

        public async Task<bool> UpdateItemAsync(Classificacao item)
        {
            return await Task.FromResult(true);
        }
    }
}