using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TIFA.Models
{

    public class Classificacao
    {

        public Classificacao()
        {
            Data = DateTime.Now.Date;
        }

        public DateTime Data { get; set; }

        public int Posicao { get; set; }

        public string Jogador { get; set; }


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

        [JsonIgnore]
        public string TipoIcone
        {
            get
            {
                if (EhLider) return "lider";

                if (EhTendenciaAlta == true) return "subiu";

                if (EhTendenciaAlta == false) return "caiu";

                return "";

            }
        }

    }

}
