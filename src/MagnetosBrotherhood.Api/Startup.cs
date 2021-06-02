using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MagnetosBrotherhood.DAL.Interfaces;
using MagnetosBrotherhood.DAL.Repositories;
using MagnetosBrotherhood.Domain.Repositories;
using MagnetosBrotherhood.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MagnetosBrotherhood.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(config => {
                config.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer();

            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(options => {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(item.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = $"v{item.ApiVersion.MajorVersion}",
                        Title = $"Brotherhood of Mutans",
                        Description = $"Magneto wants you in his team, but only if you are at least a second class mutant.",
                        TermsOfService = new System.Uri("http://magnetowantsyou.org/terms")
                    });
                }
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(e => e.FullName);
                options.OrderActionsBy(e => e.HttpMethod);
                options.AddSecurityDefinition("x-api-key", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "x-api-key",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "x-api-key" }
                        },
                        new string[] { }

                    }
                });
            });

            // Register services.
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IMutantService, MutantService>();
            services.AddSingleton<IMutantRepository, MutantRepository>();
            services.AddSingleton<IMutantDynamoDb, MutantDynamoDbRepository>();
            services.AddSingleton<IDynamoDBContext>( provider =>
            {
                IAmazonDynamoDB client = new AmazonDynamoDBClient();
                return new DynamoDBContext(client);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseRouting();
            app.UseApiVersioning();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                foreach (var item in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{item.GroupName}/swagger.json", $"Magneto wants you in his team v{item.ApiVersion.MajorVersion}");
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Magneto wants you in his team, but only if you are at least a second class mutant.");
                });
            });
        }
    }
}
