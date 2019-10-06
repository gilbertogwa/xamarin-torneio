using System;
using System.Collections.Generic;
using System.Text;
using TIFA.Models;
using Tifax2.Models;

namespace Tifax2.Models
{
    class RegrasBusiness
    {

        public const string PADRAO = "DesafioPadrão";
        public const string DESAFIO_LIDER = "DesafioDoLíder";
        public const string DESAFIO_DENTE_POR_DENTE = "DesafioDentePorDente";
        //public const string AMISTOSO = "Amistoso";

        public static IEnumerable<Regra> GetList()
        {

            var regras = new Regra[]
            {
                Create(PADRAO, "VITÓRIA DO INFERIOR: Troque de posição com o derrotado \nDERROTA DO INFERIOR: Perca uma posição \nVITÓRIA DO SUPERIOR: Mantenha posição \nEMPATE: Desafio de merda"),
                Create(DESAFIO_LIDER, "VITÓRIA DO INFERIOR: Ganhe a posição do líder \nDERROTA DO INFERIOR: Você é um lixo! Vá para última posição \nEMPATE: Você merece ser líder?"),
                Create(DESAFIO_DENTE_POR_DENTE, "VITÓRIA DO INFERIOR: Troque de posição com o derrotado \nDERROTA DO INFERIOR: Dois pesos duas medidas, perderá a diferença de posições de quem desafiou \nVITÓRIA DO SUPERIOR: Mantenha posição \nEMPATE: Pra que todo o estresse!?"),
                //Create(AMISTOSO, "Assim é fácil, não tem pressão."),
            };

            return regras;

        }

        private static Regra Create(string nome, string descricao)
        {
            return new Regra() { Nome = nome, Descricao = descricao };
        }

    }
}
