using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TIFA.Models;
using TIFA.Views;
using TIFA.ViewModels;
using Xamarin.Essentials;

namespace TIFA.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        readonly ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();

#if DEBUG
            var button = new Button()
            {
                Text = "Recalcular"
               
            };

            button.Clicked += (a, b) =>
            {

                 viewModel.RecalcularClassificacaoAsync();
                //await Plugin.PushNotification.CrossPushNotification.Current.RegisterForPushNotifications();
                //var x = Plugin.PushNotification.CrossPushNotification.Current.Token;
            };
            

            container.Children.Insert(0, button);
#endif
        }

        

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Item item))
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(viewModel)));
        }

        protected async override void OnAppearing()
        {

            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            var p = Navigation.NavigationStack.FirstOrDefault(a => a.GetType() == typeof(VersionVerifiyPage)
            || a.GetType() == typeof(IdentityPage));

            if (p != null)
            {
                Navigation.RemovePage(p);
            }

            var user = await SecureStorage.GetAsync("user");

            if (string.IsNullOrWhiteSpace(user) == false)
            {
                lblNome.Text = user;
                painelIdentificacao.IsVisible = true;
            }

        }

        private void ColumnDefinition_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}