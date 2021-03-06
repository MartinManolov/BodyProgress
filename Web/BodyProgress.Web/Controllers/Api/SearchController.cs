namespace BodyProgress.Web.Controllers.Api
{
    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("/api/[controller]")]
    [Authorize]
    public class SearchController : ApiBaseController
    {
        private readonly IUsersService usersService;

        public SearchController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public IActionResult Search()
        {
            try
            {
                string term = this.HttpContext.Request.Query["term"].ToString();
                var users = this.usersService.Search(term, 8);
                return this.Ok(users);
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
