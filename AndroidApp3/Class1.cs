using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp2
{
    public class MyAdapter : BaseAdapter<ListItem>
    {
        private Activity _Context;
        private List<ListItem> _data;

        public MyAdapter(Activity context, List<ListItem> data)
        {
            this._Context = context;
            this._data = data;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ListItem this[int position]
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
                view = _Context.LayoutInflater.Inflate(Resource.Layout.Activity2, null);
            }
            view.FindViewById<TextView>(Resource.Id.textView1).Text = _data[position].Name;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = _data[position].Count.ToString();
            return view;
        }
    }
}