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

                var clas = ((await DataStore.GetItemsAsync(true)) ?? new Classificacao[0])
                    .OrderBy(a => a.Posicao)
                    .ToArray();


                var newItem = item as Placar;

                var clasJA = clas.FirstOrDefault(a => a.Jogador == newItem.JogadorA.Nome);
                var clasJB = clas.FirstOrDefault(a => a.Jogador == newItem.JogadorB.Nome);

                newItem.PosicaoA = (clasJA?.Posicao) ?? 99;
                newItem.PosicaoAntA = (clasJA?.PosicaoAnterior) ;
                newItem.PosicaoB = (clasJB?.Posicao) ?? 99;
                newItem.PosicaoAntB = (clasJB?.PosicaoAnterior);

                await PlacarDataStore.AddItemAsync(newItem);

                RecalcularClassificacaoAsync(clas);

            });

        }

        public async void RecalcularClassificacaoAsync(Classificacao[] clas = null)
        {

            if (clas == null)
            {
                clas = ((await DataStore.GetItemsAsync(true)) ?? new Classificacao[0])
                    .OrderBy(a => a.Posicao)
                    .ToArray();
            }


            if (clas == null && clas.Length == 0) return;

            var ultimaPosicao = clas[clas.Length - 1].Posicao;
            var placares = (await PlacarDataStore.GetItemsAsync(true))?.ToArray();
            var dataMaisAntiga = clas.Select(a => a.Data).OrderBy(a => a).First();

            placares = placares.Where(a => a.Data >= dataMaisAntiga)
                .OrderBy(a=> a.DataPublicacao)
                .ToArray();

            foreach (var placar in placares)
            {
                var clasJogadorA = clas.FirstOrDefault(a => a.Jogador == placar.JogadorA.Nome);
                var clasJogadorB = clas.FirstOrDefault(a => a.Jogador == placar.JogadorB.Nome);

                if (clasJogadorA == null || clasJogadorB == null) continue;
                if (placar.JogadorAGols == null ||  placar.JogadorBGols == null) continue;

                if (placar.JogadorAGols == placar.JogadorBGols) continue;

                clasJogadorA.Posicao = placar.PosicaoA;
                clasJogadorA.PosicaoAnterior = placar.PosicaoAntA;
                clasJogadorB.Posicao = placar.PosicaoB;
                clasJogadorB.PosicaoAnterior = placar.PosicaoAntB;

                if (placar.JogadorAGols > placar.JogadorBGols)
                {
                    AtualizarPosicao(clas, ultimaPosicao, clasJogadorA, clasJogadorB);
                    continue;
                } else
                {
                    AtualizarPosicao(clas, ultimaPosicao, clasJogadorB, clasJogadorA);
                }

            }

            foreach (var item in clas)
            {
                await DataStore.AddItemAsync(item);
            }

            await ExecuteLoadClassificacaoCommand();
        }

        private void AtualizarPosicao(Classificacao[] clas, int ultimaPosicao, Classificacao vencedor, Classificacao derrotado)
        {
            if (vencedor.Posicao < derrotado.Posicao)
            {
                Rebaixar(clas, derrotado, ultimaPosicao);
            }
            else
            {
                TrocarPosicacao(vencedor, derrotado);
            }
        }

        private void Rebaixar(Classificacao[] clas, Classificacao derrotado, int ultimaPosicao)
        {

            if (derrotado.Posicao == ultimaPosicao) return;

            var clasTroca = clas.Where(a => a.Posicao == derrotado.Posicao + 1)
                .FirstOrDefault();

            if (clasTroca == null) return;

            derrotado.PosicaoAnterior = derrotado.Posicao;
            clasTroca.PosicaoAnterior = clasTroca.Posicao;
            derrotado.Posicao = clasTroca.Posicao;
            clasTroca.Posicao = derrotado.PosicaoAnterior.Value;

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