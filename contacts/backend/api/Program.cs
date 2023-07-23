using Contacts.Api.Infrastructure;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Contacts.Api.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContactsDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ContactsDb"));
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

builder.Services.AddScoped<IContactsRepository, ContactsRepository>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

// grouping endpoints

var contactsEndpoints = app.MapGroup("/api/contacts");
var phonesEndpoints = contactsEndpoints.MapGroup("/{contactId:int}/phones");

// contacts:

// GET api/contacts
// GET api/contacts?lastName=Nowak
// GET api/contacts?search=ski
// GET api/contacts?search=ski&orderBy=LastName&desc=true
contactsEndpoints.MapGet("", ContactsHandlers.GetContacts);

// GET api/contacts/1
contactsEndpoints.MapGet("{id:int}", ContactsHandlers.GetContact).WithName("GetContact");

// POST api/contacts
contactsEndpoints.MapPost("", ContactsHandlers.CreateContact);

// PUT api/contacts/1
contactsEndpoints.MapPut("{id:int}", ContactsHandlers.UpdateContact);

// DELETE api/contacts/1
contactsEndpoints.MapDelete("{id:int}", ContactsHandlers.DeleteContact);

// phones

// GET api/contacts/1/phones
phonesEndpoints.MapGet("", PhonesHandlers.GetPhones);

// GET api/contacts/1/phones/1
phonesEndpoints.MapGet("{phoneId:int}", PhonesHandlers.GetPhone);

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
