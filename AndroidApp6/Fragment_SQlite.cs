using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.AppCompat.Widget;
using Mono.Data.Sqlite;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Android.Support.V4.App;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using System.Data;
using AndroidApp4.Models;
using System.IO;
using static Android.Provider.ContactsContract.CommonDataKinds;
using Dalvik.SystemInterop;
using AndroidX.Fragment.App;

namespace AndroidApp4.Fragment
{
    [Obsolete]
    public class Fragment_SQlite : AndroidX.Fragment.App.Fragment
    {
        List<User> _Data;
        MyAdapter myAdapter;
        ListView listView1;
        Button create_btn;
        Button load_btn;
        Button add_btn;
        public static string _ConnectionString;
        public static string DBPath;
        private static string dbName = "MySqlite.db3";
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here

            var sdCardPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            create_btn = FindViewById<Button>(Resource.Id.create_btn);
            load_btn = FindViewById<Button>(Resource.Id.load_btn);
            add_btn = FindViewById<Button>(Resource.Id.add_btn);
            listView1 = FindViewById<ListView>(Resource.Id.listView1);

            if (!exists)
                SqliteConnection.CreateFile(DBPath);

            var conn = new SqliteConnection("Data Source=" + DBPath);

            if (!exists)
                CreateDatabase(conn);


            create_btn.Click += Create_btn_Click;
            load_btn.Click += Load_btn_Click;
            add_btn.Click += Add_btn_Click;
        }

        private void Create_btn_Click(object sender, EventArgs e)
        {

        }

        private void Load_btn_Click(object sender, EventArgs e)
        {
            using (var conn = new SqliteConnection(_ConnectionString))
            {
                var sql = "SELECT * FROM User;";
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        _Data = new List<User>();
                        while (reader.Read())
                        {
                            _Data.Add(new User()
                            {
                                Id = reader.GetString(reader.GetOrdinal("Id")).ToString(),
                                Username = reader.GetString(reader.GetOrdinal("Username")).ToString()
                            });
                        }
                    }
                }
                conn.Close();
            }
            myAdapter = new MyAdapter(this, _Data);
            listView1.Adapter = myAdapter;
        }

        private void Add_btn_Click(object sender, EventArgs e)
        {

        }

        private static SqliteConnection GetConnection()
        {
            bool exists = File.Exists(DBPath);

            if (!exists)
                SqliteConnection.CreateFile(DBPath);

            var conn = new SqliteConnection("Data Source=" + DBPath);

            if (!exists)
                CreateDatabase(conn);

            return conn;
        }

        private static void CreateDatabase(SqliteConnection connection)
        {
            var sql = "CREATE TABLE User (Id ntext PRIMARY KEY, Username ntext);";

            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            // Create a sample note to get the user started
            sql = "INSERT INTO User (Id, Username) VALUES (@Id, @Username);";

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Id", "1");
                cmd.Parameters.AddWithValue("@Username", "Test01");
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.SQLite_Page, container, false);
        }
    }
}