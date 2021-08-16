using System.Linq;
using BodyProgress.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace BodyProgress.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using BodyProgress.Common;
    using BodyProgress.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    return this.Redirect("/Administration/Administration/Feeds");
                }

                return this.Redirect("/Posts/Feeds");
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
