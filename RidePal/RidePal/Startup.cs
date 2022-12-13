using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RidePal.Data;
using RidePal.Data.DataInitialize;
using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using RidePal.Services.Services;
using RidePal.Web.Helpers;
using RidePal.Web.MappingConfig;
using RidePal.WEB.Controllers;
using RidePal.WEB.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
                options.UseSqlServer(Configuration.GetConnectionString("Local"));
                options.EnableSensitiveDataLogging();
            });

            services.AddHttpClient<IBingMapsServices, BingMapsServices>(options =>
             {
                 options.BaseAddress = new Uri("http://dev.virtualearth.net/REST/v1/");
                 options.DefaultRequestHeaders.Add("Accept", "application/.json");
             });

            services.AddHttpClient<IPixabayServices, PixabayServices>();
            services.AddTransient<IClaimsTransformation, AddClaimsTransformation>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
                        {
                            options.LoginPath = "/Auth/Login";
                            options.Cookie.Name = "auth_cookie";
                            options.SlidingExpiration = true;
                            options.ExpireTimeSpan = TimeSpan.FromDays(7);
                        })
            .AddGoogle(options =>
                        {
                            options.Events.OnRedirectToAuthorizationEndpoint = context =>
                            {
                                context.Response.Redirect(context.RedirectUri + "&prompt=consent");
                                return Task.CompletedTask;
                            };
                            options.ClientId = ApiSecrets.GoogleClientId;
                            options.ClientSecret = ApiSecrets.GoogleClientSecret;
                            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

                            options.Events.OnTicketReceived = ctx =>
                            {
                                var userEmail = ctx.Principal.FindFirstValue(ClaimTypes.Email);
                                var picture = ctx.Principal.FindFirstValue("pictureUrl");

                                //Check the user exists in database and if not create.
                                return Task.CompletedTask;
                            };
                        });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddControllers();
            services.AddAutoMapper(cfg => cfg.AddProfile<RidePalProfile>());
            services.AddSwaggerGen();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IEmailService, EmailServices>();
            services.AddScoped<ITrackServices, TrackServices>();
            services.AddScoped<ITripServices, TripServices>();
            services.AddScoped<IFetchSongs, FetchSongs>();
            services.AddScoped<IAuthHelper, AuthHelper>();
            services.AddScoped<IAlbumService, AlbumServices>();
            services.AddScoped<IArtistService, ArtistServices>();
            services.AddScoped<IGenreService, GenreServices>();
            services.AddScoped<IPlaylistServices, PlaylistServices>();
            services.AddScoped<ITripServices, TripServices>();
            services.AddScoped<IAWSCloudStorageService, AWSCloudStorageServices>();
            services.AddScoped<ISearchService, SearchServices>();

            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RidePal V1");
            });

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "artists",
                 pattern: "artist/top",
                 defaults: new { controller = "Artists", action = "Top" });
                endpoints.MapControllerRoute(name: "confirmEmails",
                pattern: "/ConfirmEmail",
                defaults: new { controller = "Auth", action = "ConfirmEmail" });
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