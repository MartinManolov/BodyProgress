using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BodyProgress.Web.Areas.Identity.IdentityHostingStartup))]

namespace BodyProgress.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
