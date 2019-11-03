using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;

namespace TIFA.Droid.Push
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseIdService : FirebaseMessagingService
    {
        
        const string TAG = "FirebaseIdService";

        public override void OnNewToken(string token)
        {
            SendRegistrationToServer(token);
            base.OnNewToken(token);
        }

        private void SendRegistrationToServer(string token)
        {
            var toktok = token;
        }

    }
}