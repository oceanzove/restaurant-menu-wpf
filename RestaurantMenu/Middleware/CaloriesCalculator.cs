using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMenu.Middleware
{
    public class CaloriesCalculator
    {
        public static decimal CalculateCalories(decimal proteins, decimal fats, decimal carbohydrates)
        {
            return (proteins * 4) + (fats * 9) + (carbohydrates * 4);
        }
    }
}
