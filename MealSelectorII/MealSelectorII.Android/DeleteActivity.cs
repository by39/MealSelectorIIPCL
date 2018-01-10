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
using SQLite;
using System.IO;
using Android.Support.V7.App;

namespace MealSelectorII.Droid
{
    [Activity(Label = "DeleteActivity")]
    public class DeleteActivity : AppCompatActivity
    {

        List<string> mealList = new List<string>();

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeletePage);

            //Set the toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.delete_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Delete from the list";

            //Set batabase and connecting
            refreshList();

            // Create your application here
            ListView meal_list = FindViewById<ListView>(Resource.Id.meal_List);

            //show the food in list vew box
            ArrayAdapter<string> adapter;
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mealList);
            meal_list.Adapter = adapter;

            meal_list.ItemClick += meal_listItemClick;

           
        }
        
        public void meal_listItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Mealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<Meal>();
            var table = dbM.Table<Meal>();

            Console.WriteLine("You select: " + e.Position);
            Console.WriteLine("The item is: " + mealList[e.Position]);
            //delete from database
            string food = mealList[e.Position];

            Android.App.AlertDialog.Builder message = new Android.App.AlertDialog.Builder(this);

            message.SetTitle("Delete it?");
            message.SetMessage("Are you sure to delete "+ food + " from you meal list?");
            message.SetPositiveButton("YES", (c, ev) => 
            {
                var data_Del = table.Where(x => x.Food == food).FirstOrDefault();
                dbM.Delete(data_Del);
                //reload the list
                Intent intent = new Intent(this, typeof(DeleteActivity));
                this.StartActivity(intent);
            });
            message.SetNegativeButton("NO", (c, ev) => 
            {
                //reload the list
                Intent intent = new Intent(this, typeof(DeleteActivity));
                this.StartActivity(intent);
            });
            message.Show();
        }
        
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            
            MenuInflater.Inflate(Resource.Menu.toolbar_back, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.menu_back)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        public void refreshList()
        {
            mealList.Clear();
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Mealdb.db3");
            var dbM = new SQLiteConnection(dbPath);
            dbM.CreateTable<Meal>();
            var table = dbM.Table<Meal>();

            foreach (var item in table)
            {
                Meal myMeal = new Meal(item.Food);
                mealList.Add(myMeal.Food);
            }
        }
    }
}