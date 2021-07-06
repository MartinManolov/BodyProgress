using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Web.ViewModels
{
    public class MealViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public List<FoodMealQuantityViewModel> FoodsMealQuantities { get; set; }

        public int KcalPerMeal { get; set; }

        public int CarbsPerMeal { get; set; }

        public int ProteinPerMeal { get; set; }

        public int FatsPerMeal { get; set; }

    }
}
