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

namespace MealSelectorII.Droid
{
    [Activity(Label = "AdvanceActivity")]
    public class AdvanceActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdvancePage);
            // Create your application here

            //Set the toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.advanecd_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Advanced";

            AdView myAd = FindViewById<AdView>(Resource.Id.myAd);
            AdRequest adReq = new AdRequest.Builder().Build();
            myAd.LoadAd(adReq);
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
    }
}