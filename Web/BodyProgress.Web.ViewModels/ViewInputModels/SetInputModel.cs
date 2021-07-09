namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System.ComponentModel.DataAnnotations;

    public class SetInputModel
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string ExerciseName { get; set; }

        [Range(0, 300)]
        public ushort Reps { get; set; }

        [Range(0, 500)]
        public double Weight { get; set; }
    }
}
