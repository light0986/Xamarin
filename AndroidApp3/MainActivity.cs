using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xamarin.Essentials;

namespace AndroidApp2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button button;
        Button button3;
        Button button4;
        EditText editText1;
        EditText editText2;
        ListView listView1;
        EditText editText3;
        EditText editText4;

        private List<ListItem> _data;
        private MyAdapter myAdapter;
        private int Pos;
        private int page_name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            page_name = Resource.Layout.activity_main;
            SetContentView(page_name);

            button = (Button)FindViewById(Resource.Id.button1);
            editText1 = (EditText)FindViewById(Resource.Id.editText1);
            editText2 = (EditText)FindViewById(Resource.Id.editText2);
            button.Click += Button_Click;
            
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (acc_pass(editText1.Text,editText2.Text))
            {
                _data = new ListItem[]
{
                    new ListItem("獅子", 2),
                    new ListItem("兔子", 8),
                    new ListItem("大象", 2 )
                }.ToList();
                page2();
            }
            else
            {
                Toast t = Toast.MakeText(this, "錯誤", ToastLength.Short);
                t.SetGravity(Android.Views.GravityFlags.CenterHorizontal, 0, 0);
                t.Show();
            }
        }

        private void page2()
        {
            page_name = Resource.Layout.Main;
            SetContentView(page_name);
            Toast t = Toast.MakeText(this, "登入成功", ToastLength.Short);
            t.SetGravity(Android.Views.GravityFlags.CenterHorizontal, 0, 0);
            t.Show();

            listView1 = (ListView)FindViewById(Resource.Id.listView1);
            myAdapter = new MyAdapter(this, _data);
            listView1.Adapter = myAdapter;
            listView1.ItemClick += (sender, e) =>
            {
                page_name = Resource.Layout.Detail;
                SetContentView(page_name);

                editText3 = (EditText)FindViewById(Resource.Id.editText3);
                editText4 = (EditText)FindViewById(Resource.Id.editText4);
                button3 = (Button)FindViewById(Resource.Id.button3);
                button4 = (Button)FindViewById(Resource.Id.button4);

                Pos = e.Position;
                editText3.Text = _data[e.Position].Name.ToString();
                editText4.Text = _data[e.Position].Count.ToString();

                button3.Click += Button3_Click;
                button4.Click += Button4_Click;
            };
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            _data[Pos].Name = editText3.Text;
            _data[Pos].Count = Convert.ToInt32(editText4.Text);
            page2();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            page2();
        }

        private bool acc_pass(string account, string password)
        {
            if(account == "123" && password == "123")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class ListItem
    {
        public string Name { get; set; }
        public int Count {  get; set; }
        public ListItem(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
    }


}