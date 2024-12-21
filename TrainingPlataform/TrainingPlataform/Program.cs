using Microsoft.EntityFrameworkCore;
using Training.Application.AutoMapper;
using Training.Data.Context;
using Training.Swagger;
using Training.IoC;
using System.Text;
using Training.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Training.Auth.Services;
using Training.Application.Mapper;
using Template.CrossCutting.ExceptionHandler.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to context.
builder.Services.AddDbContext<TrainingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TrainingDB"))
           .EnableSensitiveDataLogging());

// Register application services.
NativeInjector.RegisterServices(builder.Services);

builder.Services.AddAutoMapper(typeof(AutoMapperSetup));
builder.Services.AddSwaggerConfiguration();

builder.Services.AddScoped<ManualMapperSetup>();

// Configure JWT settings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

#region Authentication

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
    {   // configuração objeto JwtBearer
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Determinar se o ambiente é "Desenvolvimento" ou similar
var includeStackTrace = app.Environment.IsEnvironment(Environments.Development) ||
                        app.Environment.IsEnvironment("Testing");

app.UseExceptionHandlerMiddleware(includeStackTrace);

var serviceProvider = builder.Services.BuildServiceProvider();
var TokenJwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
TokenService.Initialize(TokenJwtSettings);

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region Authentication

app.UseAuthentication();
app.UseAuthorization();

#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
