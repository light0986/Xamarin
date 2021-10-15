using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace AndroidApp3
{
    [Activity(Label = "APP", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            TabHost tabHost = (TabHost)FindViewById(Resource.Id.tabHost1);
            tabHost.Setup();

            TabHost.TabSpec tabSpec1 = tabHost.NewTabSpec("第一頁");
            tabSpec1.SetContent(Resource.Id.linearLayout2);
            tabSpec1.SetIndicator("第一頁");

            TabHost.TabSpec tabSpec2 = tabHost.NewTabSpec("第二頁");
            tabSpec2.SetContent(Resource.Id.linearLayout3);
            tabSpec2.SetIndicator("第二頁");

            TabHost.TabSpec tabSpec3 = tabHost.NewTabSpec("第三頁");
            tabSpec3.SetContent(Resource.Id.linearLayout4);
            tabSpec3.SetIndicator("第三頁");

            tabHost.AddTab(tabSpec1);
            tabHost.AddTab(tabSpec2);
            tabHost.AddTab(tabSpec3);


        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.share:
                    Intent sendShareIntent = new Intent(Intent.ActionSend)
                        .SetType("text/plain")
                        .PutExtra(Intent.ExtraText, "www.maximeesprit.com");

                    StartActivity(sendShareIntent);
                    break;

                case Resource.Id.moreoption:
                    if (SupportActionBar.Title == Resources.GetString(Resource.String.titleRenamed))
                        SupportActionBar.Title = Resources.GetString(Resource.String.titleBase);
                    else
                        SupportActionBar.Title = Resources.GetString(Resource.String.titleRenamed);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}