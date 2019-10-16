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

            MessagingCenter.Subscribe<NewItemPage, Placar>(this, "AddItem", async (obj, item) =>
            {

                var clas = ((await DataStore.GetItemsAsync(true)) ?? new Classificacao[0])
                    .OrderBy(a => a.Posicao)
                    .ToArray();


                var novoPlacar = item as Placar;

                var clasJA = clas.FirstOrDefault(a => a.Jogador == novoPlacar.JogadorA.Nome);
                var clasJB = clas.FirstOrDefault(a => a.Jogador == novoPlacar.JogadorB.Nome);

                //if (novoPlacar.Regra.Nome == RegrasBusiness.AMISTOSO)
                //{
                //    await PlacarDataStore.AddItemAsync(novoPlacar);
                //    return;
                //}

                var ultimaPosicao = clas[clas.Length - 1].Posicao;

                novoPlacar.PosicaoA = (clasJA?.Posicao) ?? (++ultimaPosicao);
                novoPlacar.PosicaoAntA = (clasJA?.PosicaoAnterior) ;
                novoPlacar.PosicaoB = (clasJB?.Posicao) ?? (++ultimaPosicao);
                novoPlacar.PosicaoAntB = (clasJB?.PosicaoAnterior);

                await PlacarDataStore.AddItemAsync(novoPlacar);

                RecalcularClassificacaoAsync(null);

            });

        }

        public async void RecalcularClassificacaoAsync(Classificacao[] clas = null)
        {

            var classifs = new List<Classificacao>();

            if (clas == null)
            {
                clas = (await ClassificacaoInicialStore.GetItemsAsync())
                    .Select(a => a.Clone())
                    .OrderBy(a => a.Posicao)
                    .ToArray();
            }

            classifs.AddRange(clas);

            var ultimaPosicao = clas[clas.Length - 1].Posicao;
            var placares = (await PlacarDataStore.GetItemsAsync(true)).ToArray();
            var dataMaisAntiga = clas.Select(a => a.Data).OrderBy(a => a).First();

            placares = placares.Where(a => a.Data >= dataMaisAntiga)
                .OrderBy(a => a.DataPublicacao)
                .ToArray();
            
            foreach (var placar in placares)
            {

                if (placar.JogadorAGols == null || placar.JogadorBGols == null) continue;

                var clasJogadorA = GetClassificaoJogador(classifs, placar.JogadorA, ref ultimaPosicao);
                var clasJogadorB = GetClassificaoJogador(classifs, placar.JogadorB, ref ultimaPosicao);

                var golsA = placar.JogadorAGols ?? 0;
                var golsB = placar.JogadorBGols ?? 0;

                clasJogadorA.Alterado = true;
                clasJogadorB.Alterado = true;

                if (string.IsNullOrWhiteSpace(placar.Origem) == true || placar.Origem == "app")
                {
                    if (golsA == golsB)
                    {
                        AtualizarEstatistica(clasJogadorA, 0, 1, 0, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB, 0, 1, 0, golsB, golsA);
                        continue;
                    }
                    else if (golsA > golsB)
                    {
                        AtualizarEstatistica(clasJogadorA, 1, 0, 0, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB, 0, 0, 1, golsB, golsA);
                    }
                    else
                    {
                        AtualizarEstatistica(clasJogadorA, 0, 0, 1, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB, 1, 0, 0, golsB, golsA);
                    }
                }

                clasJogadorA.Posicao = placar.PosicaoA;
                clasJogadorA.PosicaoAnterior = placar.PosicaoAntA;
                clasJogadorB.Posicao = placar.PosicaoB;
                clasJogadorB.PosicaoAnterior = placar.PosicaoAntB;

                if (placar.JogadorAGols > placar.JogadorBGols)
                {
                    AtualizarPosicao(classifs, ultimaPosicao, clasJogadorA, clasJogadorB, placar.Regra);
                    continue;
                }
                else
                {
                    AtualizarPosicao(classifs, ultimaPosicao, clasJogadorB, clasJogadorA, placar.Regra);
                }

            }

            await SalvarAlteracoesAsync(classifs);
            await ExecuteLoadClassificacaoCommand(classifs.OrderBy(a => a.Posicao)
                                                    .ToArray());
        }

        private static Classificacao GetClassificaoJogador(List<Classificacao> clas, Jogador jogador, ref int ultimaPosicao)
        {
            var clasJogador = clas.FirstOrDefault(a => a.Jogador == jogador.Nome);

            if (clasJogador == null)
            {
                clasJogador = new Classificacao()
                {
                    Jogador = jogador.Nome,
                    Alterado = true,
                    Posicao = (++ultimaPosicao)
                };

                clas.Add(clasJogador);
            }

            return clasJogador;
        }

        private void AtualizarEstatistica(Classificacao classificacao, int vitoria, int empate, 
            int derrota, int golFeito, int golTomado)
        {
            classificacao.TotalJogos += 1;
            classificacao.TotalVitorias = SeZeroNull((classificacao.TotalVitorias ?? 0) + vitoria);
            classificacao.TotalDerrotas = SeZeroNull((classificacao.TotalDerrotas ?? 0) + derrota);
            classificacao.TotalEmpates = SeZeroNull((classificacao.TotalEmpates ?? 0) + empate);
            classificacao.TotalGolsFeitos = SeZeroNull((classificacao.TotalGolsFeitos ?? 0) + golFeito);
            classificacao.TotalGolsTomados = SeZeroNull((classificacao.TotalGolsTomados ?? 0) + golTomado);
        }

        private void ZerarEstatistica(Classificacao classificacao)
        {
            classificacao.TotalJogos = 0;
            classificacao.TotalVitorias = 0;
            classificacao.TotalDerrotas = 0;
            classificacao.TotalEmpates = 0;
            classificacao.TotalGolsFeitos = 0;
            classificacao.TotalGolsTomados = 0;
        }

        private int? SeZeroNull(int? valor)
        {
            if (valor == 0) return null;
            return valor;
        }

        private async Task SalvarAlteracoesAsync(IEnumerable<Classificacao> clas)
        {
            foreach (var item in clas)
            {
                if (item.Alterado == false) continue;
                await DataStore.AddItemAsync(item);
                item.Alterado = false;
            }
        }

        private void AtualizarPosicao(IEnumerable<Classificacao> clas, int ultimaPosicao, 
            Classificacao vencedor, 
            Classificacao derrotado, Regra regra)
        {
            int qtdeQueda;

            switch (regra.Nome)
            {
                case RegrasBusiness.DESAFIO_DENTE_POR_DENTE:
                    qtdeQueda = Math.Abs(vencedor.Posicao - derrotado.Posicao);
                    break;
                case RegrasBusiness.DESAFIO_LIDER:
                    if (derrotado.Posicao == 1)
                    {
                        qtdeQueda = Math.Abs(vencedor.Posicao - derrotado.Posicao);
                    } else if (vencedor.Posicao == 1)
                    {
                        qtdeQueda = Math.Abs(derrotado.Posicao - ultimaPosicao);
                    }
                    else
                    {
                        qtdeQueda = 1;
                    }
                    break;
                //case RegrasBusiness.AMISTOSO:
                //    qtdeQueda = 0;
                //    break;
                default:
                    qtdeQueda = 1;
                    break;
            }

            if (vencedor.Posicao < derrotado.Posicao)
            {
                Rebaixar(clas, derrotado, ultimaPosicao, qtdeQueda);
            }
            else
            {
                TrocarPosicao(vencedor, derrotado);
            }

        }

        private void Rebaixar(IEnumerable<Classificacao> clas, Classificacao derrotado, int ultimaPosicao, int qt = 1)
        {

            if (derrotado.Posicao == ultimaPosicao) return;

            var promovidos = clas.Where(a => a.Posicao > derrotado.Posicao && a.Posicao <= derrotado.Posicao + qt)
                .OrderBy(a => a.Posicao)
                .ToArray();

            derrotado.PosicaoAnterior = derrotado.Posicao;
            derrotado.Posicao += qt;
            derrotado.Alterado = true;

            foreach (var item in promovidos)
            {
                item.PosicaoAnterior = item.Posicao;
                item.Posicao -= 1;
                item.Alterado = true;
            }

        }

        private static void TrocarPosicao(Classificacao clasJogadorA, Classificacao clasJogadorB)
        {
            clasJogadorA.PosicaoAnterior = clasJogadorA.Posicao;
            clasJogadorA.Posicao = clasJogadorB.Posicao;
            clasJogadorA.Alterado = true;

            clasJogadorB.PosicaoAnterior = clasJogadorB.Posicao;
            clasJogadorB.Posicao = clasJogadorA.PosicaoAnterior ?? 99;
            clasJogadorB.Alterado = true;
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