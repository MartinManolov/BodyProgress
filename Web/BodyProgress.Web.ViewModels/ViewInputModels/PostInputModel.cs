using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    public class PostInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(300)]
        public string TextContent { get; set; }

        public bool IsPublic { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
