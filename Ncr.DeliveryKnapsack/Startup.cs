using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ncr.DeliveryKnapsack.Configuration;
using Ncr.DeliveryKnapsack.Repositories;
using Ncr.DeliveryKnapsack.Services;

namespace Ncr.DeliveryKnapsack
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Mailer>();
            services.AddTransient<ISolutionEvaluator, SolutionEvaluator>();
            services.AddTransient<IProblemGenerator, ProblemGenerator>();
            services.AddTransient<SolutionRepository>();
            services.AddTransient<RegistrationRepository>();
            services.Configure<MailgunConfiguration>(Configuration.GetSection("Mailgun"));
            services.Configure<DbConfiguration>(Configuration.GetSection("Db"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
