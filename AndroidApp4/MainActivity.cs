using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button button1;
        Button button2;
        Button button3;
        TextView textView1;
        ListView listView;
        public static int item_position;
        public static string[] Items = new string[] {"A","B","C"};

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            button1 = (Button)FindViewById(Resource.Id.button1);
            button2 = (Button)FindViewById(Resource.Id.button2);
            button3 = (Button)FindViewById(Resource.Id.button3);
            listView = (ListView)FindViewById(Resource.Id.listView1);
            

            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Items);
            listView.Adapter = arrayAdapter;
            listView.SetSelector(Resource.Drawable.blue);


            listView.ItemClick += ListView_ItemClick;
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            item_position = e.Position;
            textView1 = (TextView)FindViewById(Resource.Id.textView1);
            textView1.Text = Items[item_position].ToString();
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActivityA));
            ActivityA.str = Items[item_position].ToString();
            ActivityA.positon = item_position;
            StartActivity(intent);
        }

        private void Button2_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActivityB));
            StartActivity(intent);
        }

        private void Button3_Click(object sender, System.EventArgs e)
        {
            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Items);
            listView.Adapter = arrayAdapter;
            listView.SetSelector(Resource.Drawable.blue);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


    [Activity]
    public class ActivityA : AppCompatActivity
    {
        Button button4;
        EditText editText1;
        public static string str;
        public static int positon;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.PageA);
            editText1 = (EditText)FindViewById(Resource.Id.editText1);
            editText1.Text = str;

            button4 = (Button)FindViewById(Resource.Id.button4);
            button4.Click += Button4_Click;
        }

        private void Button4_Click(object sender, System.EventArgs e)
        {
            MainActivity.Items[positon] = editText1.Text;
            this.Finish();
        }
    }

    [Activity]
    public class ActivityB : AppCompatActivity
    {
        Button button5;
        EditText editText2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.PageB);
            editText2 = (EditText)FindViewById(Resource.Id.editText2);

            button5 = (Button)FindViewById(Resource.Id.button5);
            button5.Click += Button5_Click;
        }

        private void Button5_Click(object sender, System.EventArgs e)
        {
            int l = MainActivity.Items.Length;
            MainActivity.Items = MainActivity.Items.Append(editText2.Text).ToArray();
            this.Finish();
        }
    }
}