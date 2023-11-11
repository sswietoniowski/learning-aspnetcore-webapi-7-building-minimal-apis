using Contacts.Api.Configurations.Extensions;
using Contacts.Api.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// add persistence configuration
builder.AddPersistence();
// add mapper configuration
builder.AddMapper();

// add CORS configuration
builder.AddCors();

// add authentication & authorization configuration
builder.AddAuthentication();
builder.AddAuthorization();

// add Swagger configuration
builder.AddSwagger();

// add problem details
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // no need to add it explicitly, it's added by default
    app.UseDeveloperExceptionPage();
}
else
{
    // should be added at the beginning of the pipeline
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

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
