/*
    @file Program.cs
    @brief Program class
*/

using MyApp.Interface;
using MyApp.Services;

namespace MyApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Services.AddSingleton<IWelcomService, WelcomService>();
        //builder.Services.AddScoped<IWelcomService, WelcomService>();
        builder.Services.AddTransient<IWelcomService, WelcomService>();
        var app = builder.Build();

        app.MapGet("/", (IWelcomService service1, IWelcomService service2) => 
        {
            var message1 = service1.GetWelcomeMessage();
            var message2 = service2.GetWelcomeMessage();

            return $"{message1}\n{message2}";
        });

        app.Run();
    }
}
