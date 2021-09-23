using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Widget;
using static Android.Icu.Text.CaseMap;
using System;
using System.Collections.Generic;
using static Android.Content.ClipData;

namespace SQLiteApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button button;
        private EditText editText;
        private CheckBox checkBox;
        private TextView textView;
        private RadioButton radioButton1 , radioButton2, radioButton3;
        private Spinner spinner;
        private List<string> data = new List<string>();
        private List<int> imagedata = new List<int>();
        private TextView textView3;
        private GridView gridView;

    protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            button = (Button)FindViewById(Resource.Id.button1);
            editText = (EditText)FindViewById(Resource.Id.editText1);
            checkBox = (CheckBox)FindViewById(Resource.Id.checkBox1);
            textView = (TextView)FindViewById(Resource.Id.textView2);
            radioButton1 = (RadioButton)FindViewById(Resource.Id.radioButton1);
            radioButton2 = (RadioButton)FindViewById(Resource.Id.radioButton2);
            radioButton3 = (RadioButton)FindViewById(Resource.Id.radioButton3);
            spinner = (Spinner)FindViewById(Resource.Id.spinner1);
            textView3 = (TextView)FindViewById(Resource.Id.textView3);
            

            data.Add("A");
            data.Add("B");
            data.Add("C");
            data.Add("D");
            ArrayAdapter arrayAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, data);
            spinner.Adapter = arrayAdapter;

            imagedata.Add(Resource.Drawable.image0);
            imagedata.Add(Resource.Drawable.sddefault);
            ArrayAdapter arrayAdapter1 = new ArrayAdapter();

            button.Click += delegate { button1_click(); };
            radioButton1.Click += Radio_click;
            radioButton2.Click += Radio_click;
            radioButton3.Click += Radio_click;
            spinner.ItemSelected += Spinner_click;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void button1_click()
        {
            if(checkBox.Checked)
            {
                using (var db = new Android.App.AlertDialog.Builder(this))
                {
                    db.SetTitle("確認");
                    db.SetMessage("是否送出?");
                    db.SetPositiveButton("是", (sender, args) => { });
                    db.SetNegativeButton("否", (sender, args) => { });
                    db.Show();
                }
            }
            else
            {
                Toast.MakeText(Application.Context, "資料有誤", ToastLength.Long).Show();
            }
        }

        private void Radio_click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            textView.Text = rb.Text;
        }

        private void Spinner_click(object sender, EventArgs e)
        {
            Spinner sp = (Spinner)sender;
            textView3.Text = sp.SelectedItem.ToString();
        }
    }
}