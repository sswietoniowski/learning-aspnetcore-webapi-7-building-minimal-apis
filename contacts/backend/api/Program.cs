using Contacts.Api.Configurations.Extensions;
using Contacts.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();
builder.AddMapper();

builder.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// add problem details
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

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

app.UseCors();

// contacts:
app.RegisterContactsEndpoints();

// phones
app.RegisterPhonesEndpoints();

// images -> and endpoint that throws exception (just for demo purposes)
app.MapGet("/api/images", string () => throw new NotImplementedException("This endpoint is not implemented yet!"));

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
