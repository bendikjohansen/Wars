using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using Serilog;
using Wars.Resources;
using Wars.Users;
using Wars.Villages;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJwtBearer(options => options.SigningKey = builder.Configuration["Auth:JwtSecret"]);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

List<Assembly> mediatrAssemblies = [typeof(Program).Assembly];
builder.Services.AddUserModuleServices(builder.Configuration, logger);
builder.Services.AddVillageModuleServices(builder.Configuration, logger, mediatrAssemblies);
builder.Services.AddResourceModuleServices(builder.Configuration, logger, mediatrAssemblies);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(mediatrAssemblies.ToArray()));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();

public partial class Program;
