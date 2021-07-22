using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Web.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public bool IsPublic { get; set; }

        public bool IsFriend { get; set; }

        public bool IsReceivedRequest { get; set; }

        public bool IsSendedRequest { get; set; }
    }
}
