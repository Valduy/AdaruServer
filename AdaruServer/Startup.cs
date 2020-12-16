using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Factories;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.DBRepositories.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdaruServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(mvcOptions => mvcOptions.EnableEndpointRouting = false);

            services.AddScoped<IRepositoryContextFactory, AdaruDBContextFactory>();

            services.AddScoped<IChatRepository>(provider 
                => new ChatRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<IClientRepository>(provider
                => new ClientRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<ICustomerRepository>(provider
                => new CustomerRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<IImageRepository>(provider
                => new ImageRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<IPerformerRepository>(provider
                => new PerformerRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<IProfileRepository>(provider
                => new ProfileRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<IReviewRepository>(provider
                => new ReviewRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<ITagRepository>(provider
                => new TagRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
            services.AddScoped<ITaskRepository>(provider
                => new TaskRepository(Configuration.GetConnectionString("DefaultConnection"),
                    provider.GetService<IRepositoryContextFactory>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
