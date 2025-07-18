namespace Kiwi_SDR_Online_Receiver_Logging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Adds services for controllers to the services collection..
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            // Adds endpoints for controller actions..
            app.MapControllers();

            app.Run();
        }
    }
}
