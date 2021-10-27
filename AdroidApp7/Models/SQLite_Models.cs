using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace AndroidApp1.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class MyAdapter : BaseAdapter<User>
    {
        private Activity _Context;
        private List<User> _data;

        public MyAdapter(Activity context, List<User> data)
        {
            this._Context = context;
            this._data = data;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override User this[int position]
        {
            get { return _data[position]; }
        }

        public override int Count
        {
            get { return _data.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = _Context.LayoutInflater.Inflate(Resource.Layout.sqlite_items, null);
            }
            view.FindViewById<TextView>(Resource.Id.item1).Text = _data[position].Username;
            view.FindViewById<TextView>(Resource.Id.item2).Text = _data[position].Password;
            return view;
        }
    }
}