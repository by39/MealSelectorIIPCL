using System;
using SQLite;

namespace MealSelectorII
{
    public class Meal
    {
        [PrimaryKey]
        public string Food { get; set; }

        public Meal(string food)
        {
            this.Food = food;
        }
        public Meal()
        {

        }
    }
}

