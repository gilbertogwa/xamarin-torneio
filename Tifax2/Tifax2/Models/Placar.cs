using System;
using System.Collections.Generic;
using System.Text;
using Tifax2.Models;

namespace TIFA.Models
{
    public class Placar
    {

        public Placar()
        {
            Id = Guid.NewGuid().ToString();
            Data = DateTime.Now.Date;
            DataPublicacao = DateTime.Now;
        }

        public string Id { get; set; }

        public DateTime Data { get; set; }

        public Jogador JogadorA { get; set; }

        public int? JogadorAGols { get; set; }

        public Jogador JogadorB { get; set; }

        public int? JogadorBGols { get; set; }

        public DateTime DataPublicacao { get; set; }

        public int PosicaoA { get; set; }

        public int PosicaoB { get; set; }
        public int? PosicaoAntB { get;  set; }
        public int? PosicaoAntA { get;  set; }

        public Regra Regra { get; set; }

        public string Origem { get; set; }
    }
}
