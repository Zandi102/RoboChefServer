using System;
using MongoDB.Driver;

namespace RoboChefServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var connectionString = "mongodb+srv://alex:pallozzi@cluster0.xrwnatt.mongodb.net/?retryWrites=true&w=majority";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("RoboChef");


                Console.Write("Database Connected");
                services.AddSingleton(database);
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

