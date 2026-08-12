using Microsoft.AspNetCore.Rewrite;
using System;
using MyWebApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace MyWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<IPersonService, PersonService>();

        var app = builder.Build();

        // カスタムミドルウェア
        app.Use(async (context, next) =>
        {
            await next();
            Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
        });

        app.UseRewriter(new RewriteOptions().AddRedirect("history", "about"));
        app.MapGet("/", (IPersonService person) => $"Welcome to Contoso! {person.GetName()}");
        app.MapGet("/about", () => "Contoso was founded in 2000.");

        app.Run();
    }
}
