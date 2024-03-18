using FlightSearchAggregator;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Services;
using FlightSearchAggregator.Services.Bookings;
using FlightSearchAggregator.Services.Providers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureAppSettings(builder.Services, builder.Configuration);
ConfigureServices(builder.Services, builder.Configuration);
ConfigureLogging(builder.Host);

var app = builder.Build();

ConfigureApp(app);

app.Run();

void ConfigureAppSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<AppSettings>(configuration);
}

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.EnableAnnotations();
        c.DescribeAllParametersInCamelCase();

        c.UseInlineDefinitionsForEnums();

    });
    HttpClientConfig(services, configuration);

    services.AddMemoryCache();
    services.AddAutoMapper(typeof(Program));
    services.AddLogging();
    services.AddScoped<FlightService>();
    services.AddScoped<IFlyQuestService, FlyQuestService>();
    services.AddScoped<IBookingService, FlyQuestService>();
    services.AddScoped<ISkyTrailsService, SkyTrailsService>();
    services.AddScoped<IBookingService, SkyTrailsService>();
    services.AddScoped<BookFlightService>();
    services.AddScoped<BookingServiceFactory>();
    services.AddScoped<HttpService>();
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
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}

void HttpClientConfig(IServiceCollection serviceCollection, ConfigurationManager configuration)
{
    var appSettings = configuration.Get<AppSettings>();

    serviceCollection.AddHttpClient("SkyTrailsClient", client =>
    {
        client.BaseAddress = new Uri(appSettings.SkyTrailsBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    serviceCollection.AddHttpClient("FlyQuestClient", client =>
    {
        client.BaseAddress = new Uri(appSettings.FlyQuestBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });
}