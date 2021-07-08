namespace BodyProgress.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BodyProgress.Data.Common.Models;

    public class BodyStatistic : BaseDeletableModel<string>
    {
        public BodyStatistic()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Range(100, 250)]
        public int Height { get; set; }

        [Range(25, 300)]
        public double Weight { get; set; }

        public double BodyFatPercentage { get; set; }

    }
}
