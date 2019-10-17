using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tifax2.Models;

namespace TIFA.Models
{
    public class Placar : INotifyPropertyChanged
    {


        private string _nomeDesafiante;
        private bool jogadorADesafiante;
        private bool jogadorBDesafiante;

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
        public int? PosicaoAntB { get; set; }
        public int? PosicaoAntA { get; set; }

        public Regra Regra { get; set; }

        public string Origem { get; set; }

        [JsonIgnore]
        public bool JogadorADesafiante
        {
            get
            {

                return string.IsNullOrWhiteSpace(_nomeDesafiante) ? false : _nomeDesafiante == JogadorA?.Nome;
            }
            set {

                var vant = jogadorADesafiante;
                jogadorADesafiante = value;

                if (vant != value && PropertyChanged != null)
                {
                    jogadorBDesafiante = !jogadorADesafiante;

                    if (value)
                    {
                        _nomeDesafiante = JogadorA?.Nome;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(NomeDesafiante)));
                    }

                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorADesafiante)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorBDesafiante)));

                }

            }
        }

        [JsonIgnore]
        public bool JogadorBDesafiante
        {
            get {

                return string.IsNullOrWhiteSpace(_nomeDesafiante) ? false : _nomeDesafiante == JogadorB?.Nome;
            }
            set
            {
                var vant = jogadorBDesafiante;
                jogadorBDesafiante = value;

                if (vant != value && PropertyChanged != null)
                {

                    jogadorADesafiante = !jogadorBDesafiante;

                    if (value)
                    {
                        _nomeDesafiante = JogadorB?.Nome;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(NomeDesafiante)));
                    }

                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorADesafiante)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorBDesafiante)));

                }
            }
        }

        public string NomeDesafiante
        {

            get
            {


                if (JogadorADesafiante) return JogadorA?.Nome;

                if (JogadorBDesafiante) return JogadorB?.Nome;

                return null;

            }                
            set { 

                _nomeDesafiante = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorBDesafiante)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(JogadorADesafiante)));

                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
