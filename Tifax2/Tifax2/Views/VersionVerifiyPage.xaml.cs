using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TIFA.Models;
using TIFA.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TIFA.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class VersionVerifiyPage : ContentPage
    {

        public const string VERSAO_DATABASE = "7";

        public VersionVerifiyPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            await VerificarVersaoAsync();
            base.OnAppearing();

            Plugin.PushNotification.CrossPushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
        }

        public async Task VerificarVersaoAsync()
        {

            var store = DependencyService.Get<IDataStore<Config>>();
            var config = await store.GetItemAsync(null);

            if (config.Versao != VERSAO_DATABASE)
            {
                await DisplayAlert("Atenção", "ESTA VERSÃO NÃO É MAIS SUPORTADA. ATUALIZE SEU APLICATIVO!", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            Config.Current = config;

            await Navigation.PushAsync(new ItemsPage());
        }

    }
}