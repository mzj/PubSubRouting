using Dapr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                
                .AddControllers()
                //.ConfigureApiBehaviorOptions(options =>
                //{
                //    options.SuppressConsumesConstraintForFormFileParameters = true;
                //    options.SuppressInferBindingSourcesForParameters = true;
                //    options.SuppressModelStateInvalidFilter = true;
                //    options.SuppressMapClientErrors = true;
                //    options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
                //        "https://httpstatuses.com/404";
                //    options.ClientErrorMapping[StatusCodes.Status415UnsupportedMediaType].Link =
                //        "https://httpstatuses.com/415/marko";
                //})
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddDapr(b =>
                {

                    b.UseJsonSerializationOptions(new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        PropertyNameCaseInsensitive = true,
                        //PropertyNamingPolicy = null
                    });
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Subscriber", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscriber v1"));
            }

            app.Use(async (context, next) =>
            {
                logger.LogInformation($"Header: {JsonConvert.SerializeObject(context.Request.Headers, Formatting.Indented)}");

                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                logger.LogInformation($"Body: {body}");
                context.Request.Body.Position = 0;

                logger.LogInformation($"Host: {context.Request.Host.Host}");
                logger.LogInformation($"Client IP: {context.Connection.RemoteIpAddress}");
                await next.Invoke();
            });

            app.UseRouting();

            app.UseCloudEvents();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();
            });
        }
    }
}
