using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TIFA.Models;

namespace TIFA.Services
{
    public class MockDataStore : IDataStore<Classificacao>
    {
        readonly List<Classificacao> items;

        public MockDataStore()
        {
            items = new List<Classificacao>()
            {
                new Classificacao{ Posicao = 1 ,Jogador= "Giba"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 2 ,Jogador= "Felipe"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 3 ,Jogador= "Carielo"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 4 ,Jogador= "Érico"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 5 ,Jogador= "Bahia"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 6 ,Jogador= "Neto"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 7 ,Jogador= "Cabo"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 8 ,Jogador= "Little"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 9 ,Jogador= "Gustavo"  , PosicaoAnterior=10},
                new Classificacao{ Posicao = 10,Jogador="Joazinho"  , PosicaoAnterior=9},
                new Classificacao{ Posicao = 11,Jogador="Juiz"  , PosicaoAnterior=null}
            };
        }

        public async Task<bool> AddItemAsync(Classificacao item)
        {
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Classificacao item)
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

        public async Task<Classificacao> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Jogador == id));
        }

        public async Task<IEnumerable<Classificacao>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public void Listen(ObservableCollection<Classificacao> jogadores)
        {
            throw new NotImplementedException();
        }
    }
}