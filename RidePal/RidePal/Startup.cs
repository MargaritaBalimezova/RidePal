using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieForum.Web.MappingConfig;
using RidePal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using System;
using RidePal.Data.DataInitialize;
using RidePal.Data.DataInitialize.Interfaces;

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

            services.AddHttpClient<IBingMapsServices,BingMapsServices>(options =>
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

       
            services.AddControllers();
            services.AddAutoMapper(cfg => cfg.AddProfile<RidePalProfile>());

            services.AddScoped<IUserServices,UserServices>();
            services.AddScoped<ITrackServices, TrackServices>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
