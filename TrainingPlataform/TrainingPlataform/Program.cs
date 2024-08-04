using Microsoft.EntityFrameworkCore;
using Training.Application.AutoMapper;
using Training.Data.Context;
using Training.Swagger;
using Training.IoC;
using System.Text;
using Training.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to context.
builder.Services.AddDbContext<TrainingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TrainingDB")
    ).EnableSensitiveDataLogging());

// Register application services.
NativeInjector.RegisterServices(builder.Services);

builder.Services.AddAutoMapper(typeof(AutoMapperSetup));
builder.Services.AddSwaggerConfiguration();

#region Authentication

    var key = Encoding.ASCII.GetBytes(Settings.Secret);
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region Authentication

app.UseAuthentication();
app.UseAuthorization();

#endregion

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
