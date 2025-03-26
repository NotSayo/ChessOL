using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
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


GameInstance instance = new GameInstance();


IPiece r = new Knight()
{
    GameInstance = instance,
    PieceColor = EPieceColor.White,
    Position = new Vector(3,4),
};
IPiece rook = new Pawn() //TODO adjust pawn movement and taking sideways
{
    GameInstance = instance,
    PieceColor = EPieceColor.Black,
    Position = new Vector(4, 4)
};

IPiece otherRook = new Rook()
{
    GameInstance = instance, PieceColor = EPieceColor.White,
    Position = new Vector(4, 6)
};
var checkArray = new string[8, 8];
instance.Board[r.Position.Y, r.Position.X] = r;
instance.Board[rook.Position.Y, rook.Position.X] = rook;
instance.Board[otherRook.Position.Y, otherRook.Position.X] = otherRook;
rook.CheckAvailableMoves();
Console.WriteLine($"Starting Position: ({rook.Position.Y}, {rook.Position.X})");
rook.AvailableMoves.ForEach(s => checkArray[s.Y, s.X] = "X");
checkArray[rook.Position.Y, rook.Position.X] = "R";


for (int i = 0; i < 8; i++)
{
    for (int j = 0; j < 8; j++)
    {
        if (checkArray[i, j] is not null)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(checkArray[i, j]);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("I");
        }


    }
    Console.WriteLine();
}










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

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/token", () =>
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescription = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(),
        Expires = DateTime.UtcNow.AddDays(2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var newToken = tokenHandler.CreateToken(tokenDescription);
    var tokenString = tokenHandler.WriteToken(newToken);
    return Results.Ok("Token: " + tokenString);
});

app.MapPost("/login", async (UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager, [FromBody] LoginModel model) =>
{
    var user = await userManager.FindByEmailAsync(model.Email);
    if (user is not null)
        return Results.Ok("Logged in");

    user = new SystemUser
    {
        Email = model.Email,
        UserName = model.Username,
        DisplayName = model.Username
    };

    var identityResult = await userManager.CreateAsync(user, model.Password);

    if (!identityResult.Succeeded)
        return Results.InternalServerError(identityResult.Errors);

    user = await userManager.FindByEmailAsync(model.Email);

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email!)
    };


    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescription = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var newToken = tokenHandler.CreateToken(tokenDescription);
    var tokenString = tokenHandler.WriteToken(newToken);
    return Results.Ok("Token: " + tokenString);
});

    app.MapGet("/secret", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]() =>
    {
        return Results.Ok("Secret");
    });

app.Run();


public class LoginModel()
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}