namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System.ComponentModel.DataAnnotations;

    public class CommentInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string TextContent { get; set; }

        public string PostId { get; set; }
    }
}
