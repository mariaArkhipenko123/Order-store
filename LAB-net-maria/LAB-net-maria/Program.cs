using AutoMapper;
using Lab.Application.DependencyInjection;
using Lab.Application.GraphQL;
using Lab.Infrastructure.DependencyInjection;
using HotChocolate.AspNetCore;

namespace LAB_net_maria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Configuration.AddJsonFile("secret.json", optional: true, reloadOnChange: true);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); // Должен быть перед аутентификацией и авторизацией
            app.UseAuthentication(); // Теперь в правильном месте
            app.UseAuthorization(); // Теперь в правильном месте


            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            });

            //app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL("/graphql");
            });

            app.Map("/", () => "This is the root of m-arkhipenka-API from docker compose!");
            app.Run();
        }
    }
}