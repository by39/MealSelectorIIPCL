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
using Android.Support.V7.App;
using Android.Gms.Ads;
using System.IO;
using SQLite;

namespace MealSelectorII.Droid
{
    [Activity(Label = "AdvDeleteActivity")]
    public class AdvDeleteActivity : AppCompatActivity
    {

        List<string> delAdvList = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdveDelete);
            // Create your application here
            AdView myAd = FindViewById<AdView>(Resource.Id.myAd);
            AdRequest adReq = new AdRequest.Builder().Build();
            myAd.LoadAd(adReq);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.delete_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Advanced delete";

            Spinner delSpinner = FindViewById<Spinner>(Resource.Id.del_spinner);
            ListView delList = FindViewById<ListView>(Resource.Id.adv_meal_list);

            refreshList("all");

            delSpinner.ItemSelected += delegate 
            {
                refreshList(delSpinner.SelectedItem.ToString());
            };

            delList.ItemClick += delListItemClick;
        }

        public void delListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Spinner delSpinner = FindViewById<Spinner>(Resource.Id.del_spinner);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AdvMealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<AdvanceMeal>();
            var table = dbM.Table<AdvanceMeal>();

            string food = delAdvList[e.Position];

            Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);

            message.SetTitle("Delete it?");
            message.SetMessage("Are you sure to delete " + food + " from you meal list?");
            message.SetPositiveButton("YES", (c, ev) =>
            {
                var data_Del = table.Where(x => x.Food == food).FirstOrDefault();
                dbM.Delete(data_Del);

                refreshList(delSpinner.SelectedItem.ToString());

            });
            message.SetNegativeButton("NO", (c, ev) =>
            {

                refreshList(delSpinner.SelectedItem.ToString());
            });
            message.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.adv_delete, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_back)
            {
                Intent intent = new Intent(this, typeof(AdvanceActivity));
                this.StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        public void refreshList(string sort)
        {
            delAdvList.Clear();

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AdvMealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<AdvanceMeal>();
            var table = dbM.Table<AdvanceMeal>();

            foreach (var item in table)
            {
                if(sort == "Vege")
                {
                    if (item.Sort == "Vege")
                    {
                        AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                        delAdvList.Add(myAdvMeal.Food);
                    }
                }
                else if(sort == "Meat")
                {
                    if (item.Sort == "Meat")
                    {
                        AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                        delAdvList.Add(myAdvMeal.Food);
                    }
                }
                else
                {
                    AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                    delAdvList.Add(myAdvMeal.Food);
                }
            }

            ListView delList = FindViewById<ListView>(Resource.Id.adv_meal_list);

            ArrayAdapter<string> adapter;
            adapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleListItem1, delAdvList);
            delList.Adapter = adapter;
        }

       
    }
}