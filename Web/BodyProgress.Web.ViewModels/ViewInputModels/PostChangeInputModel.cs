namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System.ComponentModel.DataAnnotations;

    public class PostChangeInputModel
    {
        [Required]
        public string PostId { get; set; }

        [MinLength(3)]
        [MaxLength(300)]
        public string TextContent { get; set; }

        public bool IsPublic { get; set; }
    }
}
