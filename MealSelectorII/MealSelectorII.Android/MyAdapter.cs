using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MealSelectorII.Droid
{
    class MyAdapter : BaseAdapter<AdvanceMeal>
    {

        private Context myContext;
        private List<AdvanceMeal> advanceMeal;

        public MyAdapter(Context con, List<AdvanceMeal> meaList)
        {
            advanceMeal = meaList;
            myContext = con;
        }

        public override int Count 
        {
            get { return advanceMeal.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override AdvanceMeal this[int position]
        {
            get { return advanceMeal[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //throw new NotImplementedException();
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.ListTittle, null, false);
            }

            TextView txtMeal = row.FindViewById<TextView>(Resource.Id.meal);
            txtMeal.Text = advanceMeal[position].Food;

            TextView txtCateg = row.FindViewById<TextView>(Resource.Id.categ);
            txtCateg.Text = advanceMeal[position].Sort;

            return row;
        }
    }
}