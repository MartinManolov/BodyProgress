using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Web.ViewModels
{
    public class PostViewModel
    {
        public DateTime Date { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUsername { get; set; }

        public string TextContent { get; set; }

        public string ImageUrl { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public List<LikeViewModel> Likes { get; set; }
    }
}
