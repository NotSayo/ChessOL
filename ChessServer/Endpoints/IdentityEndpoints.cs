using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Identity.Entities;
using Shared.Model;

namespace ChessServer.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder endpoints, byte[] key)
    {
        endpoints.MapPost("/login", async (UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager, [FromBody] LoginModel model) =>
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user is null)
                return Results.NotFound("User not found");
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Results.Json(new { error = "Invalid Password" }, statusCode: 401);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email!)
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
            return Results.Ok(tokenString);
        });

        endpoints.MapPost("/register", async (UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager, [FromBody] RegistrerModel model) =>
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user is not null)
                return Results.Conflict("User already exists");

            user = new SystemUser
            {
                Email = model.Username,
                UserName = model.Username,
                DisplayName = model.Username
            };

            var identityResult = await userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
                return Results.InternalServerError(identityResult.Errors);

            user = await userManager.FindByNameAsync(model.Username);

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

        endpoints.MapGet("/token", () =>
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
            return Results.Ok(tokenString);
        });


        endpoints.MapGet("/secret", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]() =>
        {
            return Results.Ok("Secret");
        });
    }
}