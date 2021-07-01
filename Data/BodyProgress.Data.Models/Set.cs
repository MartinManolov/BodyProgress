namespace BodyProgress.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Data.Common.Models;

    public class Set : BaseDeletableModel<string>
    {
        public Set()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string WorkoutId { get; set; }

        public virtual Workout Workout { get; set; }

        [Required]
        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public ushort Reps { get; set; }

        public double Weight { get; set; }
    }
}
