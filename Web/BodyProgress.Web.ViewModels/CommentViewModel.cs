namespace BodyProgress.Web.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CommentViewModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string OwnerName { get; set; }

        public string TextContent { get; set; }
    }
}
