using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using TIFA.Droid.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;

namespace TIFA.Droid
{
    [Activity(Label = "TIFA", Icon = "@mipmap/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation       
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            RegisterDI();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Distribute.SetEnabledForDebuggableBuild(true);


            LoadApplication(new App());

        }

        private void RegisterDI()
        {
            DependencyService.Register<ClassificacaoDataStore>();
            DependencyService.Register<JogadoresDataStore>();
            DependencyService.Register<PlacarDataStore>();
            DependencyService.Register<ClassificacaoInicialDataStore>();
            DependencyService.Register<ConfigDataStore>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}