namespace BodyProgress.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Data.Common.Models;

    public class Meal : BaseDeletableModel<string>
    {
        public Meal()
        {
            this.Id = Guid.NewGuid().ToString();
            this.FoodsMealsQuantities = new HashSet<FoodsMealsQuantity>();
        }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ICollection<FoodsMealsQuantity> FoodsMealsQuantities { get; set; }

    }
}
