﻿using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShareCar.Api.Middleware;
using ShareCar.Db;
using ShareCar.Db.Entities;
using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Facebook;
using ShareCar.Logic.DI;
using System;
using System.Text;

namespace ShareCar.Api
{
    public class Startup
    {
        private readonly SymmetricSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtSecretKey")));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                optionsLifetime: ServiceLifetime.Transient
            );

            services.AddCors(options =>
                options.AddPolicy("AllowSubdomain",
                    builder =>
                    {
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    }
                )
            );

               ConfigureAuthentication(services);




            services.AddMvc()
            .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper();

            var applicationContainer = Bootstrapper.AddRegistrationsToDIContainer(services);

            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Automatic migrations 
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            }

            app.UseMiddleware<JwtInHeaderMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors(
                options => options.WithOrigins("https://ctsbaltic.com")
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader()
            );
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
         //   var jwtAppSettingOptions = ConfigureSettings(services);

            AddAuthentication(services/*, jwtAppSettingOptions*/);

            AddIdentity(services);
        }

        private IConfigurationSection ConfigureSettings(IServiceCollection services)
        {
            // Register the ConfigurationBuilder instance of FacebookAuthSettings
            services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));

            services.Configure<SendGridSettings>(Configuration.GetSection(nameof(SendGridSettings)));

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.RoleClaimName = jwtAppSettingOptions[nameof(JwtIssuerOptions.RoleClaimName)];
                options.RoleClaimValue = jwtAppSettingOptions[nameof(JwtIssuerOptions.RoleClaimValue)];
                options.IdClaimName = jwtAppSettingOptions[nameof(JwtIssuerOptions.IdClaimName)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = jwtAppSettingOptions[nameof(JwtIssuerOptions.IdClaimName)]
            );

            return jwtAppSettingOptions;
        }

        private static void AddIdentity(IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }

        private void AddAuthentication(IServiceCollection services/*, IConfigurationSection jwtAppSettingOptions*/)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
.AddCookie(options =>
{
options.Cookie.Name = "CustomAuth";
options.Cookie.Domain = ".ctsbaltic.com";
});
            /*  var tokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                  ValidateAudience = true,
                  ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = _signingKey,

                  RequireExpirationTime = false,
                  ValidateLifetime = false,
                  ClockSkew = TimeSpan.Zero
              };

              services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(configureOptions =>
              {
                  configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                  configureOptions.TokenValidationParameters = tokenValidationParameters;
                  configureOptions.SaveToken = true;
              });

              // api user claim policy
              services.AddAuthorization(options =>
              {
                  options.AddPolicy("ApiUser",
                      policy => policy.RequireClaim(jwtAppSettingOptions[nameof(JwtIssuerOptions.RoleClaimName)],
                          jwtAppSettingOptions[nameof(JwtIssuerOptions.RoleClaimValue)]));
              });*/
        }
    }
}
