using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyShare.Kernel;
using MyShare.Sample.Extensions;

namespace MyShare.Sample.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private List<Assembly> _assemblies =new List<Assembly>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var currentAssembly=this.GetType().GetTypeInfo().Assembly;
            var refAssemblyNames= currentAssembly.GetReferencedAssemblies();
            _assemblies = refAssemblyNames.Select(Assembly.Load).ToList();

            // Add framework services.
            services.AddMvc();
            //IDbConnection conn = new MySqlConnection("Server=127.0.0.1;Database=eventsource;Uid=root;Pwd=123456;")
            services.AddSingleton<IMyShareOptions>(MyShareOptions.Instance(services));
            var myShareOptions = services.BuildServiceProvider().GetService<IMyShareOptions>();
            myShareOptions.InitKernel(new SqlConnection(@"Data Source = WH-PC077\MSSQLSERVER2014;Initial Catalog = eventsource;User Id = sa;Password = 95938;"))
                .UseSample();//初始化项目
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
