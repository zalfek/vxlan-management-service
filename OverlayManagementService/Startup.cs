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
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using OverlayManagementService.Resolvers;
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
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            //        .EnableTokenAcquisitionToCallDownstreamApi(new string[] { "user.read" })
            //        .AddInMemoryTokenCaches();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("http://localhost:3000", "https://localhost:44323/")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
            .AddMicrosoftIdentityUI();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OverlayManagementService", Version = "v1" });
            });



            services.AddScoped<IOverlayConnectionService, VMOverlayConnectionService>();
            services.AddScoped<IOverlayManagementService, VMOverlayManagementService>();
            services.AddScoped<IMembershipResolver, MembershipResolver>();
            services.AddScoped<GraphServiceClient, GraphServiceClient>();
            services.AddSingleton<IRepository, JsonRepository>();
            services.AddScoped<IFirewall, Firewall>();
            //services.AddScoped<IBridge, Bridge>();
            services.AddSingleton<IAddress, IPAddress>();
            //services.AddScoped<IOpenVirtualSwitch, OpenVirtualSwitch>();
            services.AddSingleton<IIdentifier, VNI>();
            //services.AddScoped<IOverlayNetwork, VXLANOverlayNetwork>();
            //services.AddScoped<IVeth, Veth>();
            //services.AddScoped<IVirtualMachine, VirtualMachine>();
            //services.AddScoped<IVXLANInterface, VXLANInterface>();
            //services.AddScoped<IOverlayNetwork, VXLANOverlayNetwork>();
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
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
