using FlightSearchAggregator;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Services;
using FlightSearchAggregator.Services.Bookings;
using FlightSearchAggregator.Services.Providers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FlightSearchAggregator.Auth;
using FlightSearchAggregator.DbContext;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

ConfigureAppSettings(builder.Services, builder.Configuration);

var appSettings = builder.Configuration.Get<AppSettings>();
builder.WebHost.UseUrls(appSettings.AppBaseUrlHttps, appSettings.AppBaseUrlHttp);

ConfigureServices(builder.Services, builder.Configuration, appSettings);
ConfigureLogging(builder.Host);

var app = builder.Build();

ConfigureApp(app);

var logger = app.Services.GetService<ILogger<Program>>();
logger.LogInformation($"Program run on: {appSettings.AppBaseUrlHttp}, {appSettings.AppBaseUrlHttps}, v 0.04");
app.Run();

void ConfigureAppSettings(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<AppSettings>(configuration);
    CreateTestUser(configuration);
}

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, AppSettings settings)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins(settings.AppOuterUrl)
                .AllowAnyHeader()
                .AllowAnyMethod());
    });

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

    services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

    services.AddEndpointsApiExplorer();
    SwaggerConfig(services);

    HttpClientConfig(services, configuration);

    services.AddScoped<AuthService>(serviceProvider =>
        new AuthService(builder.Configuration));

    services.AddMemoryCache();
    services.AddAutoMapper(typeof(Program));
    services.AddLogging();
    services.AddScoped<FlightService>();
    services.AddScoped<IFlyQuestService, FlyQuestService>();
    services.AddScoped<ISkyTrailsService, SkyTrailsService>();
    services.AddScoped<BookFlightService>();
    services.AddScoped<FlightBookingFactory>();
    services.AddScoped<HttpService>();
    services.AddSwaggerExamplesFromAssemblyOf<Program>();
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
    app.UseCors("AllowSpecificOrigin");
    app.UseRouting();

    // Configure the HTTP request pipeline.
    // if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
    }

    // app.UseHttpsRedirection();

    app.UseAuthentication();
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

void SwaggerConfig(IServiceCollection serviceCollection)
{
    DbEmulation.Users.TryGetValue("testUser@gmail.com", out var user);
    serviceCollection.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer " +
                          user.TokenForExample + "'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.EnableAnnotations();
        c.DescribeAllParametersInCamelCase();

        c.UseInlineDefinitionsForEnums();
        c.ExampleFilters();
    });
}

void CreateTestUser(IConfiguration configuration)
{
    var testEmail = "testUser@gmail.com";
    var passwordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
        .ComputeHash(Encoding.UTF8.GetBytes("test123")));

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        expires: DateTime.Now.AddYears(100),
        signingCredentials: credentials);

    var testUser = new User
    {
        FullName = "Test User",
        Email = testEmail,
        PasswordHash = passwordHash,
        TokenForExample = new JwtSecurityTokenHandler().WriteToken(token)
    };
    DbEmulation.Users.Add(testEmail, testUser);
}