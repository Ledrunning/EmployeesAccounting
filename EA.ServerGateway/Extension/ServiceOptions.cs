using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EA.ServerGateway.Extension;

public static class ServiceOptions
{
    public static void AddSwaggerGenWithOptions(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddSwaggerUi(this WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = "swagger"; // to serve Swagger UI at the app's root

            // prompt for basic auth credentials
            c.DefaultModelsExpandDepth(-1);
            c.DefaultModelExpandDepth(-1);
            c.DefaultModelRendering(ModelRendering.Example);
            c.DisplayRequestDuration();
        });
    }
}