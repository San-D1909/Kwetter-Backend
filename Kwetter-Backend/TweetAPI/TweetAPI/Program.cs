using Microsoft.EntityFrameworkCore;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.ExceptionServices;
using System.Text;
using TweetAPI;
using TweetAPI.Models;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        IConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = "host.docker.internal",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
        };
       IConnection connection = connectionFactory.CreateConnection();

            IModel channel = connection.CreateModel();
        channel.ExchangeDeclare("userExchange", ExchangeType.Topic, true);

        channel.QueueDeclare("user", true, false, false, null);
        channel.QueueBind("user", "userExchange", "userDemo");

        var consumer = new EventingBasicConsumer(channel);
        // define a callback function for incoming messages
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.Unicode.GetString(body);
            Console.WriteLine("Received message: {0}", message);
        };
        channel.BasicConsume("user", true, consumer);


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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();


    }
}