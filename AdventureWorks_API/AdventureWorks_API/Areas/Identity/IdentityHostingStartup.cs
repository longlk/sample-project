using System;
using AdventureWorks_API.Areas.Identity.Data;
using AdventureWorks_API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AdventureWorks_API.Areas.Identity.IdentityHostingStartup))]
namespace AdventureWorks_API.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AdventureWorks_APIContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AdventureWorksDatabase")));

                services.AddDefaultIdentity<AdventureWorks_APIUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AdventureWorks_APIContext>();
            });
        }
    }
}