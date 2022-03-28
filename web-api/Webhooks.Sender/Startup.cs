using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Webhooks.Sender.DbContexts;

namespace Webhooks.Sender
{
    public class Startup
    {
        private readonly string _allowAnyOrigin = "_allowAnyOrigin";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("WebhookSender")
                    .AddTransientHttpErrorPolicy(
                        p => p.WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10)
                        }));

            services.AddCors(options =>
                    {
                        options.AddPolicy(name: _allowAnyOrigin,
                                          builder =>
                                          {
                                              builder.AllowAnyOrigin()
                                                     .AllowAnyHeader()
                                                     .AllowAnyMethod();
                                          });
                    });

            services.AddControllers();

            services.AddDbContext<WebhooksContext>(opt => opt.UseInMemoryDatabase("Webhooks"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webhooks.Sender", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webhooks.Sender v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_allowAnyOrigin);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
