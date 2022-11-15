using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieForum.Web.MappingConfig;
using RidePal.Data;
using System;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using RidePal.Data.DataInitialize;
using RidePal.Data.DataInitialize.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using RidePal.Web.Helpers;

namespace RidePal
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

            services.AddControllersWithViews();
            services.AddAutoMapper(cfg => cfg.AddProfile<RidePalProfile>());
            services.AddDbContext<RidePalContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
                options.EnableSensitiveDataLogging();
            });

            services.AddHttpClient<IBingMapsServices, BingMapsServices>(options =>
             {
                 options.BaseAddress = new Uri("http://dev.virtualearth.net/REST/v1/");
                 options.DefaultRequestHeaders.Add("Accept", "application/.json");
             });

            services.AddHttpClient<IFetchSongs, FetchSongs>(options =>
            {
                options.BaseAddress = new Uri("https://api.deezer.com/search/");
                options.DefaultRequestHeaders.Add("Accept", "application/.json");
            });

            services.AddHttpClient<ISpotifyAccountServices, SpotifyAccountServices>(c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            services.AddHttpClient<ISpotifyServices, SpotifyServices>(x =>
            {
                x.BaseAddress = new Uri(" https://api.spotify.com/v1/");
                x.DefaultRequestHeaders.Add("Accept", "application/.json");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.Cookie.Name = "auth_cookie";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            }).AddGoogle(options =>
            {
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=consent");
                    return Task.CompletedTask;
                };
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.Events.OnTicketReceived = ctx =>
                {
                    var userEmail = ctx.Principal.FindFirstValue(ClaimTypes.Email);
                    //Check the user exists in database and if not create.
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
            });

            services.AddControllers();
            services.AddAutoMapper(cfg => cfg.AddProfile<RidePalProfile>());

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITrackServices, TrackServices>();
            services.AddScoped<IAuthHelper, AuthHelper>();
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
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });
        }
    }
}
