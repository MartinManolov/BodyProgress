namespace BodyProgress.Data.Models
{
    using System;

    using BodyProgress.Data.Common.Models;

    public class FoodsMealsQuantity : BaseDeletableModel<string>
    {
        public FoodsMealsQuantity()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string MealId { get; set; }

        public virtual Meal Meal { get; set; }

        public string FoodId { get; set; }

        public virtual Food Food { get; set; }

        public int Quantity { get; set; }
    }
}
