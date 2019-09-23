using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

using TIFA.Models;
using TIFA.Views;
using TIFA.Services;
using System.Collections.Generic;

namespace TIFA.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public IDataStore<Jogador> JogadorDataStore => DependencyService.Get<IDataStore<Jogador>>();

        public IDataStore<Placar> PlacarDataStore => DependencyService.Get<IDataStore<Placar>>();

        public ObservableCollection<Classificacao> Items { get; set; }

        public ObservableCollection<Jogador> Jogadores { get; set; }

        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {

            Title = "Classificação";
            Items = new ObservableCollection<Classificacao>();
            Jogadores = new ObservableCollection<Jogador>();
            LoadItemsCommand = new Command(async () =>
            {
                await ExecuteLoadJogadoresCommand();
                await ExecuteLoadClassificacaoCommand();                
            });

            MessagingCenter.Subscribe<NewItemPage, Placar>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Placar;
                await PlacarDataStore.AddItemAsync(newItem);

                RecalcularClassificacaoAsync();

            });

        }

        private async void RecalcularClassificacaoAsync()
        {

            var clas = (await DataStore.GetItemsAsync(true))?.ToArray();

            if (clas == null && clas.Length == 0) return;

            var placares = (await PlacarDataStore.GetItemsAsync(true))?.ToArray();
            var dataMaisAntiga = clas.Select(a => a.Data).OrderBy(a => a).First();

            placares = placares.Where(a => a.Data >= dataMaisAntiga).ToArray();

            foreach (var placar in placares)
            {
                var clasJogadorA = clas.FirstOrDefault(a => a.Jogador == placar.JogadorA.Nome);
                var clasJogadorB = clas.FirstOrDefault(a => a.Jogador == placar.JogadorB.Nome);

                if (clasJogadorA == null || clasJogadorB == null) continue;
                if (placar.JogadorAGols == null ||  placar.JogadorBGols == null) continue;

                if (placar.JogadorAGols > placar.JogadorBGols)
                {
                    if (clasJogadorA.Posicao < clasJogadorB.Posicao) continue;
                    TrocarPosicacao(clasJogadorA, clasJogadorB);
                }
                else
                {
                    if (clasJogadorB.Posicao < clasJogadorA.Posicao) continue;
                    TrocarPosicacao(clasJogadorB, clasJogadorA);
                }

            }

            foreach (var item in clas)
            {
                await DataStore.AddItemAsync(item);
            }

            await ExecuteLoadClassificacaoCommand();
        }

        private static void TrocarPosicacao(Classificacao clasJogadorA, Classificacao clasJogadorB)
        {
            clasJogadorA.PosicaoAnterior = clasJogadorA.Posicao;
            clasJogadorA.Posicao = clasJogadorB.Posicao;

            clasJogadorB.PosicaoAnterior = clasJogadorB.Posicao;
            clasJogadorB.Posicao = clasJogadorA.PosicaoAnterior ?? 99;
        }

        async Task ExecuteLoadClassificacaoCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadJogadoresCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Jogadores.Clear();
                var items = await JogadorDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Jogadores.Add(item);
                }
                JogadorDataStore.Listen(Jogadores);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}