using authAPI.Controllers;
using authAPI.Data;
using authAPI.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authAPI
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

            services.AddDbContext<SocialContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MyConnection")));

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<SocialContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true; // say�sal bir de�er olsun mu �ifrede 
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;  // kullan�c� 5 kere yanl�� giri� yaparsa hesab�na 5 dk eri�emeyecek yani kitlenecek.
                options.Lockout.AllowedForNewUsers = true;

                // �ifrede olmas� gereken karakterlerin ne olaca��n� yazd�k.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-. _ @ +";
                options.User.RequireUniqueEmail = true; // her olu�turulan mail adresi birbirinden farkl� olacak.

            });

            services.AddCors();

            #region Add Authentication

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {   // Token bilgisini burada validate edece�iz.

                x.RequireHttpsMetadata = false; // bu token bilgisi sadece https protokol�n� kullanan isteklerden mi gelsin?
                x.SaveToken = true; //token bilgisi server taraf�nda kaydedilsin mi?
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Bize gelen token bilgisinin 3.k�s�m�n� yani imza bilgisinin kontrol�n� yapal�m m� yapmayal�m m�?
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Secret").Value)),
                    ValidateIssuer = false, // token bilgisini olu�turan ki�iden bahsediliyor.
                    ValidateAudience = false
                };
            });
            #endregion


            services.AddControllers();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "authAPI", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "authAPI v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<SocialContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
