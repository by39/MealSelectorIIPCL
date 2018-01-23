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
    [Activity(Label = "AdvanceActivity")]
    public class AdvanceActivity : AppCompatActivity
    {

        List<AdvanceMeal> myAdvMealList = new List<AdvanceMeal>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdvancePage);
            // Create your application here


            //Set database
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AdvMealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<AdvanceMeal>();

            
            var table = dbM.Table<AdvanceMeal>();

            //Set the toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.advanecd_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Advanced";

            AdView myAd = FindViewById<AdView>(Resource.Id.myAd);
            AdRequest adReq = new AdRequest.Builder().Build();
            myAd.LoadAd(adReq);
            
            EditText nameText = FindViewById<EditText>(Resource.Id.food_text);

            Spinner sort = FindViewById<Spinner>(Resource.Id.select);

            Spinner feedSort = FindViewById<Spinner>(Resource.Id.feedSelect);

            Button addBtn = FindViewById<Button>(Resource.Id.add_button);

            Button delBtn = FindViewById<Button>(Resource.Id.delete_button);

            TextView myfeed = FindViewById<TextView>(Resource.Id.feed_result);

            Button feedMe = FindViewById<Button>(Resource.Id.feed_button);

            addBtn.Click += delegate 
            {
                Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);

                string food = nameText.Text;
                string sFood = sort.SelectedItem.ToString();

                if (isContian(food))
                {
                    message.SetTitle("Opssss");
                    message.SetMessage(food + " is in the list already.");
                    message.SetNegativeButton("OK", (c, ev) => { });
                    message.Show();
                }
                else
                {
                    if (nameText.Text == "")
                    {
                        message.SetTitle("Warming");
                        message.SetMessage("Empty meal input.");
                        message.SetNegativeButton("OK", (c, ev) => { });
                        message.Show();
                    }
                    else
                    {
                        if (sort.SelectedItem.ToString().Equals("Select sort of your meal..."))
                        {
                            message.SetTitle("Warming");
                            message.SetMessage("Select a sort of your meal.");
                            message.SetNegativeButton("OK", (c, ev) => { });
                            message.Show();
                        }

                        else
                        {


                            AdvanceMeal myAdvMeal = new AdvanceMeal(food, sFood);
                            dbM.Insert(myAdvMeal);

                            message.SetTitle("Done");
                            message.SetMessage("You add a food " + food + " in to you list.");
                            message.SetNegativeButton("OK", (c, ev) => { });
                            message.Show();

                            nameText.Text = "";
                            sort.SetSelection(0);
                        }
                    }
                }
            };

            delBtn.Click += delegate 
            {
                Intent intent = new Intent(this, typeof(AdvDeleteActivity));
                this.StartActivity(intent);
            };

            feedMe.Click += delegate 
            {
                Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);

                myAdvMealList.Clear();

                string feed_Sort = feedSort.SelectedItem.ToString();

                foreach(var item in table)
                {
                    if (feed_Sort.Equals("Vege"))
                    {
                        if(item.Sort == "Vege")
                        {
                            AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                            myAdvMealList.Add(myAdvMeal);
                            GetMeal();
                        }
                    }
                    else if (feed_Sort.Equals("Meat"))
                    {
                        if(item.Sort == "Meat")
                        {
                            AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                            myAdvMealList.Add(myAdvMeal);
                            GetMeal();
                        }
                    }
                    else if(feed_Sort.Equals("All"))
                    {
                        AdvanceMeal myAdvMeal = new AdvanceMeal(item.Food, item.Sort);
                        myAdvMealList.Add(myAdvMeal);
                        GetMeal();
                    }
                    else
                    {
                        message.SetTitle("Select a sort");
                        message.SetMessage("Select a sort of the meal you prefer.");
                        message.SetNegativeButton("OK", (c, ev) => { });
                        message.Show();
                        break;
                    }
                }

                /*int length = myAdvMealList.Count();

                if(length != 0)
                {
                    Random rand = new Random();
                    int randMeal = rand.Next(0, length);

                    myfeed.Text = "Your meal is " + myAdvMealList[randMeal].Food + " for today!";

                }
                else
                {
                    message.SetTitle("Your don't have anything to eat..");
                    message.SetMessage("The list is empty, add some into your meal list..");
                    message.SetNegativeButton("OK", (c, ev) => { });
                    message.Show();
                }*/
            };
        }

        public void GetMeal()
        {
            Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);

            TextView myfeed = FindViewById<TextView>(Resource.Id.feed_result);
            int length = myAdvMealList.Count();

            if (length != 0)
            {
                Random rand = new Random();
                int randMeal = rand.Next(0, length);

                myfeed.Text = "Your meal is " + myAdvMealList[randMeal].Food + " for today!";

            }
            else
            {
                message.SetTitle("Your don't have anything to eat..");
                message.SetMessage("The list is empty, add some into your meal list..");
                message.SetNegativeButton("OK", (c, ev) => { });
                message.Show();
            }
        }



        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.toolbar_advance, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_advn)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        public bool isContian(string food)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AdvMealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<AdvanceMeal>();
            var table = dbM.Table<AdvanceMeal>();

            bool contain = false;

            List<AdvanceMeal> myAdvMealList = new List<AdvanceMeal>();

            foreach(var item in table)
            {
                AdvanceMeal myAdvM = new AdvanceMeal(item.Food , item.Sort);
                myAdvMealList.Add(myAdvM);
            }

            for(int i = 0; i<myAdvMealList.Count; i++)
            {
                if(myAdvMealList[i].Food == food)
                {
                    contain = contain || true;
                }
                break;
            }
            return contain;
        }
    }
}