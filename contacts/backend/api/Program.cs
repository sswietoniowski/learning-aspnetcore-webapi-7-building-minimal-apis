using Contacts.Api.Configurations.Extensions;
using Contacts.Api.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();
builder.AddMapper();

builder.AddCors();

builder.AddAuthentication();
builder.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add problem details
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // no need to add it explicitly, it's added by default
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // should be added at the beginning of the pipeline
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// contacts:
app.RegisterContactsEndpoints();

// phones
app.RegisterPhonesEndpoints();

// images -> and endpoint that throws exception (just for demo purposes)
app.MapGet("/api/images", string () => throw new NotImplementedException("This endpoint is not implemented yet!"));

// catch all endpoint
// app.MapFallback(async Task (context) =>
// {
//     context.Response.StatusCode = StatusCodes.Status404NotFound;
//     await context.Response.WriteAsync($"The endpoint you are looking for does not exist!");
// });

// catch all endpoint (must be registered after all other endpoints)
app.Map("/{*path}", [EnableCors] [ResponseCache(NoStore = true)] (string path) =>
{
    return Results.Problem(
        title: "The endpoint you are looking for does not exist!",
        detail: path,
        statusCode: StatusCodes.Status404NotFound);
}).ExcludeFromDescription();

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
