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

namespace TIFA.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();

#if DEBUG
            var button = new Button()
            {
                Text = "Recalcular"
               
            };

            button.Clicked += (a, b) => viewModel.RecalcularClassificacaoAsync();
            container.Children.Insert(0, button);
#endif
        }

        

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(viewModel)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            var p = Navigation.NavigationStack.FirstOrDefault(a => a.GetType() == typeof(AboutPage));

            if (p != null)
            {
                Navigation.RemovePage(p);
            }
        }

        private void ColumnDefinition_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}