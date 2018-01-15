using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MealSelectorII
{
    public class AdvanceMeal
    {
        //public int Id { get; set; }
        [PrimaryKey]
        public string Food { get; set; }
        public string Sort { get; set; }

        public AdvanceMeal(string food, string sort)
        {
            this.Food = food;
            this.Sort = sort;
        }
        public AdvanceMeal()
        {

        }
    }
}
