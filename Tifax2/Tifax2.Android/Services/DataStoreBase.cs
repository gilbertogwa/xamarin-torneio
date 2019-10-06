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
using Firebase.Database;

namespace TIFA.Droid.Services
{
    public class DataStoreBase
    {
        private const string URL_DB = "https://gibis-tifa.firebaseio.com/";

#if DEBUG
        private const string NAME_DB = "teste";
#else
        private const string NAME_DB = "producao";
#endif

        protected readonly FirebaseClient _database;

        public DataStoreBase()
        {

            _database = new FirebaseClient($"{URL_DB}{NAME_DB}");
        }

    }
}