using CourseWorkAd.DBContext;
using CourseWorkAd.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CourseWorkAd
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
            services.AddDbContext<ApplicationDBContext>(options =>
            {

                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration["ConnectionString"], sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                });

                
            });

           
            services.AddScoped<ValidFavour>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.LoginPath = "/Home/login";
                    }
                );


            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            /*var mvcBuilder = services.AddControllersWithViews();
            mvcBuilder.AddRazorRuntimeCompilation();*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                /* app.UseExceptionHandler("/error.html");*/
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /*app.UseRouting();*/

            app.UseFileServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}"
                    );
            });


        }
    }
}
