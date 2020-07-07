using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NetCoreReactTempl.App;
using NetCoreReactTempl.DAL;
using NetCoreReactTempl.DAL.Managers;
using NetCoreReactTempl.Domain.Configuration;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using NetCoreReactTempl.Web.API.Filters;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace NetCoreReactTempl.Web.API.Host
{
    public class Startup
    {
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));

            services.AddControllers(o =>
                {
                    o.Filters.Add(new ApiExceptionFilterAttribute());
                })                
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddAutoMapper(typeof(DataContext).Assembly, typeof(ConfigurationStore).Assembly);

            services.Configure<Dictionary<string, string>>(Configuration.GetSection("DictionarySetting"));

            var secret = Configuration.GetSection("DictionarySetting")["AuthSecret"];
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IDataManager<User>, DataManager<DAL.Entities.User, User>>();
            services.AddScoped<IDataManager<Data>, DataManager<DAL.Entities.Data, Data>>();
            services.AddScoped<IDataManager<Field>, DataManager<DAL.Entities.Field, Field>>();
            services.AddSingleton<IConfigurationStore, ConfigurationStore>();


            services.AddTransient<IValidator<App.Handlers.Auth.Query.GetList>, App.Handlers.Auth.Query.GetListValidator>();
            services.AddTransient<IValidator<App.Handlers.Auth.Query.Get>, App.Handlers.Auth.Query.GetValidator>();
            services.AddTransient<IValidator<App.Handlers.Auth.Command.Registration>, App.Handlers.Auth.Command.RegistrationValidator>();
            services.AddTransient<IValidator<App.Handlers.Auth.Command.Authorization>, App.Handlers.Auth.Command.AuthorizationValidator>();
            services.AddTransient<IValidator<App.Handlers.Auth.Command.Delete>, App.Handlers.Auth.Command.DeleteValidator>();
            services.AddTransient<IValidator<App.Handlers.Data.Query.GetList>, App.Handlers.Data.Query.GetListValidator>();
            services.AddTransient<IValidator<App.Handlers.Data.Query.Get>, App.Handlers.Data.Query.GetValidator>();
            services.AddTransient<IValidator<App.Handlers.Data.Command.Create>, App.Handlers.Data.Command.CreateValidator>();
            services.AddTransient<IValidator<App.Handlers.Data.Command.Update>, App.Handlers.Data.Command.UpdateValidator>();
            services.AddTransient<IValidator<App.Handlers.Data.Command.Delete>, App.Handlers.Data.Command.DeleteValidator>();

            services.AddMediatR(Assembly.Load("NetCoreReactTempl.App"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<ResourseStore>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseEndpoints(a =>
            {
                a.MapControllers();
            });
        }
    }
}
