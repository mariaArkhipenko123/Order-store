using dotenv.net;
using Lab.Domain.Entities;
using Lab.Infrastructure.Context;
using Lab.Application.Interfaces;
using Lab.Infrastructure.Repository;
using Lab.Infrastructure.Repository.Orm_Dapper;
using Lab.Infrastructure.Repository.Orm_EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lab.Application.Interfaces.Repo;
using Lab.Application.Interfaces.Mongo;
using Lab.Infrastructure.Handlers;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Logical;
using Lab.Application.Services;
using Lab.Infrastructure.Services;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lab.Infrastructure.Providers;

namespace Lab.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
             this IServiceCollection services, ConfigurationManager config)
        {
            DotEnv.Load();

            string postgresConnectionString = config["PostgreSQL:ConnectionString"];
            services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(postgresConnectionString));

            string mongoConnectionString = config["MongoDB:URI"];

            services.AddSingleton<MongoDbContext>(provider =>
                new MongoDbContext(mongoConnectionString, "m-arkhipenka"));


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config["Redis:ConnectionString"];
                options.InstanceName = "m-arkhipenka";
            });
           
            services.AddIdentity<User, Role>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PostgresDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<User>), typeof(GenericRepository<User>));
            services.AddScoped(typeof(IGenericRepository<Order>), typeof(GenericRepository<Order>));
            services.AddScoped(typeof(IGenericRepository<Product>), typeof(GenericRepository<Product>));
            services.AddScoped(typeof(IGenericRepository<UserAccess>), typeof(GenericRepository<UserAccess>));
            services.AddScoped<IUserAccessRepository, UserAccessRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuditLogHandler, AuditLogHandler>();
            services.AddScoped<IMetaDataHandler, MetaDataHandler>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthUserService, AuthUserService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IJwtProvider, JwtProvider>();


            var jwtkey = config["Jwt:Key"];
            if (jwtkey == null)
                throw new ArgumentNullException("Jwt:key is null");

            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(config["Firebase:ServiceAccount"])
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании FirebaseApp: {ex.Message}");
            }


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = config["Jwt:Issuer"],
                      ValidAudience = config["Jwt:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey))
                  };

              })
              .AddGoogle(options =>
              {
                  options.ClientId = config["Google:web:client_id"] ?? throw new NullReferenceException("Google:web:client_id is null");
                  options.ClientSecret = config["Google:web:client_secret"] ?? throw new NullReferenceException("Google:web:client_secret is null");
                  options.CallbackPath = "/signin-google";
              });
            services.AddAuthorization();
            services.AddAuthentication();
            return services;
      
        }
    }
}
