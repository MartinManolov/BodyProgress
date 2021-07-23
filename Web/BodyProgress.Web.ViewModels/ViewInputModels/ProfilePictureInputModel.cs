using BodyProgress.Web.ViewModels.ValidationAtributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    public class ProfilePictureInputModel
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}
