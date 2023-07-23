using Contacts.Api.Configurations.Extensions;
using Contacts.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();
builder.AddMapper();

builder.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

// contacts:
app.RegisterContactsEndpoints();

// phones
app.RegisterPhonesEndpoints();

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
