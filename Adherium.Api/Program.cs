using FastEndpoints;
using FastEndpoints.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJwtBearer(o => o.SigningKey = builder.Configuration["Jwt:SigningKey"]!)
    .AddAuthorization();

//Add Db Context with SQLite


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();

namespace Adherium.Api
{
    public partial class Program; // required to make partial class accessible to xUnit 
}