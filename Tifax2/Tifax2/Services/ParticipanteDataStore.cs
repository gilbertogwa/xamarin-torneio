using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TIFA.Models;

namespace TIFA.Services
{
    public class ParticipanteDataStore : IDataStore<Jogador>
    {
        readonly List<Jogador> items;

        public ParticipanteDataStore()
        {
            items = new List<Jogador>()
            {
                new Jogador{ Nome ="Giba" }    ,
                new Jogador{ Nome ="Felipe" } ,
                new Jogador{ Nome ="Carielo" } ,
                new Jogador{ Nome ="Paulo" } ,
            };
        }

        public async Task<bool> AddItemAsync(Jogador item)
        {
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Jogador item)
        {
           // var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
          //  items.Remove(oldItem);
          //  items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
           // items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Jogador> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Jogador>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public void Listen(ObservableCollection<Jogador> jogadores)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAll(IEnumerable<Jogador> items)
        {
            throw new NotImplementedException();
        }
    }
}