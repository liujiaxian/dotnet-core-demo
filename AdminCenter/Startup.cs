using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminCenter.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace AdminCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

           
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your secret goes here")),

                    ValidateIssuer = true,
                    ValidIssuer = "The name of the issuer",

                    ValidateAudience = true,
                    ValidAudience = "The name of the audience",

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

 //           services.AddMvc().ConfigureApplicationPartManager(manager =>
 //           {
 //               //移除ASP.NET CORE MVC管理器中默认内置的MetadataReferenceFeatureProvider，该Provider如果不移除，还是会引发InvalidOperationException: Cannot find compilation library location for package 'MyNetCoreLib'这个错误
 //               manager.FeatureProviders.Remove(manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider));
 //               //注册我们定义的ReferencesMetadataReferenceFeatureProvider到ASP.NET CORE MVC管理器来代替上面移除的MetadataReferenceFeatureProvider
 //               manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
 //           });

 //           //添加认证Cookie信息
 //           services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
 //.AddCookie(options =>
            //{
            //  options.LoginPath = new PathString("/account/login");
            //  options.AccessDeniedPath = new PathString("/account/denied");
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            //验证中间件
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
