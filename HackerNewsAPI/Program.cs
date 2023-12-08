using HackerNewsAPI.Clients;
using HackerNewsAPI.Clients.Interfaces;
using HackerNewsAPI.Filters;
using HackerNewsAPI.Repositories;
using HackerNewsAPI.Repositories.Interfaces;
using HackerNewsAPI.Services;
using HackerNewsAPI.Services.Interfaces;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddControllers(options => { options.Filters.Add<HttpResponseExceptionFilter>(); })
            .AddJsonOptions(x => { x.JsonSerializerOptions.PropertyNameCaseInsensitive = true; });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IHackerNewsClientOptions, HackerNewsClientOptions>();

        builder.Services.AddHttpClient<IHackerNewsClient, HackerNewsClient>()
            .ConfigureHttpClient((serviceProvider, httpClient) =>
            {
                var clientConfig = serviceProvider.GetRequiredService<IHackerNewsClientOptions>();
                httpClient.BaseAddress = clientConfig.BaseAddress;
                httpClient.Timeout = TimeSpan.FromSeconds(clientConfig.Timeout);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "HackerNewsApi");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(2))
            .ConfigurePrimaryHttpMessageHandler(x =>
                new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UseCookies = false,
                    AllowAutoRedirect = false,
                    UseDefaultCredentials = true,
                });
        builder.Services.AddScoped<IHackerNewsRepository, HackerNewsRepository>();
        builder.Services.AddTransient<IStoryMapperService, StoryMapperService>();
        builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}