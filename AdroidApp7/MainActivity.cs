using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Mono.Data.Sqlite;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidApp1.Models;
using System.Collections.Generic;
using Android.Content;
using System.IO;
using static Android.Service.Voice.VoiceInteractionSession;
using Java.Util;
using static Android.Renderscripts.Sampler;
using static Android.Content.ClipData;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public static string UserAccount = "Guest";
        IMenuItem nav_send;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            var HomeTransaction = FragmentManager.BeginTransaction();
            HomeTransaction.Replace(Resource.Id.frameLayout1, new HomePage_Activity(), "Welcome");
            HomeTransaction.Commit();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        [Obsolete]
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                var HomeTransaction = FragmentManager.BeginTransaction();
                HomeTransaction.Replace(Resource.Id.frameLayout1, new HomePage_Activity(), "Home_Page");
                HomeTransaction.Commit();
            }
            else if (id == Resource.Id.nav_gallery)
            {
                if (UserAccount == "Guest")
                {
                    Toast.MakeText(this,"請先登入",ToastLength.Short).Show();
                }
                else
                {
                    var HomeTransaction = FragmentManager.BeginTransaction();
                    HomeTransaction.Replace(Resource.Id.frameLayout1, new SQLitePage_Activity(), "Sqlite_page");
                    HomeTransaction.Commit();
                }
            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {
                nav_send = item;
                if(UserAccount == "Guest")
                {
                    Intent intent = new Intent(this, typeof(LoginPage_Activity));
                    intent.PutExtra("title",1);
                    StartActivityForResult(intent, 1);
                }
                else
                {
                    AndroidX.AppCompat.App.AlertDialog.Builder alertDiag = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("登出?");
                    alertDiag.SetMessage("請問是否登出?");
                    alertDiag.SetPositiveButton("登出", (senderAlert, args) =>
                    {
                        nav_send.SetTitle("登入");
                        UserAccount = "Guest";
                        TextView Guest_textView = (TextView)FindViewById(Resource.Id.Guest_textView);
                        Guest_textView.Text = UserAccount;

                        var HomeTransaction = FragmentManager.BeginTransaction();
                        HomeTransaction.Replace(Resource.Id.frameLayout1, new HomePage_Activity(), "Home_Page");
                        HomeTransaction.Commit();
                    });
                    alertDiag.SetNegativeButton("取消", (senderAlert, args) =>
                    {
                        alertDiag.Dispose();
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok)
            {
                nav_send.SetTitle("登出");
                TextView Guest_textView = (TextView)FindViewById(Resource.Id.Guest_textView);
                Guest_textView.Text = UserAccount;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [Activity]
    public class LoginPage_Activity : AppCompatActivity
    {
        Button Login_btn;
        EditText Logint_Account;
        EditText Logint_Password;

        private static string _ConnectionString;
        private static string DBPath;
        private static string dbName = "MySqlite.db3";

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Login_Page);

            Login_btn = (Button)FindViewById(Resource.Id.Login_btn);
            Logint_Account = (EditText)FindViewById(Resource.Id.Logint_Account);
            Logint_Password = (EditText)FindViewById(Resource.Id.Logint_Password);

            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = System.IO.Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            Login_btn.Click += Login_btn_Click;
        }

        private void Login_btn_Click(object sender, EventArgs e)
        {
            if(readable(new SqliteConnection(_ConnectionString)))
            {
                var intent = new Intent(this, typeof(MainActivity));
                MainActivity.UserAccount = Logint_Account.Text;
                SetResult(Result.Ok, intent);
                this.Finish();
            }
            else
            {
                Toast.MakeText(this, "帳號密碼錯誤", ToastLength.Short).Show();
            }
        }

        private bool readable(SqliteConnection connectionString)
        {
            bool exists = File.Exists(DBPath);
            bool boo = false;
            if (exists)
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    var sql = "SELECT * FROM User WHERE Username = @Username AND Password = @Password;";
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Username", Logint_Account.Text);
                        cmd.Parameters.AddWithValue("@Password", Logint_Password.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                boo = true;
                            }
                            else
                            {
                                boo = false;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            return boo;
        }

    }

    [Obsolete]
    public class HomePage_Activity : Fragment
    {
        private static string _ConnectionString;
        private static string DBPath;
        private static string dbName = "MySqlite.db3";

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.Home_Page, container, false);

            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = System.IO.Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            if (!exists)
                SqliteConnection.CreateFile(DBPath);
            if (!exists)
                CreateDatabase(new SqliteConnection(_ConnectionString));

            return View;
        }

        private void CreateDatabase(SqliteConnection connectionString)
        {
            var sql = "CREATE TABLE User (Username ntext PRIMARY KEY, Password ntext);";

            connectionString.Open();

            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            connectionString.Close();
        }
    }

    [Obsolete]
    public class SQLitePage_Activity : Fragment
    {
        List<User> _Data;
        MyAdapter myAdapter;
        ListView listView1;

        private static string _ConnectionString;
        private static string DBPath;
        private static string dbName = "MySqlite.db3";

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.Sqlite_page, container, false);

            FloatingActionButton fab = View.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            listView1 = View.FindViewById<ListView>(Resource.Id.listView1);

            if (!exists)
                SqliteConnection.CreateFile(DBPath);
            if (!exists)
                CreateDatabase(new SqliteConnection(_ConnectionString));
            Refresh(_ConnectionString);

            listView1.ItemClick += ListView1_ItemClick;
            listView1.ItemLongClick += ListView1_ItemLongClick;

            return View;
        }

        private void ListView1_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AndroidX.AppCompat.App.AlertDialog.Builder alertDiag = new AndroidX.AppCompat.App.AlertDialog.Builder(Activity);
            alertDiag.SetTitle("刪除?");
            alertDiag.SetMessage("請問是否刪除選擇?");
            alertDiag.SetPositiveButton("刪除", (senderAlert, args) => 
            {
                string str = _Data[e.Position].Username;
                Delete_Data(new SqliteConnection(_ConnectionString), str);
                Refresh(_ConnectionString);
            });
            alertDiag.SetNegativeButton("取消", (senderAlert, args) => 
            {
                alertDiag.Dispose();
            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        private void ListView1_ItemClick(object sender, AdapterView.ItemClickEventArgs e) //按一下
        {
            Intent intent = new Intent(Activity, typeof(UserDetail_Activity));
            UserDetail_Activity.UserName = _Data[e.Position].Username;
            UserDetail_Activity.Password = _Data[e.Position].Password;
            StartActivityForResult(intent, 0);
        }

        private void FabOnClick(object sender, EventArgs e) //新增作業
        {
            Intent intent = new Intent(Activity, typeof(Add_Items_Activity));
            StartActivityForResult(intent , 0);
        }

        [Obsolete]
        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(requestCode == 0 && resultCode == Result.Ok)
            {
                Refresh(_ConnectionString);
            }
        }

        private void CreateDatabase(SqliteConnection connectionString)
        {
            connectionString.Open();

            var sql = "CREATE TABLE User (Username ntext PRIMARY KEY, Password ntext);";
            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            sql = "INSERT INTO User VALUES ('admin01','password');";
            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            connectionString.Close();
        }

        private void Refresh(string ConnectionString)
        {
            bool exists = File.Exists(DBPath);
            if (exists)
            {
                using (var conn = new SqliteConnection(ConnectionString))
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
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Password = reader.GetString(reader.GetOrdinal("Password"))
                                });
                            }
                        }
                    }
                    conn.Close();
                }
                if (_Data.Count != 0)
                {
                    myAdapter = new MyAdapter(Activity, _Data);
                    listView1.Adapter = myAdapter;
                }
                else
                {
                    listView1.SetAdapter(null);
                    Toast.MakeText(Activity,"沒有紀錄",ToastLength.Short).Show();
                }
            }
        }

        private void Delete_Data(SqliteConnection connectionString , string Username)
        {
            connectionString.Open();

            var sql = "DELETE FROM User Where Username = @Username;";
            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.ExecuteNonQuery();
            }

            connectionString.Close();
        }
    }

    [Activity]
    public class Add_Items_Activity : AppCompatActivity
    {
        Button button;
        EditText add_items_edt01;
        EditText add_items_edt02;

        private static string _ConnectionString;
        private static string DBPath;
        private static string dbName = "MySqlite.db3";

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Add_Items);
            button = (Button)FindViewById(Resource.Id.add_items_btn);
            add_items_edt01 = (EditText)FindViewById(Resource.Id.add_items_edt01);
            add_items_edt02 = (EditText)FindViewById(Resource.Id.add_items_edt02);

            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = System.IO.Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            if (!exists)
                SqliteConnection.CreateFile(DBPath);
            if (!exists)
                CreateDatabase(new SqliteConnection(_ConnectionString));

            button.Click += Button_Click;
        }

        private void CreateDatabase(SqliteConnection connectionString)
        {
            var sql = "CREATE TABLE User (Username ntext PRIMARY KEY, Password ntext);";

            connectionString.Open();

            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            connectionString.Close();
        }

        private void add_items(SqliteConnection connectionString)
        {
            var sql = "INSERT INTO User (Username , Password) VALUES (@Username , @Password);";
            connectionString.Open();

            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Username", add_items_edt01.Text);
                cmd.Parameters.AddWithValue("@Password", add_items_edt02.Text);
                cmd.ExecuteNonQuery();
            }
            connectionString.Close();
        }

        private bool readable(SqliteConnection connectionString)
        {
            bool exists = File.Exists(DBPath);
            bool boo = false;
            if (exists)
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    var sql = "SELECT * FROM User WHERE Username = @Username;";
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Username", add_items_edt01.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                boo = true;
                            }
                            else
                            {
                                boo = false;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            return boo;
        }

        [Obsolete]
        private void Button_Click(object sender, EventArgs e)
        {
            if(add_items_edt01.Text.Length * add_items_edt02.Text.Length != 0)
            {
                if(readable(new SqliteConnection(_ConnectionString)))
                {
                    Toast.MakeText(this, "帳號存在", ToastLength.Short).Show();
                }
                else
                {
                    add_items(new SqliteConnection(_ConnectionString));
                    var intent = new Intent(this, typeof(SQLitePage_Activity));
                    SetResult(Result.Ok, intent);
                    this.Finish();
                }
            }
        }
    }

    [Activity]
    public class UserDetail_Activity : AppCompatActivity
    {
        Button edit_btn;
        EditText editText_Password01;
        EditText editText_Password02;
        TextView textView_Name;

        public static string UserName;
        public static string Password;
        private static string _ConnectionString;
        private static string DBPath;
        private static string dbName = "MySqlite.db3";

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.User_Detail);

            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath; ;
            DBPath = System.IO.Path.Combine(sdCardPath, dbName);
            bool exists = File.Exists(DBPath);
            _ConnectionString = "Data Source=" + DBPath;

            edit_btn = FindViewById<Button>(Resource.Id.edit_btn);
            editText_Password01 = FindViewById<EditText>(Resource.Id.editText_Password01);
            editText_Password02 = FindViewById<EditText>(Resource.Id.editText_Password02);
            textView_Name = FindViewById<TextView>(Resource.Id.textView_Name);

            edit_btn.Click += Edit_btn_Click;
            textView_Name.Text = UserName;
        }

        [Obsolete]
        private void Edit_btn_Click(object sender, EventArgs e)
        {
            if (editText_Password01.Text.Length != 0)
            {
                if(editText_Password01.Text == editText_Password02.Text)
                {
                    change_items(new SqliteConnection(_ConnectionString));
                    var intent = new Intent(this, typeof(SQLitePage_Activity));
                    SetResult(Result.Ok, intent);
                    this.Finish();
                }
                else
                {
                    editText_Password01.Text = "";
                    editText_Password02.Text = "";
                    Toast.MakeText(this, "密碼不一致", ToastLength.Long);
                }
            }
        }

        private void change_items(SqliteConnection connectionString)
        {
            var sql = "UPDATE User SET Password = @Password WHERE Username = @Username";
            connectionString.Open();
            using (var cmd = connectionString.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Username", textView_Name.Text);
                cmd.Parameters.AddWithValue("@Password", editText_Password01.Text);
                cmd.ExecuteNonQuery();
            }
            connectionString.Close();
        }
    }
}

