using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;
using System.Collections.Generic;
using System.IO;
using Android.Support.V7.App;
using Android.Gms.Ads;


namespace MealSelectorII.Droid
{
	[Activity (Label = "MealSelectorII.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            //Set batabase and connecting
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Mealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<Meal>();

            List<Meal> mealList = new List<Meal>();
            var table = dbM.Table<Meal>();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Main page";

            // Get our button from the layout resource,
            // and attach an event to it
            EditText addText = FindViewById<EditText>(Resource.Id.add_text);
            Button addButton = FindViewById<Button>(Resource.Id.add_button);
            Button delButton = FindViewById<Button>(Resource.Id.del_button);
            TextView showFeed = FindViewById<TextView>(Resource.Id.show_feed);
            Button getButton = FindViewById<Button>(Resource.Id.get_button);

            AdView myAd = FindViewById<AdView>(Resource.Id.myAd);
            AdRequest adReq = new AdRequest.Builder().Build();
            myAd.LoadAd(adReq);



            addButton.Click += delegate
            {
                Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);
                string food = addText.Text;
                if(addText.Text != "")
                {
                    if(!IsContain(food))
                    {
                        Meal myNewMeal = new Meal(food);

                        dbM.Insert(myNewMeal);
                        message.SetTitle("Done");
                        message.SetMessage("You add a food " + food + " in to you list.");
                        message.SetNegativeButton("OK", (c, ev) => { });
                        message.Show();

                        addText.Text = "";
                    }
                    else
                    {
                        message.SetTitle("Opssss");
                        message.SetMessage(food + " is in the list already.");
                        message.SetNegativeButton("OK", (c, ev) => { });
                        message.Show();
                    }
                }
                else
                {
                    message.SetTitle("Warming");
                    message.SetMessage("Empty input..");
                    message.SetNegativeButton("OK", (c, ev) => { });
                    message.Show();
                }  
            };

            getButton.Click += delegate 
            {
                Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);
                mealList.Clear();
                foreach (var item in table)
                {
                    Meal myMeal = new Meal(item.Food);
                    mealList.Add(myMeal);
                }


                int number = mealList.Count;
                if (number != 0)
                {
                    Random rand = new Random();
                    int randMeal = rand.Next(0, number);

                    showFeed.Text = "Your meal is: " + mealList[randMeal].Food;
                    message.SetTitle("Meal time!!");
                    message.SetMessage("You have " + mealList[randMeal].Food + " for you meal..");
                    message.SetNegativeButton("OK", (c, ev) => { });
                    message.Show();
                }
            };

            delButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(DeleteActivity));
                this.StartActivity(intent);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.menu_advn)
            {
                Intent intent = new Intent(this, typeof(AdvanceActivity));
                this.StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        public bool IsContain(string food)
        {
            bool isCont = false;

            //Set batabase and connecting
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Mealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<Meal>();
            var table = dbM.Table<Meal>();

            List<Meal> checkList = new List<Meal>();

            foreach (var item in table)
            {
                Meal myMeal = new Meal(item.Food);
                checkList.Add(myMeal);
            }

            for (int i = 0; i<checkList.Count; i++)
            {
                if(checkList[i].Food == food)
                {
                    isCont = isCont || true;
                }
            }
            return isCont;
        }
    }
}


