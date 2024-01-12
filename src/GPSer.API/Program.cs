using GPSer.Core.Automapper;
using GPSer.Core.Commands;
using GPSer.Core.Options;
using GPSer.Core.State;
using GPSer.Data;
using GPSer.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

Log.Logger = CreateSerilogLogger(builder.Configuration);
builder.Host.UseSerilog();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateDeviceCommand).Assembly);
});

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.(\"bearer {token}\")",
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "JWTToken_Auth_API",
//        Version = "v1"
//    });
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "JWT Authorization header using the Bearer scheme.(\"bearer {token}\")",
//    });
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

builder.Services.AddDbContext<GPSerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<GPSerDbContext>()
    //.AddDefaultTokenProviders()
    .AddApiEndpoints();

//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<GPSerDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton<IRemoteClientState, RemoteClientState>();
builder.Services.AddSingleton<IDeviceState, DeviceState>();

//builder.Services.AddHostedService<MQTTLocationWorker>();
//builder.Services.AddHostedService<MQTTComandReaderWorker>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = configuration["JWT:Audience"],
//        ValidIssuer = configuration["JWT:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
//    };
//});


builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.Jwt))
    .ValidateDataAnnotations();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    Log.Information("Applying Migrations...");
    var db = scope.ServiceProvider.GetRequiredService<GPSerDbContext>();
    db.Database.Migrate();
    Log.Information("Migrations applied");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapIdentityApi<IdentityUser>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("API started...");
app.Run();

static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}
