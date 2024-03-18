using FlightSearchAggregator;
using FlightSearchAggregator.Services;
using FlightSearchAggregator.Services.Bookings;
using FlightSearchAggregator.Services.Providers;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ConfigureAppSettings(builder.Services, builder.Configuration);
ConfigureServices(builder.Services);
ConfigureLogging(builder.Host);

var app = builder.Build();

ConfigureApp(app);

app.Run();

void ConfigureAppSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<AppSettings>(configuration);
}

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        c.UseInlineDefinitionsForEnums();
    });

    services.AddMemoryCache();
    services.AddAutoMapper(typeof(Program));
    services.AddLogging();
    services.AddScoped<FlightService>();
    services.AddScoped<IFlyQuestService, FlyQuestService>();
    services.AddScoped<ISkyTrailsService, SkyTrailsService>();
 
}

void ConfigureLogging(IHostBuilder hostBuilder)
{
    hostBuilder.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 10_485_760,
            rollOnFileSizeLimit: true,
            retainedFileCountLimit: null,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));
}

void ConfigureApp(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}
