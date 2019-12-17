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
    public class PlacarDataStore : DataStoreBase, IDataStore<Placar>
    {


        public async Task<bool> AddItemAsync(Placar item)
        {
            var j1 = item.JogadorA.Nome.Trim().Replace(" ", "_").ToLower();
            j1 = j1 + "_p" + item.PosicaoA;

            var j2 = item.JogadorB.Nome.Trim().Replace(" ", "_").ToLower();
            j2 = j2 + "_p" + item.PosicaoB;

            var jogadores = string.Compare(j1, j2) <= 0 ? $"{j1}_{j2}" : $"{j2}_{j1}";
            var id = item.Data.ToString("yyyy_MM_dd_") + jogadores;

            var db =_database.Child($"placares/{id}");
           
            var json = JsonConvert.SerializeObject(item);

            await db.PutAsync(json);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await Task.FromResult(true);
        }

        public Task<Placar> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Placar>> GetItemsAsync(bool forceRefresh = false)
        {
            Dictionary<string, Placar> response;

            try
            {
                response = await _database.Child("placares").OnceSingleAsync<Dictionary<string, Placar>>();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }

            if (response == null)
            {
                return Array.Empty<Placar>();
            }

            var itens = response.Where(a => a.Key != null)
                .Select(a => a.Value)
                .OrderBy(a => a.Data)
                .ToArray();

            return await Task.FromResult(itens);
        }

        public void Listen(ObservableCollection<Placar> jogadores)
        {
        }

        public Task<bool> SaveAll(IEnumerable<Placar> items)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(Placar item)
        {
            return await Task.FromResult(true);
        }
    }
}