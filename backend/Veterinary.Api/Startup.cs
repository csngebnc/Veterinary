using Autofac;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using IdentityModel;
using IdentityServer4.Configuration;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Veterinary.Api.Services;
using Veterinary.Api.Validation.ProblemDetails.Data;
using Veterinary.Api.Validation.ProblemDetails.Exceptions;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities;

namespace Veterinary.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            services.AddDbContext<VeterinaryDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<VeterinaryUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole<Guid>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<VeterinaryDbContext>();

            services.AddIdentityServer(options =>
            {
                options.UserInteraction = new UserInteractionOptions()
                {
                    LogoutUrl = "/Account/Logout",
                    LoginUrl = "/Account/Login",

                    LoginReturnUrlParameter = "returnUrl"
                };
                options.Authentication.CookieAuthenticationScheme = IdentityConstants.ApplicationScheme;
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Configuration.GetSection("IdentityServer:IdentityResources"))
                .AddInMemoryApiResources(Configuration.GetSection("IdentityServer:ApiResources"))
                .AddInMemoryApiScopes(Configuration.GetSection("IdentityServer:ApiScopes"))
                .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
                .AddAspNetIdentity<VeterinaryUser>()
                .AddProfileService<ProfileService>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:Audience");
                    options.RequireHttpsMetadata = false;
                }
            );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("api-openid", policy => policy.RequireAuthenticatedUser()
                    .RequireClaim("scope", "api-openid")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.AddPolicy("Manager", policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(JwtClaimTypes.Role, "ManagerDoctor")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.AddPolicy("Doctor", policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(JwtClaimTypes.Role, "ManagerDoctor", "NormalDoctor")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.AddPolicy("User", policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(JwtClaimTypes.Role, "ManagerDoctor", "NormalDoctor", "User")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.DefaultPolicy = options.GetPolicy("api-openid");
            });

            services.AddOpenApiDocument(config =>
            {
                config.Title = "Veterinary API";
                config.Description = "Veterinary appointment booking and medical record api";
                config.DocumentName = "Veterinary";

                config.AddSecurity("OAuth2", new OpenApiSecurityScheme
                {
                    OpenIdConnectUrl =
                        $"{Configuration.GetValue<string>("Authentication:Authority")}/.well-known/openid-configuration",
                    Scheme = "Bearer",
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                $"{Configuration.GetValue<string>("Authentication:Authority")}/connect/authorize",
                            TokenUrl = $"{Configuration.GetValue<string>("Authentication:Authority")}/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenId" },
                                { "api-openid", "all" }
                            }
                        }
                    }
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));
            });

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddMediatR(Assembly.Load("Veterinary.Application"));
            services.AddFluentValidation(new[] { Assembly.Load("Veterinary.Application") });

            services.AddProblemDetails(ConfigureProblemDetails);
            services.AddRazorPages();
            services.AddControllers();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Veterinary.Dal"))
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                config.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "veterinary-swagger",
                    ClientSecret = null,
                    UsePkceWithAuthorizationCodeGrant = true,
                    ScopeSeparator = " ",
                    Realm = null,
                    AppName = "Veterinary Swagger Client"
                };
            });

            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;

            options.Map<ForbiddenException>(
              (ctx, ex) =>
              {
                  var pd = StatusCodeProblemDetails.Create(StatusCodes.Status403Forbidden);
                  pd.Title = "Forbidden (403)";
                  return pd;
              }
            );

            options.Map<MethodNotAllowedException>(
              (ctx, ex) =>
              {
                  var pd = StatusCodeProblemDetails.Create(StatusCodes.Status405MethodNotAllowed);
                  pd.Title = ex.Message;
                  return pd;
              }
            );

            options.Map<EntityNotFoundException>(
              (ctx, ex) =>
              {
                  var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                  pd.Title = "A megadott azonosítóval nem található rögzített elem.";
                  return pd;
              }
            );

            options.Map<ValidationException>(
              (ctx, ex) =>
              {
                  var pd = new InputValidationErrors(ex.Errors);
                  pd.Title = "Sikertelen mentés, mivel validációs hibák vannak.";
                  pd.Status = 400;
                  return pd;
              }
            );

        }
    }
}
