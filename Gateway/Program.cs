using System.Drawing;
using Gateway.Aggregator;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddTransientDefinedAggregator<PilotsDestinationsAggregator>();



var app = builder.Build();
app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();