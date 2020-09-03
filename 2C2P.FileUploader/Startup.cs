using _2C2P.FileUploader.Data;
using _2C2P.FileUploader.Extensions;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Interfaces.Managers;
using _2C2P.FileUploader.Interfaces.Repositories;
using _2C2P.FileUploader.Managers;
using _2C2P.FileUploader.Mappers.MapperProfile;
using _2C2P.FileUploader.Models.ConfigurationOptions;
using _2C2P.FileUploader.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace _2C2P.FileUploader
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

            services.AddMvc();

            services.Configure<AppConfiguration>(Configuration.GetSection(AppConfiguration.ConfigurataionName));

            services.AddDbContext<TransactionEntitiesDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TransactionEntitiesConnection")));

            services.AddScoped<IFileUploadManager, FileUploadManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TransactionEntitiesDbContext>>();
            services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionManager, TransactionManager>();

            services.AddAutoMapper(
                typeof(TransactionEntiryMapperProfile), 
                typeof(TransactionUploadMapperProfile));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.VerifyConfiguration();

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
