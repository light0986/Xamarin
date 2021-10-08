using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndroidApps
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string[] Balls = new string[] { "籃球","足球","棒球","其他" };
        private string[] Balls_EN = new string[] {"BasketBall","FootBall","BaseBall","Others" };

        ListView listView;
        TextView textView;
        ToggleButton toggleButton;
        Button button;
        SeekBar seekBar;

        [Obsolete]
#pragma warning disable CS0809 // 過時成員會覆寫非過時成員
        protected override void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS0809 // 過時成員會覆寫非過時成員
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            listView = (ListView)FindViewById(Resource.Id.listView1);
            textView = (TextView)FindViewById(Resource.Id.textView1);
            toggleButton = (ToggleButton)FindViewById(Resource.Id.toggleButton1);
            button = (Button)FindViewById(Resource.Id.button1);
            seekBar = (SeekBar)FindViewById(Resource.Id.seekBar1);

            JavaList<IDictionary<string, object>> datas = new JavaList<IDictionary<string, object>>();
            for(int i = 0; i < Balls.Length; i++)
            {
                datas.Add(GetDictionary(Balls[i],Balls_EN[i]));
            }

            SimpleAdapter simpleAdapter = new SimpleAdapter(this, datas, Android.Resource.Layout.SimpleListItem2, new string[] { "one", "two" }, new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 });
            listView.Adapter = simpleAdapter;
            listView.SetSelector(Resource.Drawable.blue);

            listView.ItemClick += listview_click;
            toggleButton.CheckedChange += toggleButton_check;
            button.Click += buttonclick;
            seekBar.ProgressChanged += seekBar_change;
        }

        private void seekBar_change(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            textView.Text = e.Progress.ToString();
        }

        private void toggleButton_check(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (toggleButton.Checked)
            {
                toggleButton.Text = "ON";
            }
            else
            {
                toggleButton.Text = "OFF";
            }
        }

        [Obsolete]
        private void buttonclick(object sender, EventArgs e)
        {
            if (toggleButton.Checked)
            {
                ProgressDialog progressDialog = new ProgressDialog(this);
                progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
                progressDialog.SetMessage("請稍後...");
                progressDialog.SetCancelable(false);
                progressDialog.Max = seekBar.Progress;
                progressDialog.Show();
                runprogress(progressDialog);
            }
        }

        [Obsolete]
        private async void runprogress(ProgressDialog progressDialog)
        {
            int i = 0;
            while (i <= seekBar.Progress)
            {
                i++;
                await Task.Delay(50);
                progressDialog.Progress = i;
            }
            progressDialog.Cancel();
        }

        private void timerPickerChange(object sender, TimePicker.TimeChangedEventArgs e)
        {
            textView.Text = string.Format("{0}:{1}", e.HourOfDay, e.Minute.ToString("00"));
        }

        private void listview_click(object sender, AdapterView.ItemClickEventArgs e)
        {
            textView.Text = Balls[e.Position];
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private JavaDictionary<string, object> GetDictionary(string v1, string v2)
        {
            JavaDictionary<string,object> dic = new JavaDictionary<string, object>();
            dic.Add("one",v1);
            dic.Add("two",v2);
            return dic;
        }
    }

    public class MyAdapter : BaseAdapter<string>
    {
        private Activity _Context;
        private string[] _Items;

        public MyAdapter(Activity context, string[] items)
        {
            _Context = context;
            _Items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return _Items[position]; }
        }

        public override int Count
        {
            get { return _Items.Length; }
        }

        public override View GetView(int position , View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = _Context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }
            view.FindViewById<TextView>(Resource.Id.text).Text = _Items[position];
            return view;
        }
    }
}