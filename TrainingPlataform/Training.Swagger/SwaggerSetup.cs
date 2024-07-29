using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Training.Swagger
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Training Plataform",
                    Version = "v1",
                    Description = "Documentação suporte para uso de nossas API's.",
                    Contact = new OpenApiContact
                    {
                        Name = "Gubio Garcia",
                        Email = "gubiogarcaia@gmail.com"
                    }
                });

                string xmlPath = Path.Combine("wwwroot", "api-doc.xml");
                opt.IncludeXmlComments(xmlPath);
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            return app.UseSwagger().UseSwaggerUI(c =>
            {
            c.RoutePrefix = "documentation";
            c.SwaggerEndpoint("../swagger/v1/swagger.json", "API v1");
            });
        }
    }
}
