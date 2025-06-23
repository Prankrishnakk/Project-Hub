
using Application.Interface.AdminInterface;
using Application.Interface.AuthInterface;
using Application.Interface.HodInterface;
using Application.Interface.NotificationInterface;
using Application.Interface.StudentInterface;
using Application.Interface.TutorInterface;
using Application.Mapper;
using Application.Services.AdminService;
using Application.Services.AuthServices;
using Application.Services.HodService;
using Application.Services.SignalR;
using Application.Services.StudentServices;
using Application.Services.TutorService;
using Infrastructure.Context;
using Infrastructure.Repositories.AdminRepository;
using Infrastructure.Repositories.AuthRepository;
using Infrastructure.Repositories.HodRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.TutorRepository;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Project_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add Services & Repositories
            builder.Services.AddScoped<IStudentAuthRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentAuthService, StudentAuthService>();
            builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();
            builder.Services.AddScoped<IProjectGroupService, ProjectGroupService>();
            builder.Services.AddScoped<IGetDepartmentGroupsRepository, GetDepartmentGroupsRepository>();
            builder.Services.AddScoped<IGetDepartmentGroupsService, GetDepartmentGroupsService>();
            builder.Services.AddScoped<IStudentProjectRepository,StudentProjectRepository>();
            builder.Services.AddScoped<IStudentProjectService, StudentProjectService>();
            builder.Services.AddScoped<ITutorReviewRepository, TutorReviewRepository>();
            builder.Services.AddScoped<ITutorReviewService, TutorReviewService>();
            builder.Services.AddScoped<IGetProjectDetailsRepository, GetProjectDetalisRepository>();
            builder.Services.AddScoped<IGetProjectDetailsService, GetProjectDetailsService>();
            builder.Services.AddScoped<IStudentFeedbackRepository, StudentFeedbackRepository>();
            builder.Services.AddScoped<IStudentFeedbackService, StudentFeedbackService>();
            builder.Services.AddScoped<ITutorGroupRepository,TutorGroupRepository>();
            builder.Services.AddScoped<ITutorGroupService, TutorGroupService>();
            builder.Services.AddScoped<IProjectSubmissionRepository, ProjectSubmissionRepository>();
            builder.Services.AddScoped<IProjectSubmissionService, ProjectSubmissionService>();
            builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IProjectAddService, ProjectAddService>();
            builder.Services.AddScoped<IProjectAddRepository, ProjectAddRepository>(); 






            //✅ Add SignalR
            builder.Services.AddSignalR();

            // ✅ Add CORS (for frontend access to SignalR)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .SetIsOriginAllowed(_ => true); 
                });
            });









            // Swagger Configuration

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });





            // JWT Authentication Configuration 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                o.RequireHttpsMetadata = false;  // Use true in production for security
                o.SaveToken = true;
            });



            builder.Services.AddDbContext<AppDbContext>(Options =>
        Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ✅ Use CORS
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<NotificationHub>("/notificationhub"); //SignalR

            app.Run();
        }
    }
}
