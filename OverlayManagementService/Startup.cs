using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.OpenApi.Models;
using OverlayManagementService.Factories;
using OverlayManagementService.Infrastructure;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using OverlayManagementService.Services;


namespace OverlayManagementService
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"))
                    .EnableTokenAcquisitionToCallDownstreamApi()
                    .AddInMemoryTokenCaches();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:3000", "https://localhost:44323/")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                policy => policy.Requirements.Add(new GroupPolicyRequirement(Configuration["Groups:Admin"])));
                options.AddPolicy("Member",
              policy => policy.Requirements.Add(new GroupPolicyRequirement(Configuration["Groups:Member"])));
            });
            services.AddSingleton<IAuthorizationHandler, GroupPolicyHandler>();

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OverlayManagementService", Version = "v1" });
            });



            services.AddScoped<IOverlayConnectionService, VMOverlayConnectionService>();
            services.AddSingleton<IOverlayManagementService, VMOverlayManagementService>();
            services.AddScoped<GraphServiceClient, GraphServiceClient>();
            services.AddSingleton<IRepository, JsonRepository>();
            services.AddScoped<IFirewall, Firewall>();
            services.AddSingleton<IAddress, IPAddress>();
            services.AddSingleton<IIdentifier, VNI>();
            services.AddSingleton<INetworkFactory, NetworkFactory>();
            services.AddSingleton<IBridgeFactory, BridgeFactory>();
            services.AddSingleton<IVirtualMachineFactory, VirtualMachineFactory>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OverlayManagementService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
