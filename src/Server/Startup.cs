using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Server.Commons;
    using Server.Commons.Behaviours;
    using Server.Entities;
    using Server.Services;
    using Server.SignalR.Hubs;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog());
            services.AddHealthChecks();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentRequestService, CurrentRequestService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddSignalR();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("SignalRPoc"));

            var jwtConfiguration = this.Configuration.GetSection("Jwt").Get<JwtConfiguration>();

            services.AddAuthentication(c =>
            {
                c.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecurityKey)),
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (string.IsNullOrEmpty(accessToken) && context.Request.Headers.TryGetValue("Authorization", out var value))
                        {
                            accessToken = value.ToString().Replace("Bearer ", string.Empty);
                        }

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.ConfigureSwaggerGen(c =>
            {
                c.CustomSchemaIds(s => s.FullName);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SignalRPoc Server",
                    Version = "v1",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = new PathString("/api");
                await next();
            });

            SeedUsers(app, loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "SignalRPoc Server v1"));
            }

            app.UseHealthChecks("/health");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<UserHub>("/userHub");
            });
        }

        private static void SeedUsers(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("SeedUsers");

            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetService<ApiContext>();
                context!.Add(new User { Id = Guid.NewGuid(), Password = "P@ssword!", UserName = "User1" });
                context!.Add(new User { Id = Guid.NewGuid(), Password = "P@ssword!", UserName = "User2" });
                context!.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, exception.Message);
            }
        }
    }
}
