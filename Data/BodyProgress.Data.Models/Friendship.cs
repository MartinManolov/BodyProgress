namespace BodyProgress.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BodyProgress.Data.Common.Models;
    using BodyProgress.Data.Models.Enums;

    public class Friendship : BaseDeletableModel<string>
    {
        public Friendship()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Friend")]
        public string FriendId { get; set; }

        public virtual ApplicationUser Friend { get; set; }

        public FriendshipStatus Status { get; set; }
    }
}
