using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using static Android.Graphics.ColorSpace;

namespace AndroidApp2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListView listView;
        private TextView textView;
        private string[] Balls = new string[] { "籃球", "足球", "棒球", "其他" };
        private int count = 0;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            listView = (ListView)FindViewById(Resource.Id.listView1);
            textView = (TextView)FindViewById(Resource.Id.textView1);

            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleListItemMultipleChoice,Balls);
            listView.Adapter = arrayAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple ;

            count = listView.Count;

            listView.ItemClick += ListView_Click;
            
        }

        private void ListView_Click(object sender, System.EventArgs e)
        {
            string selAll = "";
            for(int p = 0; p < count; p++)
            {
                if (listView.IsItemChecked(p))
                {
                    selAll += Balls[p] + " ";
                }
                textView.Text = "我最喜歡的運動是" + selAll;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}