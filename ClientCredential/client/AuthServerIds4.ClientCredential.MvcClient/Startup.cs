using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AuthServerIds4.ClientCredential.MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string identityServerUrl = "https://localhost:44366/";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

           // services.AddAuthentication(options =>
           // {
           //     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           //     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
           //     options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
           // })
           //.AddCookie()
           //.AddOpenIdConnect(options =>
           //{
           //    options.SignInScheme = "Cookies";
           //    options.Authority = identityServerUrl;
           //    options.RequireHttpsMetadata = true;
           //    options.ClientId = "clientcredentialId";
           //    options.ClientSecret = "clientCredentialSecret";
               
           //    options.Scope.Add("clientcreapi");
           //    options.Scope.Add("profile");
           //    options.Scope.Add("offline_access");
           //    options.SaveTokens = true;

           //     // Set the correct name claim type
           //     options.TokenValidationParameters = new TokenValidationParameters
           //    {
           //        NameClaimType = "name"

           //    };
           //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
