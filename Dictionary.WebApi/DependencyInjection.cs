using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Dictionary.WebApi
{
    public static class DependencyInjection
    {
        public static void WebApiDependencyInjection(this IServiceCollection service)
        {
            service.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dictionary", Version = "v1" });

                 c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                 {
                     Name = "X-Api-Key",
                     In = ParameterLocation.Header,
                     Type = SecuritySchemeType.ApiKey,
                     Description = "API Key authentication"
                 });

                 c.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                new string[] {}
            }
         });

                 var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                 var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                 c.IncludeXmlComments(xmlPath);
             });

        }
    }
}