namespace BodyProgress.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class PostViewModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string OwnerUsername { get; set; }

        public string OwnerProfilePicture { get; set; }

        public bool IsLiked { get; set; }

        public string TextContent { get; set; }

        public string ImageUrl { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public List<LikeViewModel> Likes { get; set; }
    }
}
