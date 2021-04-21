using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Service.Base.Auth;
using Service.Base.Dispatchers;
using Service.Base.Redis;
using Service.Base.Swagger;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.Repositories;
using Service.Identity.Services;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Service.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(environment.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
                                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(Configuration)
                                .CreateLogger();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson().AddMvcOptions(o => o.AllowEmptyInputInBodyModelBinding = true);

            // DbContext
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Database"));
                options.UseSnakeCaseNamingConvention();
            });

            // add cache memory
            // services.AddDistributedMemoryCache();

            // add redis
            services.AddRedis();

            // Add context accessor
            services.AddHttpContextAccessor();

            // Add Jwt
            services.AddJwt();

            //cors settings
            services.AddCors(o => o.AddPolicy("IdentityServicePolicy", policy =>
            {
                policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));

            //DB Access Repo
            //ConfigureRepositories(services);

            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add password hasher Service
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Swagger
            services.AddSwagger();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    // options.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), XmlCommentsFileName));

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme{
                               Reference = new OpenApiReference
                               {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                               }
                            },
                            new string[] { }
                        }
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();

                //Swagger Middleware
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            //app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("IdentityServicePolicy");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces().InstancePerLifetimeScope();

            // builder
            builder.AddDispatchers();
        }

        //public void ConfigureRepositories(IServiceCollection services)
        //{
        //    services.AddScoped<IUserRepository, UserRepository>();
        //    services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        //    services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        //    services.AddScoped<IRoleRepository, RoleRepository>();
        //    services.AddScoped<IPermissionRepository, PermissionRepository>();
        //}
    }
}
