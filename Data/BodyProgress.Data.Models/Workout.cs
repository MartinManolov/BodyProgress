namespace BodyProgress.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BodyProgress.Data.Common.Models;

    public class Workout : BaseDeletableModel<string>
    {
        public Workout()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Sets = new HashSet<Set>();
        }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
    }
}
