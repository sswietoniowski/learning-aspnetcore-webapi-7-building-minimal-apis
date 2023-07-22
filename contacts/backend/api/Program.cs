using AutoMapper;
using Contacts.Api.DTOs;
using Contacts.Api.Infrastructure;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Contacts.Api.Domain;

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

// contacts:

// GET api/contacts?search=ski
app.MapGet("/api/contacts", async ([FromQuery] string? search,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contacts = await repository.GetContactsAsync(search);

    var contactsDto = mapper.Map<IEnumerable<ContactDto>>(contacts);

    return TypedResults.Ok(contactsDto);
});

// GET api/contacts/1
app.MapGet("/api/contacts/{id:int}", async ([FromRoute] int id,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contact = await repository.GetContactAsync(id);

    if (contact is null)
    {
        return Results.NotFound();
    }

    var contactDto = mapper.Map<ContactDto>(contact);

    return TypedResults.Ok(contactDto);
}).WithName("GetContact");

// POST api/contacts
app.MapPost("/api/contacts", async ([FromBody] ContactForCreationDto contactForCreationDto, 
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contact = mapper.Map<Contact>(contactForCreationDto);

    await repository.CreateContactAsync(contact);

    var contactDto = mapper.Map<ContactDto>(contact);

    return Results.CreatedAtRoute("GetContact", new { id = contactDto.Id }, contactDto);
});

// PUT api/contacts/1
app.MapPut("/api/contacts/{id:int}", async ([FromRoute] int id, [FromBody] ContactForUpdateDto contactForUpdateDto,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contact = mapper.Map<Contact>(contactForUpdateDto);
    contact.Id = id;

    var success = await repository.UpdateContactAsync(contact);

    if (!success)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

// DELETE api/contacts/1
app.MapDelete("/api/contacts/{id:int}", async ([FromRoute] int id, 
    [FromServices] IContactsRepository repository) =>
{
    var success = await repository.DeleteContactAsync(id);

    if (!success)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

// phones

// GET api/contacts/1/phones
app.MapGet("/api/contacts/{contactId:int}/phones", ([FromRoute] int contactId, 
    [FromServices] ContactsDbContext dbContext) =>
{
    var contact = dbContext.Contacts.Include(c => c.Phones)
        .FirstOrDefault(c => c.Id == contactId);

    if (contact is null)
    {
        return Results.NotFound();
    }

    var phonesDto = contact.Phones
        .Select(p => new PhoneDto(p.Id, p.Number, p.Description));

    return Results.Ok(phonesDto);
});

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
