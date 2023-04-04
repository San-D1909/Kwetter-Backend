using Microsoft.EntityFrameworkCore;
using Polly;
using UserProfileAPI;
using RabbitMQ.Client;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
        builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

        var app = builder.Build();

        // Create polly policy for database connection:
        var policy = Policy.Handle<Exception>().WaitAndRetryForever(
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200), // Wait 200ms between each try.
            onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging.
            {
                Console.WriteLine("Could not connect to database, retrying");
            });

        // Migrate latest database changes during startup
        using (var scope = app.Services.CreateScope())
        {
            // Here is the migration executed inside the polly policy
            policy.Execute(() =>
            {
                var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationContext>();

                dbContext.Database.Migrate();
            });
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", string.Empty);
            });
        }



        app.UseRouting();

        app.UseAuthorization();

        app.UseHttpsRedirection();


        app.MapControllers();


        app.Run();
    }
}