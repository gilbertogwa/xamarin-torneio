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
    public class ClassificacaoInicialDataStore : DataStoreBase, IDataStoreList<ClassificacaoInicial>
    {
        private static ClassificacaoInicial[] _itens;

        public async Task<IEnumerable<ClassificacaoInicial>> GetItemsAsync(bool force = false)
        {

            if (_itens != null && _itens.Length > 0)
            {
                return await Task.FromResult(_itens);
            }

            var response = await _database.Child("classificacaoInicial").OnceSingleAsync<Dictionary<string, ClassificacaoInicial>>();

            var itens = response.Where(a=> a.Key != null)
                .Select(a=> a.Value)
                .OrderBy(a => a.Posicao)
                .ToArray();

            _itens = itens;
            return await Task.FromResult(itens);
        }

    }
}