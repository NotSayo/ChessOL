using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ChessServer.Endpoints;
using ChessServer.Hubs;
using IdentityLibrary.Identity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Chess.GameManager;
using Shared.Chess.Pieces;
using Shared.Identity.Entities;
using Shared.Types;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./Keys/JwtKey.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("./Databases/ConnectionStrings.json", optional: true, reloadOnChange: true);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);

builder.Services.AddDbContext<ChessContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionString:Sqlite"], sqliteOptions =>
    {
        sqliteOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});

builder.Services.AddIdentity<SystemUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = false;
    options.Password.RequiredUniqueChars = 0;
    options.Lockout.AllowedForNewUsers = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;
    options.Lockout.MaxFailedAccessAttempts = int.MaxValue; // TODO change later
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ChessContext>()
.AddDefaultTokenProviders();;

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = false,
        ValidateActor = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddCors();

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 102400; // 100 KB
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityEndpoints(key);

app.MapHub<LobbyHub>("/lobby");
app.MapHub<GameHub>("/game");
app.MapHub<QueueHub>("/queue");

app.Run();


