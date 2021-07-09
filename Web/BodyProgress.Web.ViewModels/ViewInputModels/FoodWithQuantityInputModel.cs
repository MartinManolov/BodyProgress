namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System.ComponentModel.DataAnnotations;

    public class FoodWithQuantityInputModel
    {
        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string FoodName { get; set; }

        [Range(1, 5000)]
        public int Quantity { get; set; }

        [Range(1, 800)]
        public int KcalPer100g { get; set; }

        [Range(1, 100)]
        public int CarbsPer100g { get; set; }

        [Range(1, 100)]
        public int ProteinPer100g { get; set; }

        [Range(1, 100)]
        public int FatPer100g { get; set; }
    }
}
