using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TIFA.Services;
using TIFA.Views;
using System.Collections.Generic;
using TIFA.Models;
using Microsoft.AppCenter;
using Xamarin.Essentials;
//using Plugin.PushNotification;

namespace TIFA
{
    public partial class App : Application
    {

        private static readonly List<Type>  services = new List<Type>();

        static App()
        {
            services.Add(typeof(Microsoft.AppCenter.Analytics.Analytics));
            services.Add(typeof(Microsoft.AppCenter.Crashes.Crashes));
            services.Add(typeof(Microsoft.AppCenter.Distribute.Distribute));

        }

        public App()
        {
            InitializeComponent();
            Xamarin.Forms.DataGrid.DataGridComponent.Init();           
            MainPage = new MainPage();

            //CrossPushNotification.Current.OnTokenRefresh += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            //};

            //CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
            //{

            //    System.Diagnostics.Debug.WriteLine("Received");

            //};

            //CrossPushNotification.Current.OnNotificationOpened += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Opened");
            //    foreach (var data in p.Data)
            //    {
            //        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
            //    }


            //};

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
