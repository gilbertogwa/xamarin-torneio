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

        private static readonly List<Type>  services = new List<Type>();

        static App()
        {
            services.Add(typeof(Analytics));
            services.Add(typeof(Crashes));
            services.Add(typeof(Distribute));
        }

        public App()
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();           
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
           
            AppCenter.Start("android=5205bff7-d8ce-4e36-8c4b-2369d18d7516;", services.ToArray());
            
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
