using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Serilog settings

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();
//Only use serilog
builder.Services.AddSerilog();
//Use serilog along with built in loggers
//builder.Logging.AddSerilog();

#endregion Serilog settings

// Add services to the container.
builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
//CORS Named policy
//builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
//{
//    //Allowing only few origins
//    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
//    //Allowing all origins
//    //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//}));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com", "http://gmail.com").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        policy.WithOrigins("http://outlook.com", "http://microsoft.com").AllowAnyHeader().AllowAnyMethod();
    });
});
var keyJWTSecretLocal = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretLocal"));
var keyJWTSecretMicrosoft = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretMicrosoft"));
var keyJWTSecretGoogle = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretGoogle"));
//JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGoogleUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretGoogle),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
    };
}).AddJwtBearer("LoginForMicrosoftUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretMicrosoft),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
    };
}).AddJwtBearer("LoginForLocalUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretLocal),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response"))
        .RequireCors("AllowOnlyLocalhost");

    endpoints.MapControllers()
             .RequireCors("AllowAll");

    endpoints.MapGet("api/testingendpoint2",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));
});

app.Run();