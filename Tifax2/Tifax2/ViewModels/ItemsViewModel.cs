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
using Tifax2.Models;
using TIFA.Util;
using TIFA.Business;

namespace TIFA.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        
        public IEnumerable<Regra> Regras { get; set; } = RegrasBusiness.GetList();

        public static IDataStoreList<Config> ConfigDataStore
                => DependencyService.Get<IDataStoreList<Config>>();


        public static IDataStoreList<ClassificacaoInicial> ClassificacaoInicialStore 
            => DependencyService.Get<IDataStoreList<ClassificacaoInicial>>();
        
        public static IDataStore<Jogador> JogadorDataStore => DependencyService.Get<IDataStore<Jogador>>();

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

            MessagingCenter.Subscribe<NewItemPage, Placar>(this, "AddItem", IncluirPlacar);

        }

        private async void IncluirPlacar(NewItemPage source, Placar placar)
        {

            var clas = ((await DataStore.GetItemsAsync(true)) ?? new Classificacao[0])
                .OrderBy(a => a.Posicao)
                .ToArray();

            var novoPlacar = placar as Placar;

            var clasJA = clas.FirstOrDefault(a => a.Jogador == novoPlacar.JogadorA.Nome);
            var clasJB = clas.FirstOrDefault(a => a.Jogador == novoPlacar.JogadorB.Nome);

            var ultimaPosicao = clas[clas.Length - 1].Posicao;

            novoPlacar.PosicaoA = (clasJA?.Posicao) ?? (++ultimaPosicao);
            novoPlacar.PosicaoAntA = (clasJA?.PosicaoAnterior);
            novoPlacar.PosicaoB = (clasJB?.Posicao) ?? (++ultimaPosicao);
            novoPlacar.PosicaoAntB = (clasJB?.PosicaoAnterior);

            await PlacarDataStore.AddItemAsync(novoPlacar);

            RecalcularClassificacaoAsync(null);


        }
        public async void RecalcularClassificacaoAsync(Classificacao[] clas = null)
        {
            if (clas == null)
            {
                clas = (await ClassificacaoInicialStore.GetItemsAsync())
                    .Select(a => a.Clone())
                    .OrderBy(a => a.Posicao)
                    .ToArray();
            }

            var placares = (await PlacarDataStore.GetItemsAsync(true)).ToArray();

            var busi = new ClassificacaoBusiness();
            var classifs = busi.RecalcularClassificacao(clas, placares);

            var novaLista = classifs.Where(a => a.Excluir == false)
                                    .OrderBy(a => a.Posicao)
                                    .ToArray();

            await ExecuteLoadClassificacaoCommand(novaLista);

            _ = DataStore.SaveAll(clas);

        }


        async Task ExecuteLoadClassificacaoCommand(IEnumerable<Classificacao> clas = null)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                IEnumerable<Classificacao> items;

                if (clas == null)
                {
                    items = await DataStore.GetItemsAsync(true);
                } else
                {
                    items = clas;
                }
                
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