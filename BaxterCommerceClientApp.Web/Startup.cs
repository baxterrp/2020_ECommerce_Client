﻿using BaxterCommerce.Client;
using BaxterCommerce.Client.Products;
using BaxterCommerce.Client.Users;
using BaxterCommerceClientApp.Web.Services;
using BaxterCommerceClientApp.Web.Services.Products;
using BaxterCommerceClientApp.Web.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaxterCommerceClientApp.Web
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
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IProductGroupService, ProductGroupService>();

            var config = new ClientConfiguration();
            Configuration.GetSection("ClientConfiguration").Bind(config);
            services.AddSingleton(config);

            services.AddSingleton<IAuthenticationClient, AuthenticationClient>();
            services.AddSingleton<IUserRegistrationClient, UserRegistrationClient>();
            services.AddSingleton<ICreateProductGroupClient, CreateProductGroupClient>();
            services.AddSingleton<IFindProductGroupClient, FindProductGroupClient>();

            services.AddCors(sp => sp.AddPolicy("StandardPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("StandardPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
