using Coffee.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Coffee
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
            services.AddControllers();
            // Cấu hình dịch vụ RazorRuntimeCompilation.
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            // Cấu hình kết nối cơ sở dữ liệu.
            var ConnectionStr = Configuration.GetConnectionString("dbStr");
            services.AddDbContext<website_cafeMVCContext>(option => option.UseSqlServer(ConnectionStr));

            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            // Cấu hình cơ chế xác thực Cookie. Có thể đặt tên co Cookie("tên ở đây nè"); tên mặc định sẽ là cookies
            services.AddAuthentication("CookieAuth").AddCookie
                ("CookieAuth",
                    options =>
                    {
                        options.Cookie.Name = "CookieAuth";
                        options.LoginPath = "/Login/SignIn";
                        options.AccessDeniedPath = "/Login/AccessDenied";
                    }
                );
            // ỦY quyền
            services.AddAuthorization(options =>
            {
                // Tạo quyền truy cập Administrator với yêu cầu 1 cặp name-value: Admin-Ad
                options.AddPolicy("Administrator",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("Admin");
                    });
                options.AddPolicy("Staff",
                  policy =>
                  {
                      policy.RequireAuthenticatedUser();
                      policy.RequireClaim("Staff");
                  });
                options.AddPolicy("Customer",
                  policy =>
                  {
                      policy.RequireAuthenticatedUser();
                      policy.RequireClaim("Staff");
                  });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();
                // Xác thực
                app.UseAuthentication();
                // Ủy quyền
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }
        }
    }
