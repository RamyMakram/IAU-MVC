using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MustafidAppModels.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MustafidApp.JWT;
using Microsoft.AspNetCore.Authorization;
using MustafidApp.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using AutoMapper;
using MustafidApp.Mapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MustafidApp.Controllers.v1;

namespace MustafidApp
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
			#region Api Versioning
			// Add API Versioning to the Project
			services.AddApiVersioning(config =>
			{
				// Specify the default API Version as 1.0
				config.DefaultApiVersion = new ApiVersion(1, 0);
				// If the client hasn't specified the API version in the request, use the default API version number 
				config.AssumeDefaultVersionWhenUnspecified = true;
				// Advertise the API versions supported for the particular endpoint
				config.ReportApiVersions = true;
			});
			#endregion
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
				o.SaveToken = true;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["JWT:Issuer"],
					ValidAudience = Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Key)
				};
			});
			//services.AddAuthorization(options =>
			//{
			//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
			//        .RequireAuthenticatedUser()
			//        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build();
			//});

			services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

			services.AddDbContext<MustafidAppContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("default"));
			});

			services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.DictionaryKeyPolicy = null;
				options.JsonSerializerOptions.PropertyNamingPolicy = null;
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1.5", new OpenApiInfo { Title = "MustafidApp", Version = "v1.5", Description = "966xxxxxxxxx|9999|thJlxqVh6QR273i+PSzqdoMGr6VaLlEMWRAI+Nw+b4qVIGh+X9OAXi2mfPXFiqFgP5YOAnsNUwf9f5i2c90XLw==" });
				// using System.Reflection;

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please insert your JWT Token into field",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement{
					{
						new OpenApiSecurityScheme{
							Reference = new OpenApiReference{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[]{}
					}
				});
				c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			//services.AddAutoMapper(typeof(Startup));
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new AutoMapperProfile(Configuration));
			});
			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, MustafidAppContext db)
		{
			if (env.IsDevelopment())
			{
			}

			#region Validate Logout Tokens
			app.Use(async (context, next) =>
				{
					var token = context.Request.Headers["Authorization"].ToString();
					if (token != null && !string.IsNullOrEmpty(token) && token.Split("Bearer ").Count() == 2)
					{
						var tokenHandler = new JwtSecurityTokenHandler();
						var SecretKey = Configuration.GetValue<string>("JWT:Key");
						var key = Encoding.ASCII.GetBytes(SecretKey);


						tokenHandler.ValidateToken(token.Split("Bearer ")[1], new TokenValidationParameters
						{
							ValidateIssuer = false,
							ValidateAudience = false,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = Configuration["JWT:Issuer"],
							ValidAudience = Configuration["JWT:Audience"],
							IssuerSigningKey = new SymmetricSecurityKey(key)
						}, out SecurityToken validatedToken);

						var jwtToken = (JwtSecurityToken)validatedToken;

						var refToken = jwtToken.Claims.First(x => x.Type == "RefToken").Value;

						if (refToken != null)
						{
							try
							{
								var options = new DbContextOptionsBuilder<MustafidAppContext>().UseSqlServer(Configuration.GetConnectionString("default")).Options;
								// With the options generated above, we can then just construct a new DbContext class

								using (var _appContext = new MustafidAppContext(options))
								{
									// Your code here
									var ifExpired = /*await new AuthController().IfExpired(refToken)*/await _appContext.UserTokens.AnyAsync(q => q.RefToken == refToken && q.Expired);
									if (ifExpired)
									{
										await context.Response.WriteAsJsonAsync(new ResponseClass { Success = false, data = "NoAuthExp" });
										return;
									}
								}
							}
							catch (Exception ee)
							{

								throw;
							}
						}
					}

					await next.Invoke();
				});
			#endregion

			app.UseStatusCodePages(async statusCodeContext =>
			{
				switch (statusCodeContext.HttpContext.Response.StatusCode)
				{
					case 401:
						statusCodeContext.HttpContext.Response.StatusCode = 200;
						await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new ResponseClass { Success = false, data = "NoAuth" });
						break;
						//case 403:
						//    statusCodeContext.HttpContext.Response.StatusCode = 400;
						//    await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new ErrorMessage { httpStatus = 500, Message = "some message" });
						//    break;
				}
			});
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "MustafidApp v1.5"));

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.Map("/", async (req) =>
				{
					req.Response.Redirect("/swagger");
					return;
				});
			});

			//app.Use(async (context, next) =>
			//{
			//    await next();

			//    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
			//    {
			//        context.Response.ContentType = "application/json";
			//        context.Response.StatusCode = 200;


			//        await context.Response.WriteAsJsonAsync(new ResponseClass { Success = false, data = "NoAuth" });
			//    }

			//    //if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
			//    //{
			//    //    context.Response.ContentType = "application/json";

			//    //    await context.Response.WriteAsync(new
			//    //    {
			//    //        Message = "Your claims are incorrect."
			//    //    }.ToString());
			//    //}
			//});
		}
	}
}
