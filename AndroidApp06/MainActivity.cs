using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using DrawerLayout = AndroidX.DrawerLayout.Widget.DrawerLayout;
using NavigationView = Android.Support.Design.Widget.NavigationView;

namespace AndroidApp3
{
    [Activity(Label = "APP", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout m_drawerLayout;
        NavigationView m_navigationView;
        TabHost tabHost;

        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.baseline_menu_black_24dp);

            m_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.maintActivity_drawerlayout);
            m_navigationView = FindViewById<NavigationView>(Resource.Id.maintActivity_navigationView);
            m_navigationView.NavigationItemSelected += M_navigationView_NavigationItemSelected;

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

                case Android.Resource.Id.Home:
                    if (m_drawerLayout.IsDrawerOpen(GravityCompat.Start))
                        m_drawerLayout.CloseDrawers();
                    else
                        m_drawerLayout.OpenDrawer(GravityCompat.Start);
                    
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        [System.Obsolete]
        private void M_navigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.leftmenu_profile:
                    Toast.MakeText(this, "leftmenu_profile", ToastLength.Short).Show();
                    m_drawerLayout.CloseDrawers();
                    break;

                case Resource.Id.leftmenu_shopping_list:
                    Toast.MakeText(this, "leftmenu_shopping_list", ToastLength.Short).Show();
                    m_drawerLayout.CloseDrawers();
                    break;

                case Resource.Id.leftmenu_favorites:
                    Toast.MakeText(this, "leftmenu_favorites", ToastLength.Short).Show();
                    m_drawerLayout.CloseDrawers();
                    break;

                case Resource.Id.leftmenu_params_option:
                    Toast.MakeText(this, "leftmenu_params_option", ToastLength.Short).Show();
                    m_drawerLayout.CloseDrawers();
                    break;

                case Resource.Id.leftmenu_params_about:
                    Toast.MakeText(this, "leftmenu_params_about", ToastLength.Short).Show();
                    m_drawerLayout.CloseDrawers();
                    break;
            }
            m_drawerLayout.CloseDrawer(GravityCompat.Start);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}