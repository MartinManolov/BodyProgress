namespace BodyProgress.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Workouts = new HashSet<Workout>();
            this.Meals = new HashSet<Meal>();
            this.BodyStatistics = new HashSet<BodyStatistic>();
            this.FriendRequestsMade = new HashSet<Friendship>();
            this.FriendRequestsAccepted = new HashSet<Friendship>();
            this.Posts = new HashSet<Post>();
        }

        public virtual ICollection<Workout> Workouts { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }

        public virtual ICollection<BodyStatistic> BodyStatistics { get; set; }

        public virtual ICollection<Friendship> FriendRequestsMade { get; set; }

        public virtual ICollection<Friendship> FriendRequestsAccepted { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public bool IsPublic { get; set; }

        public string ProfilePicture { get; set; }

        [MaxLength(40)]
        public string Goal { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
