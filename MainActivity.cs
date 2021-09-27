using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Collections.Generic;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private GridView gridView;
        private ImageView imageView;
        private TextView textView;
        private int[] thumbIds = {Resource.Drawable.sddefault, Resource.Drawable.image0};

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            gridView = (GridView)FindViewById(Resource.Id.gridView1);
            imageView = (ImageView)FindViewById(Resource.Id.imageView1);
            textView = (TextView)FindViewById(Resource.Id.textView1);

            ImageAdapter.thumbIds = thumbIds;
            gridView.Adapter = new ImageAdapter(this);
            gridView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) 
            { imageView.SetImageResource(thumbIds[args.Position]); };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}