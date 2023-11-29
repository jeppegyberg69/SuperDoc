using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.API.Authorization.Factories;
using SuperDoc.Customer.API.Swagger;
using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Documents;
using SuperDoc.Customer.Repositories.Users;
using SuperDoc.Customer.Services.Cases;
using SuperDoc.Customer.Services.Cases.Factories;
using SuperDoc.Customer.Services.Documents;
using SuperDoc.Customer.Services.Documents.Factories;
using SuperDoc.Customer.Services.Security;
using SuperDoc.Customer.Services.Users;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
#nullable disable
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
#nullable enable
    };
});

// Configure cross-origin resource sharing (CORS)
builder.Services.AddCors(options =>
{
    // Test environment
    options.AddPolicy(name: "testEnv",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                      });
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Add services to the container
builder.Services.AddDbContext<SuperDocContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILoginDtoFactory, LoginDtoFactory>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICaseFactory, CaseFactory>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IRevisionFactory, RevisionFactory>();


builder.Services.AddScoped<IAccessService, AccessService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("testEnv");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
