using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using TIFA.Models;
using TIFA.ViewModels;
using Tifax2.Models;

namespace TIFA.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public ItemsViewModel ViewModel { get;  }

        public Placar Item { get; set; }

        public NewItemPage(ItemsViewModel viewModel)
        {
            InitializeComponent();

            Item = new Placar
            {
                Origem = "app"
            };

            if (Config.Current != null)
            {
                Item.Regra = viewModel.Regras.FirstOrDefault(a => a.Nome == Config.Current.RegraDefault);
            }
            
            ViewModel = viewModel;

            BindingContext = this;

        }

        async void Save_Clicked(object sender, EventArgs e)
        {

            if (Item.JogadorA == null)
            {
                await DisplayAlert("Atenção", "Selectione um jogador!", "Ok");
                return;
            } 

            if (Item.Regra.Nome != RegrasBusiness.ESTOU_FORA)
            {
                if (Item.JogadorA == null || Item.JogadorB == null ||
                    Item.JogadorAGols == null || Item.JogadorBGols == null)
                {
                    await DisplayAlert("Atenção", "Todos os campos são obrigatórios", "Ok");
                    return;
                }
            } else
            {

                var nome = Item.JogadorA.Nome;
                var resp = await DisplayAlert("Tem certeza?", $"'{nome}' será retirado do jogo. É isso mesmo?", "Sim", "Não");

                if (resp == false) return;

                // Na regra "Estou fora!" o jogador A deve ser igual ao jogador B
                Item.JogadorB = Item.JogadorA;
                Item.JogadorAGols = 0;
                Item.JogadorBGols = 1;

            }

            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();

        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}