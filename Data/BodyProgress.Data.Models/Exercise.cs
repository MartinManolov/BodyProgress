namespace BodyProgress.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Data.Common.Models;

    public class Exercise : BaseDeletableModel<string>
    {
        public Exercise()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
