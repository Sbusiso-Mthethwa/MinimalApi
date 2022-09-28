using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Controllers;
using MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DeveloperRepository>();
/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();*/

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

var app = builder.Build();
/*app.UseSwagger();
app.UseSwaggerUI();*/



app.MapGet("/", () => "Hello world, this Sbu!").AllowAnonymous();

app.MapGet("/developers", ([FromServices] DeveloperRepository devs) => 
{ 
    return devs.GetAll();
});

app.MapGet("/developers/{id}", ([FromServices] DeveloperRepository devs, Guid id) =>
{
    var developer = devs.GetDeveloper(id);
    return developer is not null ? Results.Ok(developer) : Results.NotFound();
});

app.MapPost("/developers", ([FromServices] DeveloperRepository devs, Developer developer) =>
{
    devs.Create(developer);
    return Results.Created($"/developers/{developer.Id}", developer);
});

app.MapDelete("/developers/{id}", ([FromServices] DeveloperRepository devs, Guid id) =>
{
    devs.Delete(id);
    return Results.Ok();
});

app.MapPut("/developers", ([FromServices] DeveloperRepository devs, Guid id, Developer developer) =>
{
    if (devs.GetDeveloper(id) is null) return Results.NotFound();
    devs.Update(developer);
    return Results.Ok(developer);
});


app.Run();
