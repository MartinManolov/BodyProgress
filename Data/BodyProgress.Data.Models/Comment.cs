namespace BodyProgress.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BodyProgress.Data.Common.Models;

    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Required]
        [MaxLength(150)]
        public string TextContent { get; set; }

        [Required]
        [ForeignKey("Post")]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
