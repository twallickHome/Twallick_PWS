using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TedWallick.Models;

[assembly: HostingStartup(typeof(Ted Wallick.Areas.Identity.IdentityHostingStartup))]
namespace Ted Wallick.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TedWallickContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TedWallickContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<TedWallickContext>();
            });
        }
    }
}