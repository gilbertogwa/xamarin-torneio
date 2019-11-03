using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using TIFA.Models;
using TIFA.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Collections.Generic;

namespace TIFA.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class IdentityPage : ContentPage
    {

        private static (string, string)[] _frases = new (string, string)[]
            {
                ( "O primeiro, pode ser o último.", "Mas eu vencerei, sou perdedor!" ),
                ( "Falsidade ideológica é crime sabia?", "E importunar os outros também!" ),
                ( "Tem certeza?", "Sim" ),
                ("Está mentindo?", "Não"),
                ("O poeta inacabado disse:", "Lutar, morrer, sonhar... que sono!"),
                ("Não acredito!", "Acredite!"),
                ("Como vou acreditar num mentiroso?", "Não sou mentiroso!"),
                ("Reconhimento facial ativado...", "Pode ver, ué!"),
                ("Você de novo!", "Quero competir, não posso!?" ),
                ("Que sorte! Você é o último!", "É o 'chips' do Jira" )
            };

        private List<(string, string)> _frasesRestantes = new List<(string, string)>();

        public static IDataStore<Jogador> JogadorDataStore => DependencyService.Get<IDataStore<Jogador>>();
        
        public ObservableCollection<Jogador> Jogadores { get; set; } = new ObservableCollection<Jogador>();

        public IdentityPage()
        {
        
            BindingContext = this;

            InitializeComponent();
            lblFrase.IsVisible = false;
            btnConfirmar.IsVisible = false;
            
            CarregarJogadores();

        }

        private async void CarregarJogadores()
        {
            var jogadores = await JogadorDataStore.GetItemsAsync();

            foreach (var item in jogadores.OrderBy(a=> a.Nome))
            {
                Jogadores.Add(item);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            var jogador = cboJogadores.SelectedItem as Jogador;

            if (jogador == null) 
            {
                return;
            }

            await SecureStorage.SetAsync("user", jogador.Nome);

            await Navigation.PushAsync(new ItemsPage());

        }

        private void cboJogadores_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboJogadores.SelectedItem == null)
            {
                lblFrase.IsVisible = false;
                btnConfirmar.IsVisible = false;
                return;
            } 

            var rnd = new Random();
            
            if (_frasesRestantes.Count == 0)
            {
                _frasesRestantes.AddRange(_frases);
            }

            var sorte = rnd.Next(0, _frasesRestantes.Count - 1);

            lblFrase.Text = _frasesRestantes[sorte].Item1;
            btnConfirmar.Text = _frasesRestantes[sorte].Item2;

            _frasesRestantes.RemoveAt(sorte);

            lblFrase.IsVisible = true;
            btnConfirmar.IsVisible = true;

        }
    }
}