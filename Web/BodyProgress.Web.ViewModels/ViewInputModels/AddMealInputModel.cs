namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Web.ViewModels.ValidationAtributes;

    public class AddMealInputModel
    {
        public AddMealInputModel()
        {
            this.FoodsWithQuantities = new List<AddFoodWithQuantityInputModel>();
        }

        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string MealName { get; set; }

        [DateBetween2020andUtsNow]
        public DateTime Date { get; set; }

        public List<AddFoodWithQuantityInputModel> FoodsWithQuantities { get; set; }
    }
}
