namespace BodyProgress.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Data.Common.Models;

    public class Food : BaseDeletableModel<string>
    {
        public Food()
        {
            this.Id = Guid.NewGuid().ToString();
            this.FoodsMealsQuantities = new HashSet<FoodsMealsQuantity>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public int KcalPer100G { get; set; }

        public int CarbsPer100G { get; set; }

        public int ProteinPer100G { get; set; }

        public int FatPer100G { get; set; }

        public virtual ICollection<FoodsMealsQuantity> FoodsMealsQuantities { get; set; }
    }
}
