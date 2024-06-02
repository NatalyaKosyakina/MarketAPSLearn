
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Market.Models;
using Market.repo;
using Microsoft.Extensions.FileProviders;

namespace Market
{
    public class Program
    {
        public static WebApplication? GetApplication() //string[] args
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var cfg = config.Build();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.RegisterType<ProductRepository>().As<IProductRepository>();
                cb.Register(c => new MarketContext(cfg.GetConnectionString("db"))).InstancePerDependency();
            });

            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddMemoryCache(op => op.TrackStatistics = true);

            return builder.Build();
            
        }
        public static void Main(string[] args)
        {
            var app = GetApplication();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            //app.UseAuthorization();

            var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilePath);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilePath),
                RequestPath = "/static"
            }) ;

            app.MapControllers();

            app.Run();
        }
    }
}
