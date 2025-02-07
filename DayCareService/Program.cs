using DayCare.Core.Interfaces;
using DayCare.Core.Services;
using DayCare.Service;
using DayCare.Service.Middlewares;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Serilog.Events.LogEventLevel MinimalLoggingLevel = (Serilog.Events.LogEventLevel)Enum.Parse(typeof(Serilog.Events.LogEventLevel), builder.Configuration.GetValue<string>("Log:MinimumLevel"), true);
Log.Logger = new LoggerConfiguration()
                                 .MinimumLevel.Information()
                                 .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                                 .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                                 .WriteTo.File(path: builder.Configuration.GetValue<string>("Log:Path"),rollingInterval:RollingInterval.Day,
                                 restrictedToMinimumLevel: MinimalLoggingLevel).CreateLogger();
#region Dependencies
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddScoped<IDayCareDbRepository, DayCareDbService>();
#endregion
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DayCareService", Version = "v1" });
});
builder.Services.AddControllers();
builder.Services.AddSerilog();
var hosts = builder.Configuration.GetSection("Cors:AllowedOrigins")
                                    .Get<List<string>>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      builder =>
                      {
                          builder.WithOrigins(hosts.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                      });
});
var dbConnAppSettings = builder.Configuration.GetSection("DbConnectionStrings").GetChildren().ToDictionary(x=>x.Key,x=>x.Value);
Bootstrapper.BootupDBContext(dbConnAppSettings);


var app = builder.Build();
if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DayCareService v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.MapControllers();
app.UseResponse();
Log.Information("Starting web host");
app.Run();
