using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Mono.Data.Sqlite;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Android.Widget;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidApp1.Models;
using AndroidApp1.Othres;
using System.Collections.Generic;
using Android.Content;
using System.IO;
using Android.Hardware;
using Android.Media;
using Android.Locations;
using System.Linq;
using Android.Gms.Maps;
using static Google.Android.Material.Tabs.TabLayout;
using Android.Gms.Maps.Model;
using Android.Views.Animations;
using Xamarin.Essentials;

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

        [Obsolete]
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_home)
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
            else if (id == Resource.Id.nav_camera) //
            {
                var HomeTransaction = FragmentManager.BeginTransaction();
                HomeTransaction.Replace(Resource.Id.frameLayout1, new CameraPage_Activity(), "Camera_Page");
                HomeTransaction.Commit();
            }
            else if (id == Resource.Id.nav_gps)
            {
                var HomeTransaction = FragmentManager.BeginTransaction();
                HomeTransaction.Replace(Resource.Id.frameLayout1, new GPSPage_Activity(), "GPS_Page");
                HomeTransaction.Commit();
            }
            else if (id == Resource.Id.nav_mpas)
            {
                var HomeTransaction = FragmentManager.BeginTransaction();
                HomeTransaction.Replace(Resource.Id.frameLayout1, new MapPage_Activity(), "googleMap_Page");
                HomeTransaction.Commit();
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
            DBPath = System.IO.Path.Combine(sdCardPath, dbName);
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

    [Obsolete]
    public class CameraPage_Activity : Fragment
    {
        Button video_btn;
        Button record_btn;
        CamerFragment _camerFragment;

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.Camera_Page, container, false);

            video_btn = View.FindViewById<Button>(Resource.Id.video_btn);
            record_btn = View.FindViewById<Button>(Resource.Id.record_btn);
            video_btn.Click += Video_btn_Click;
            record_btn.Click += Record_btn_Click; ;

            return View;
        }

        private void Video_btn_Click(object sender, EventArgs e)
        {
            _camerFragment.Camera.TakePicture(null, null, new TakePictureCallBack(this.Activity));
        }

        private void Record_btn_Click(object sender, EventArgs e)
        {
            var HomeTransaction = FragmentManager.BeginTransaction();
            HomeTransaction.Replace(Resource.Id.frameLayout1, new RecordPage_Activity(), "RecordPage");
            HomeTransaction.Commit();
        }

        [Obsolete]
        public override void OnResume()
        {
            _camerFragment = new CamerFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.video_viewer, _camerFragment).Commit();
            base.OnResume();
        }

        [Obsolete]
        public override void OnPause()
        {
            _camerFragment.Camera.StopPreview();
            _camerFragment.Camera.Release();
            _camerFragment.Camera = null;
            base.OnPause();
        }

    }

    [Obsolete]
    public class RecordPage_Activity : Fragment
    {
        public Camera _Camera { get; set; }

        ISurfaceHolder _Holder;
        MediaRecorder _MediaRecorder;
        SurfaceHolderCallBack _SurfaceHolderCallBack;

        Button record_start_btn;
        Button camera_page_btn;
        SurfaceView record_View;

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.Record_Page, container, false);

            record_start_btn = View.FindViewById<Button>(Resource.Id.record_start_btn);
            camera_page_btn = View.FindViewById<Button>(Resource.Id.camera_page_btn);
            record_View = View.FindViewById<SurfaceView>(Resource.Id.record_View);

            record_start_btn.Click += Record_start_btn_Click;
            camera_page_btn.Click += Camera_page_btn_Click;

            _MediaRecorder = new MediaRecorder();
            _Camera = Camera.Open();
            _Holder = record_View.Holder;
            _SurfaceHolderCallBack = new SurfaceHolderCallBack(this.Activity, _Camera, _MediaRecorder);
            _Holder.AddCallback(_SurfaceHolderCallBack);
            _Holder.SetType(SurfaceType.PushBuffers);

            return View;
        }

        private void Camera_page_btn_Click(object sender, EventArgs e)
        {
            var HomeTransaction = FragmentManager.BeginTransaction();
            HomeTransaction.Replace(Resource.Id.frameLayout1, new CameraPage_Activity(), "Camera_Page");
            HomeTransaction.Commit();
        }

        private void Record_start_btn_Click(object sender, EventArgs e)
        {
            if (record_start_btn.Text == "開始錄影")
            {
                var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
                var VideoPath = Path.Combine(sdCardPath, "video.3gp");
                File.Create(VideoPath);

                record_start_btn.Text = "停止錄影";
                _Camera.StartPreview();
                _Camera.Unlock();
                _MediaRecorder.SetCamera(_Camera);
                _MediaRecorder.SetVideoSource(VideoSource.Default);
                _MediaRecorder.SetAudioSource(AudioSource.Mic);
                _MediaRecorder.SetProfile(CamcorderProfile.Get(CamcorderQuality.High));
                _MediaRecorder.SetOutputFile(VideoPath);
                _MediaRecorder.SetPreviewDisplay(_Holder.Surface);
                _MediaRecorder.SetOrientationHint(_SurfaceHolderCallBack.RotataDegrees);
                _MediaRecorder.Prepare();
                _MediaRecorder.Start();
                Toast.MakeText(this.Activity,"開始錄影",ToastLength.Short).Show();
            }
            else
            {
                record_start_btn.Text = "開始錄影";
                _MediaRecorder.Stop();
                _Camera.Reconnect();
                _Camera.StartPreview();
                Toast.MakeText(this.Activity, "錄影完成", ToastLength.Short).Show();
            }
        }
    }

    [Obsolete]
    public class GPSPage_Activity : Fragment , ILocationListener
    {
        EditText editText1;
        EditText editText2;
        EditText editText3;
        EditText editText4;
        EditText editText5;
        Button button1;
        Button button2;
        Button button3;
        LocationManager locationManager;
        string locationstring;
        public string TAG { get; set;}

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.gps_Page, container, false);

            editText1 = View.FindViewById<EditText>(Resource.Id.editText1);
            editText2 = View.FindViewById<EditText>(Resource.Id.editText2);
            editText3 = View.FindViewById<EditText>(Resource.Id.editText3);
            editText4 = View.FindViewById<EditText>(Resource.Id.editText4);
            editText5 = View.FindViewById<EditText>(Resource.Id.editText5);

            button1 = View.FindViewById<Button>(Resource.Id.button1);
            button2 = View.FindViewById<Button>(Resource.Id.button2);
            button3 = View.FindViewById<Button>(Resource.Id.button3);

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            return View;
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            editText1.Text = location.Latitude.ToString();
            editText2.Text = location.Longitude.ToString();
            editText3.Text = location.Provider.ToString();
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
            
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            locationstring = LocationManager.GpsProvider;
            if (locationManager.IsLocationEnabled)
            {
                locationManager.RequestLocationUpdates(locationstring, 6000, 1, this);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            LocationToAddress();
        }

        private async void LocationToAddress() //經緯度轉地址
        {
            try
            {
                var geo = new Geocoder(this.Activity, Java.Util.Locale.Default);
                var addresses = await geo.GetFromLocationAsync(Convert.ToDouble(editText1.Text), Convert.ToDouble(editText2.Text), 1);
                if (addresses.Any())
                {
                    editText4.Text = addresses[0].GetAddressLine(0);
                }
                else
                {
                    editText4.Text = "Could not find any address.";
                }
            }
            catch{}
        }

        private void Button3_Click(object sender, EventArgs e) //地址轉經緯度 
        {
            try
            {
                var add = Convert.ToString(editText4.Text);
                Geocoder geocoder = new Geocoder(this.Activity, Java.Util.Locale.Default);
                IList<Address> resault = geocoder.GetFromLocationName(add, 10);
                if (resault != null)
                {
                    editText5.Text = resault[0].Latitude.ToString() + "," + resault[0].Longitude.ToString();
                }
            }
            catch{}
        }

        [Obsolete]
        public override void OnResume()
        {
            locationManager = Context.GetSystemService(Context.LocationService) as LocationManager;
            base.OnResume();
        }

        [Obsolete]
        public override void OnPause()
        {
            locationManager.RemoveUpdates(this);
            base.OnPause();
        }
    }

    [Obsolete]
    public class MapPage_Activity : Fragment , IOnMapReadyCallback , ILocationListener
    {
        MapView mapview;
        GoogleMap GoogleMap;
        LocationManager locationManager;
        Button button1;
        Button button2;
        Button Button3;
        SurfaceView map_record_View;
        FloatingActionButton now_location;
        Marker markerOpt1;

        string locationstring;
        public double nowLatitude;
        public double nowLongitude;
        int zoom;

        public string TAG { get; set; }

        public Camera _Camera { get; set; }

        SensorSpeed speed = SensorSpeed.UI;
        ISurfaceHolder _Holder;
        MediaRecorder _MediaRecorder;
        SurfaceHolderCallBack_Two _SurfaceHolderCallBack;

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var View = inflater.Inflate(Resource.Layout.googleMap_Page, container, false);

            zoom = 18;

            button1 = View.FindViewById<Button>(Resource.Id.zoom_out);
            button2 = View.FindViewById<Button>(Resource.Id.zoom_in);
            Button3 = View.FindViewById<Button>(Resource.Id.map_record_button);
            now_location = View.FindViewById<FloatingActionButton>(Resource.Id.now_location);
            map_record_View = View.FindViewById<SurfaceView>(Resource.Id.map_record_View);

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
            now_location.Click += Fab_Click;

            mapview = (MapView)View.FindViewById(Resource.Id.map);
            mapview.OnCreate(savedInstanceState);
            mapview.OnResume();
            mapview.GetMapAsync(this);

            _MediaRecorder = new MediaRecorder();
            _Camera = Camera.Open();
            _Holder = map_record_View.Holder;
            _SurfaceHolderCallBack = new SurfaceHolderCallBack_Two(this.Activity, _Camera, _MediaRecorder);
            _Holder.AddCallback(_SurfaceHolderCallBack);
            _Holder.SetType(SurfaceType.PushBuffers);

            return View;
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            if (markerOpt1 != null){ markerOpt1.Remove(); }

            LatLng lat = new LatLng(nowLatitude, nowLongitude);
            markerOpt1 = GoogleMap.AddMarker(
                new MarkerOptions().SetPosition(lat)
                .InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.baseline_navigation_black_18))
                .Anchor(0.5f,0.5f)
                .SetRotation((float)e.Reading.HeadingMagneticNorth));
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            GoogleMap = googleMap;
            Fab_Click(null,null);

            LatLng lat = new LatLng(nowLatitude, nowLongitude);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(lat);
            builder.Zoom(zoom);
            builder.Build();

            Compass.Start(speed);
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            try
            {
                locationstring = LocationManager.GpsProvider;
                if (locationManager.IsLocationEnabled)
                {
                    locationManager.RequestLocationUpdates(locationstring, 6000, 1, (ILocationListener)this);
                }
            }
            catch { }
        }

        private void Button1_Click(object sender, EventArgs e) //zoom in
        {
            zoom--;
            if (zoom < 10) zoom = 10;

            LatLng location = new LatLng(nowLatitude, nowLongitude);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(location, zoom);
            GoogleMap.MoveCamera(cameraUpdate);
        }

        private void Button2_Click(object sender, EventArgs e) //zoom out
        {
            zoom++;
            LatLng location = new LatLng(nowLatitude, nowLongitude);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(location, zoom);
            GoogleMap.MoveCamera(cameraUpdate);   
        }

        private void Button3_Click(object sender, EventArgs e) //record
        {
            if (Button3.Text == "Record")
            {
                var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
                var VideoPath = Path.Combine(sdCardPath, "video.3gp");
                File.Create(VideoPath);

                Button3.Text = "Stop";
                _Camera.StartPreview();
                _Camera.Unlock();
                _MediaRecorder.SetCamera(_Camera);
                _MediaRecorder.SetVideoSource(VideoSource.Default);
                _MediaRecorder.SetAudioSource(AudioSource.Mic);
                _MediaRecorder.SetProfile(CamcorderProfile.Get(CamcorderQuality.High));
                _MediaRecorder.SetOutputFile(VideoPath);
                _MediaRecorder.SetPreviewDisplay(_Holder.Surface);
                _MediaRecorder.SetOrientationHint(_SurfaceHolderCallBack.RotataDegrees);
                _MediaRecorder.Prepare();
                _MediaRecorder.Start();
                Toast.MakeText(this.Activity, "開始錄影", ToastLength.Short).Show();
            }
            else
            {
                Button3.Text = "Record";
                _MediaRecorder.Stop();
                _Camera.Reconnect();
                _Camera.StartPreview();
                Toast.MakeText(this.Activity, "錄影完成", ToastLength.Short).Show();
            }
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            nowLatitude = location.Latitude;
            nowLongitude = location.Longitude;

            LatLng lat = new LatLng(location.Latitude,location.Longitude);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(lat, zoom);
            GoogleMap.MoveCamera(cameraUpdate);
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
           
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }

        [Obsolete]
        public override void OnResume()
        {
            locationManager = Context.GetSystemService(Context.LocationService) as LocationManager;
            base.OnResume();
        }

        [Obsolete]
        public override void OnPause()
        {
            locationManager.RemoveUpdates(this);
            base.OnPause();
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
            if (readable(new SqliteConnection(_ConnectionString)))
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
}

