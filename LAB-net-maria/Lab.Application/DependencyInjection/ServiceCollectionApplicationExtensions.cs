using Lab.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lab.Application.GraphQL;
using Lab.Application.Models.DTOs.GraphQL;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Mongo;
using Lab.Application.Interfaces.Logical;


namespace Lab.Application.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IServiceCollection AddApplication(
             this IServiceCollection services, ConfigurationManager config)
        {
            
            services.AddScoped<IUserAccessService, UserAccessService>();
            services.AddSingleton<IWebSocketService, WebSocketService>();
            services.AddScoped<IUserAttributesService, UserAttributesService>();
          
            services
                 .AddGraphQLServer()
                 .AddQueryType<GraphQueries>()
                 .AddMutationType<GraphMutations>()
                 .AddType<UserType>()
                 .AddProjections()
                 .AddFiltering()
                 .AddSorting();
        
            services.AddAuthorization();
            services.AddAuthentication();
            return services;
        }
    }
}
