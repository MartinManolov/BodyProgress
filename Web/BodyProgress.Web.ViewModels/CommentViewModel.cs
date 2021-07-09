using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BodyProgress.Web.ViewModels
{
     public class CommentViewModel
    {
        public DateTime Date { get; set; }

        public string OwnerName { get; set; }

        public string OwnerId { get; set; }

        public string TextContent { get; set; }
    }
}
