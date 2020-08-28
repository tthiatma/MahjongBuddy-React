using FluentValidation.AspNetCore;
using MahjongBuddy.API.Middleware;
using MahjongBuddy.Application.Games;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.EntityFramework.EntityFramework;
using MahjongBuddy.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using MahjongBuddy.Core;
using MahjongBuddy.API.SignalR;
using System.Threading.Tasks;
using System;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Infrastructure.Photos;
using MahjongBuddy.Infrastructure.Email;

namespace MahjongBuddy.API
{
    public class Startup
    {
        private readonly string hubName = "gameHub";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Create>());

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = false;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddDbContext<MahjongBuddyDbContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("WWW-Authenticate")
                    .WithOrigins("http://localhost:3000", "http://192.168.86.167:3000", "http://192.168.86.26")
                    .AllowCredentials();
                });
            });


            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddAutoMapper(typeof(Create.Handler));
            services.AddSignalR();
            var builder = services.AddIdentityCore<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            });

            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<MahjongBuddyDbContext>();
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();
            identityBuilder.AddDefaultTokenProviders();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsGameHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(3)
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if(!string.IsNullOrEmpty(accessToken) 
                                && (path.StartsWithSegments($"/{hubName}")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IFacebookAccessor, FacebookAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IPointsCalculator, HomeGameCalculator>();
            services.AddScoped<HandTypeBuilder, HandTypeBuilder>();
            services.AddScoped<ExtraPointBuilder, ExtraPointBuilder>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<FacebookAppSettings>(Configuration.GetSection("Authentication:Facebook"));
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
                await next();
            });

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opt => opt.NoReferrer());
            app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            app.UseXfo(opt => opt.Deny());
            app.UseCsp(opt => opt
                    .BlockAllMixedContent()
                    //.StyleSources(s => s.Self()
                    //    .CustomSources("https://fonts.googleapis.com", "sha256-F4GpCPyRepgP5znjMD8sc7PEjzet5Eef4r09dEGPpTs="))
                    .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "data:"))
                    .FormActions(s => s.Self())
                    .FrameAncestors(s => s.Self())
                    .ImageSources(s => s.Self().CustomSources("https://res.cloudinary.com", "blob:", "data:"))
                    //.ScriptSources(s => s.Self().CustomSources("sha256-ma5XxS1EBgt17N22Qq31rOxxRWRfzUTQS1KOtfYwuNo="))
                );

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>($"/{hubName}");
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
