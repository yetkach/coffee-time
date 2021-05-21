using CoffeeTime.Data.EF;
using CoffeeTime.Data.Interfaces;
using CoffeeTime.Data.Repositories;
using CoffeeTime.Logics.Interfaces;
using CoffeeTime.Logics.Services;
using CoffeeTime.Web.Models;
using CoffeeTime.Web.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CoffeeTime.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CoffeeDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            services.AddScoped<ICoffeeRepository, CoffeeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICoffeeService, CoffeeService>();
            services.AddScoped<IOrderGuidService, OrderGuidService>();

            services.AddAutoMapper(typeof(MapperConfigure));
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpContextAccessor();
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(15));
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<OrderViewModel>, OrderViewModelValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
