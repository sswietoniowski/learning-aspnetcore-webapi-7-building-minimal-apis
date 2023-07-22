using AutoMapper;
using Contacts.Api.Domain;
using Contacts.Api.DTOs;
using Contacts.Api.Infrastructure;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

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

const int DefaultContactsPageNumber = 1;
const int DefaultContactsPageSize = 10;
const int MaxContactsPageSize = 50;

// GET api/contacts
// GET api/contacts?lastName=Nowak
// GET api/contacts?search=ski
// GET api/contacts?search=ski&orderBy=LastName&desc=true
app.MapGet("/api/contacts", async ([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
    int? pageNumber, int? pageSize,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper, HttpContext context) =>
{
    if (pageNumber is null)
    {
        pageNumber = DefaultContactsPageNumber;
    }

    if (pageSize is null)
    {
        pageSize = DefaultContactsPageSize;
    }

    if (pageNumber <= 0)
    {
        return Results.BadRequest();
    }

    if (pageSize > MaxContactsPageSize)
    {
        pageSize = MaxContactsPageSize;
    }

    var (contacts, paginationMetadata) = await repository.GetContactsAsync(lastName, search, orderBy, desc, (int)pageNumber, (int)pageSize);

    var contactsDto = mapper.Map<IEnumerable<ContactDto>>(contacts);

    context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

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

// GET api/contacts/1/phones/1
app.MapGet("/api/contacts/{contactId:int}/phones/{phoneId:int}", ([FromRoute] int contactId, [FromRoute] int phoneId,
    [FromServices] ContactsDbContext dbContext) =>
{
    var contact = dbContext.Contacts.Include(c => c.Phones)
        .FirstOrDefault(c => c.Id == contactId);

    if (contact is null)
    {
        return Results.NotFound();
    }

    var phone = contact.Phones.FirstOrDefault(p => p.Id == phoneId);

    if (phone is null)
    {
        return Results.NotFound();
    }

    var phoneDto = new PhoneDto(phone.Id, phone.Number, phone.Description);

    return Results.Ok(phoneDto);
});

// recreate & migrate the database on each run, for demo purposes
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.Migrate();

app.Run();
