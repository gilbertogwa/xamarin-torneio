using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIFA.Models;
using TIFA.Util;
using Tifax2.Models;

namespace TIFA.Business
{
    public class ClassificacaoBusiness
    {

        public  IEnumerable<Classificacao> RecalcularClassificacao(Classificacao[] clas, Placar[] placares)
        {

            var classifs = new PosicaoList<Classificacao>();

            classifs.InsertRange(clas, item => item.Posicao - 1);

            var dataMaisAntiga = classifs.Select(a => a.Data).OrderBy(a => a).First();

            placares = placares.Where(a => a.Data >= dataMaisAntiga)
                .OrderBy(a => a.Data)
                .ThenBy(a => a.DataPublicacao)
                .ToArray();

            foreach (var placar in placares)
            {

                if (placar.JogadorAGols == null || placar.JogadorBGols == null) continue;

                var clasJogadorA = classifs[placar.JogadorA.Nome];
                var clasJogadorB = classifs[placar.JogadorB.Nome];

                VerificarClassificacao(classifs, placar.JogadorA.Nome, clasJogadorA);
                VerificarClassificacao(classifs, placar.JogadorB.Nome, clasJogadorB);

                var golsA = placar.JogadorAGols ?? 0;
                var golsB = placar.JogadorBGols ?? 0;

                clasJogadorA.Data.Alterado = true;
                clasJogadorB.Data.Alterado = true;

                if (placar.Regra.Nome != RegrasBusiness.ESTOU_FORA && (string.IsNullOrWhiteSpace(placar.Origem) == true || placar.Origem == "app"))
                {
                    if (golsA == golsB)
                    {
                        AtualizarEstatistica(clasJogadorA.Data, 0, 1, 0, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB.Data, 0, 1, 0, golsB, golsA);
                        continue;
                    }
                    else if (golsA > golsB)
                    {
                        AtualizarEstatistica(clasJogadorA.Data, 1, 0, 0, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB.Data, 0, 0, 1, golsB, golsA);
                    }
                    else
                    {
                        AtualizarEstatistica(clasJogadorA.Data, 0, 0, 1, golsA, golsB);
                        AtualizarEstatistica(clasJogadorB.Data, 1, 0, 0, golsB, golsA);
                    }
                }

                if (placar.JogadorAGols > placar.JogadorBGols)
                {
                    AtualizarPosicao(classifs,  clasJogadorA, clasJogadorB, placar);
                }
                else
                {
                    AtualizarPosicao(classifs, clasJogadorB, clasJogadorA, placar);
                }

            }

            foreach (var item in classifs.GetPosicoes())
            {
                var jogador = item.Data;
                jogador.PosicaoAnterior = jogador.Posicao;
                jogador.Posicao = item.Index + 1;
            }

            return classifs;

        }

        private static void VerificarClassificacao(PosicaoList<Classificacao> lista, string jogador, 
            Posicao<Classificacao> posicao)
        {
            if (posicao.Data == null)
            {
                posicao.Data = new Classificacao() { 
                    Jogador = jogador,
                 Alterado =true, Data = DateTime.Now, };
                lista[posicao.Index] = posicao.Data;
            }
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

        private int? SeZeroNull(int? valor)
        {
            if (valor == 0) return null;
            return valor;
        }

        private void AtualizarPosicao(PosicaoList<Classificacao> clas, 
            Posicao<Classificacao> vencedor, Posicao<Classificacao> derrotado, Placar placar)
        {

            int qtdeQueda;
            bool trocaPosicao;
            var regra = placar.Regra;

            if (regra.Nome == RegrasBusiness.ESTOU_FORA)
            {
                if (vencedor.Data.Jogador != derrotado.Data.Jogador)
                {
                    var msg = "Quando um jogador desiste, a partida deve ser entre ele mesmo.";
                    msg += $"\r\n[ Data: {placar.Data} - '{vencedor.Data.Jogador}' x '{derrotado.Data.Jogador}']";
                    throw new InvalidOperationException(msg);
                }
                clas.RemoveByKey(derrotado.Data.Jogador);
                return;
            }

            switch (regra.Nome)
            {
                case RegrasBusiness.DESAFIO_DENTE_POR_DENTE:
                    trocaPosicao = vencedor.Index > derrotado.Index;
                    qtdeQueda = Math.Abs(vencedor.Index - derrotado.Index);
                    break;
                case RegrasBusiness.DESAFIO_LIDER:

                    if (vencedor.Index != 0 && derrotado.Index != 0)
                    {
                        var msg = $"O desafio entre '{vencedor.Data.Jogador}' e "
                            + $"'{derrotado.Data.Jogador}' é inválido. "
                            + "Nenhum dos dois possui a liderança.";
                        msg += $"\r\n[ Data: {placar.Data} - '{vencedor.Data.Jogador}' x '{derrotado.Data.Jogador}']";
                        throw new InvalidOperationException(msg);
                    }

                    trocaPosicao = (vencedor.Index != 0);
                    qtdeQueda = Math.Abs(derrotado.Index - (clas.Count - 1));

                    break;
                default:
                    trocaPosicao = vencedor.Index > derrotado.Index;
                    qtdeQueda = 1;
                    break;
            }

            if (trocaPosicao)
            {
                var (posDerrotado, posVencedor) = (vencedor.Index, derrotado.Index);

                clas[posDerrotado] = derrotado.Data;
                clas[posVencedor] = vencedor.Data;
                return;
            }

            clas.Remove(derrotado.Index);
            clas.InsertOrUpdate(derrotado.Index + qtdeQueda, derrotado.Data);


        }
      
    }
}
