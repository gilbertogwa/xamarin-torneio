using Newtonsoft.Json;
using System;
using System.IO;
using TIFA.Business;
using TIFA.Models;
using Tifax2.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Tifax2.Tests
{
    public class UnitTest1
    {

        private static ClassificacaoInicial[] CreateClassificaoInicial()
        {
            return new ClassificacaoInicial[]
            {
                Create("A", 1),
                Create("B", 2),
                Create("C", 3),
                Create("D", 4),
                Create("E", 5)
            };
        }

        [Fact]
        public void Desafio_MassaDeDados()
        {

            var clasIniText = File.ReadAllText("classificacao-inicial.json");
            var placText = File.ReadAllText("placares.json");

            var c = JsonConvert.DeserializeObject<ClassificacaoInicial[]>(clasIniText);
            var p = JsonConvert.DeserializeObject<Placar[]>(placText);

            var bus = new ClassificacaoBusiness();

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.Select(a=> a.Jogador).ToArray();

            //Assert.Equal("C", result[0].Jogador);
            //Assert.Equal("B", result[1].Jogador);
            //Assert.Equal("A", result[2].Jogador);
            //Assert.Equal("D", result[3].Jogador);
            //Assert.Equal("E", result[4].Jogador);


        }


        [Fact]
        public void Desafio_DentePorDente_PosicaoMenor_Vence()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("A", 0, "C", 3, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("C", result[0].Jogador);
            Assert.Equal("B", result[1].Jogador);
            Assert.Equal("A", result[2].Jogador);
            Assert.Equal("D", result[3].Jogador);
            Assert.Equal("E", result[4].Jogador);


        }

        [Fact]
        public void Desafio_DentePorDente_PosicaoMenor_Perde()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("A", 3, "C", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("B", result[1].Jogador);
            Assert.Equal("D", result[2].Jogador);
            Assert.Equal("E", result[3].Jogador);
            Assert.Equal("C", result[4].Jogador);


        }

        [Fact]
        public void Desafio_Lider_PosicaoMenor_Vence()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("A", 0, "B", 3, RegrasBusiness.DESAFIO_LIDER, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("B", result[0].Jogador);
            Assert.Equal("A", result[1].Jogador);
            Assert.Equal("C", result[2].Jogador);
            Assert.Equal("D", result[3].Jogador);
            Assert.Equal("E", result[4].Jogador);


        }

        [Fact]
        public void Desafio_Lider_PosicaoMenor_Perde()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("A", 3, "B", 0, RegrasBusiness.DESAFIO_LIDER, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("C", result[1].Jogador);
            Assert.Equal("D", result[2].Jogador);
            Assert.Equal("E", result[3].Jogador);
            Assert.Equal("B", result[4].Jogador);


        }

        [Fact]
        public void Desafio_DentePorDente_Duas_Partidas_PerdedoresCaem()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("C", 0, "B", 3, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));
            p.Add(CreateP("D", 0, "A", 3, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("B", result[1].Jogador);
            Assert.Equal("C", result[2].Jogador);
            Assert.Equal("E", result[3].Jogador);
            Assert.Equal("D", result[4].Jogador);


        }

        [Fact]
        public void Desafio_DentePorDente_Duas_Partidas_PerdedoresGanham()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("C", 3, "B", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));
            p.Add(CreateP("E", 3, "B", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("C", result[1].Jogador);
            Assert.Equal("E", result[2].Jogador);
            Assert.Equal("D", result[3].Jogador);
            Assert.Equal("B", result[4].Jogador);


        }

        [Fact]
        public void Desafio_Multiplos()
        {

            var bus = new ClassificacaoBusiness();
            
            var c = CreateClassificaoInicial();
            var p = new List<Placar>();

            // RESULTADO: C B A D E
            p.Add(CreateP("A", 0, "C", 3, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            // RESULTADO: C B D A E 
            p.Add(CreateP("A", 0, "B", 1, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            // RESULTADO: C B A D E
            p.Add(CreateP("B", 1, "D", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            // RESULTADO: C B A D E
            p.Add(CreateP("C", 1, "E", 0, RegrasBusiness.DESAFIO_LIDER, p.Count));

            // RESULTADO: C B E D A
            p.Add(CreateP("E", 1, "A", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            // RESULTADO: A B E D C
            p.Add(CreateP("C", 0, "A", 1, RegrasBusiness.DESAFIO_LIDER, p.Count));

            // RESULTADO: A B E D C
            p.Add(CreateP("E", 1, "A", 1, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            // RESULTADO: A B D E C
            p.Add(CreateP("A", 3, "E", 2, RegrasBusiness.PADRAO, p.Count));

            // RESULTADO: A B E C D
            p.Add(CreateP("A", 3, "D", 2, RegrasBusiness.DESAFIO_LIDER, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("B", result[1].Jogador);
            Assert.Equal("E", result[2].Jogador);
            Assert.Equal("C", result[3].Jogador);
            Assert.Equal("D", result[4].Jogador);


        }

        [Fact]
        public void Desafio_EstouFora_UmJogador_AMenos()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("C", 3, "C", 0, RegrasBusiness.ESTOU_FORA, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("B", result[1].Jogador);
            Assert.Equal("D", result[2].Jogador);
            Assert.Equal("E", result[3].Jogador);
            Assert.Equal(c.Length-1, result.Length);


        }

        [Fact]
        public void Desafio_EstouFora_Retorno_Do_Desistente_Perdedor()
        {

            var bus = new ClassificacaoBusiness();


            var c = CreateClassificaoInicial();

            var p = new List<Placar>();

            p.Add(CreateP("C", 3, "C", 0, RegrasBusiness.ESTOU_FORA, p.Count));
            p.Add(CreateP("E", 3, "D", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));
            p.Add(CreateP("C", 3, "B", 0, RegrasBusiness.DESAFIO_DENTE_POR_DENTE, p.Count));

            var clasFinal = bus.RecalcularClassificacao(c, p.ToArray());

            var result = clasFinal.ToArray();

            Assert.Equal(c.Length, result.Length);
            Assert.Equal("A", result[0].Jogador);
            Assert.Equal("C", result[1].Jogador);
            Assert.Equal("E", result[2].Jogador);
            Assert.Equal("D", result[3].Jogador);
            Assert.Equal("B", result[4].Jogador);


        }

        private static ClassificacaoInicial Create(string jogador, int posicao)
        {
            return new ClassificacaoInicial()
            {
                Data = DateTime.Now.Date, 
                Jogador = jogador,
                Posicao = posicao
            };
        }

        private static Placar CreateP(string jogadorA, int g1, string jogadorB, int g2, string regra, double seq)
        {
            return new Placar()
            {
                Data = DateTime.Now.Date,
                DataPublicacao = DateTime.Now.AddSeconds(seq),
                JogadorA = new Jogador() { Nome = jogadorA},
                JogadorB = new Jogador() { Nome = jogadorB },
                 JogadorAGols = g1, JogadorBGols = g2, 
                 Regra = new Models.Regra() {  Nome = regra}
            };
        }


    }
}
