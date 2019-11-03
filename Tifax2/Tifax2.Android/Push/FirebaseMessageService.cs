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
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    class FirebaseMessageService : FirebaseMessagingService
    {

        public override void OnMessageReceived(RemoteMessage message)
        {
            var helper = new NotificationHelper();
            var noficationInfo = message.GetNotification();
            helper.CreateNotification(noficationInfo.Title, noficationInfo.Body);
            base.OnMessageReceived(message);
        }

    }
}