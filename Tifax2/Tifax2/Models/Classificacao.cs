﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TIFA.Util;

namespace TIFA.Models
{

    public class Classificacao: IPosicaoChave
    {

        public Classificacao()
        {
            Data = DateTime.Now.Date;
        }

        public DateTime Data { get; set; }

        public int Posicao { get; set; }

        public string Jogador { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Classificacao jogador)) return false;

            return (jogador.Jogador == Jogador);
        }

        public override int GetHashCode()
        {
            return 1156687923 + EqualityComparer<string>.Default.GetHashCode(Jogador);
        }

        public string GetKey()
        {
            return Jogador;
        }

        public int? PosicaoAnterior { get; set; }

        internal bool EhLider { get => Posicao == 1; }

        internal bool? EhTendenciaAlta
        {
            get
            {
                if (PosicaoAnterior == null || Posicao == PosicaoAnterior) return null;
                return (Posicao < PosicaoAnterior);
            }
        }

        public int? TotalVitorias { get; set; }

        public int? TotalDerrotas { get; set; }

        public int? TotalEmpates { get; set; }

        public int? TotalGolsFeitos { get; set; }

        public int? TotalGolsTomados { get; set; }


        [JsonIgnore]
        public string TipoIcone
        {
            get
            {
                if (EhLider) return "lider";

                if (EhTendenciaAlta == true) return "subiu";

                if (EhTendenciaAlta == false) return "caiu";

                return "igual";

            }
        }

        [JsonIgnore]
        public bool Alterado { get; set; }

        public int TotalJogos { get; set; }

        [JsonIgnore]
        public bool Excluir { get; set; }

    }

}
