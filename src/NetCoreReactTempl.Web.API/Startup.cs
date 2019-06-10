using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreReactTempl.DAL;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using NetCoreReactTempl.Web.API.Filters;
using FluentValidation;

namespace NetCoreReactTempl.Web.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));

            services.AddMvc(o => {
                o.Filters.Add(new ApiExceptionFilterAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options => {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddMemoryCache();

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

            services.AddScoped<IDataManager<User>, DataManager<User>>();
            services.AddScoped<IDataManager<Data>, DataManager<Data>>();
            services.AddScoped<IDataManager<Field>, DataManager<Field>>();
            services.AddSingleton<ConfigurationStore>();
                       

            services.AddTransient<IValidator<Handlers.Auth.Query.GetList>,Handlers.Auth.Query.GetListValidator>();
            services.AddTransient<IValidator<Handlers.Auth.Query.Get>,Handlers.Auth.Query.GetValidator>();
            services.AddTransient<IValidator<Handlers.Auth.Command.Registration>, Handlers.Auth.Command.RegistrationValidator>();
            services.AddTransient<IValidator<Handlers.Auth.Command.Authorization>, Handlers.Auth.Command.AuthorizationValidator>();
            services.AddTransient<IValidator<Handlers.Auth.Command.Delete>, Handlers.Auth.Command.DeleteValidator>();
            services.AddTransient<IValidator<Handlers.Data.Query.GetList>, Handlers.Data.Query.GetListValidator>();
            services.AddTransient<IValidator<Handlers.Data.Query.Get>, Handlers.Data.Query.GetValidator>();
            services.AddTransient<IValidator<Handlers.Data.Command.Create>, Handlers.Data.Command.CreateValidator>();
            services.AddTransient<IValidator<Handlers.Data.Command.Update>, Handlers.Data.Command.UpdateValidator>();
            services.AddTransient<IValidator<Handlers.Data.Command.Delete>, Handlers.Data.Command.DeleteValidator>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<ResourseStore>();
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

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
