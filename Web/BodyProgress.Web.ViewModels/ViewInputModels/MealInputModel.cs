namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Web.ViewModels.ValidationAtributes;

    public class MealInputModel
    {
        public MealInputModel()
        {
            this.FoodsWithQuantities = new List<FoodWithQuantityInputModel>();
        }

        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string MealName { get; set; }

        [DateBetween2020andUtsNow]
        public DateTime Date { get; set; }

        public List<FoodWithQuantityInputModel> FoodsWithQuantities { get; set; }
    }
}
