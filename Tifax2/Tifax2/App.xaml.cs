using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TIFA.Services;
using TIFA.Views;
using System.Collections.Generic;
using TIFA.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Microsoft.AppCenter.Distribute;

namespace TIFA
{
    public partial class App : Application
    {


        public App()
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            //InitClassificacao();
            //InitJogadores();
            //InitPlacares();
            
            MainPage = new MainPage();
        }

        private void InitJogadores()
        {

            var items = new List<Jogador>()
            {
                new Jogador{Id="1", Nome= "Jira"     },
                new Jogador{Id="2",  Nome= "Felipe"   },
                new Jogador{Id="3",  Nome= "Carielli"  },
                new Jogador{Id="4",  Nome= "Érico"    },
                new Jogador{Id="5",  Nome= "Bahia"    },
                new Jogador{Id="6",  Nome= "Neto"     },
                new Jogador{Id="7",  Nome= "Cabo"     },
                new Jogador{Id="8",  Nome= "Little"   },
                new Jogador{Id="9",  Nome= "Gustavo"  },
                new Jogador{Id="10",  Nome="Joazinho" },
                new Jogador{Id="11",  Nome="Juiz"      }
            };

            var f = DependencyService.Get<IDataStore<Jogador>>();


            foreach (var item in items)
            {
                f.AddItemAsync(item);
            }
        }

        private void InitPlacares()
        {

            var items = new List<Placar>()
            {
                new Placar(){ 
                    Data = new DateTime(2019,09,20,19,39,00), 
                    JogadorA = new Jogador(){  Id = "10", Nome = "Joaozinho"}, 
                    JogadorAGols = 2,
                    JogadorB = new Jogador(){  Id = "11", Nome = "Juiz"},
                    JogadorBGols = 0
                },
                new Placar(){
                    Data = new DateTime(2019,09,20,19,39,00),
                    JogadorA = new Jogador(){  Id = "8", Nome = "Little"},
                    JogadorAGols = 3,
                    JogadorB = new Jogador(){  Id = "10", Nome = "Joaozinho"},
                    JogadorBGols = 0
                },
                new Placar(){
                    Data = new DateTime(2019,09,20,19,39,00),
                    JogadorA = new Jogador(){  Id = "9", Nome = "Gustavo"},
                    JogadorAGols = 2,
                    JogadorB = new Jogador(){  Id = "11", Nome = "Juiz"},
                    JogadorBGols = 0
                },
            };

            var f = DependencyService.Get<IDataStore<Placar>>();


            foreach (var item in items)
            {
                f.AddItemAsync(item);
            }
        }

        private void InitClassificacao()
        {

            var items = new List<Classificacao>()
            {
                new Classificacao{ Posicao = 1 ,Jogador= "Jira"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 2 ,Jogador= "Felipe"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 3 ,Jogador= "Carielli"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 4 ,Jogador= "Érico"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 5 ,Jogador= "Bahia"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 6 ,Jogador= "Neto"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 7 ,Jogador= "Cabo"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 8 ,Jogador= "Little"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 10,Jogador="Joazinho"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 9 ,Jogador= "Gustavo"  , PosicaoAnterior=null},
                new Classificacao{ Posicao = 11,Jogador="Juiz"  , PosicaoAnterior=null}
            };

            var f = DependencyService.Get<IDataStore<Classificacao>>();


            foreach (var item in items)
            {
                f.AddItemAsync(item);
            }
        }


        protected override void OnStart()
        {
            AppCenter.Start("android=5205bff7-d8ce-4e36-8c4b-2369d18d7516;",
                  typeof(Analytics), typeof(Crashes), typeof(Distribute));
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
