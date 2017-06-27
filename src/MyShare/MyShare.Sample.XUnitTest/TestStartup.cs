using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Sample.Web;

namespace MyShare.Sample.XUnitTest
{
    public class TestStartup : Startup
    {

        public TestStartup(IHostingEnvironment env) : base(env)
        {

        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            ConfigureServices(services);
        }
    }
}