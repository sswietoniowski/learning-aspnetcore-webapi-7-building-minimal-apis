# Learning ASP.NET Core - WebAPI (.NET 7) Building Minimal APIs

This repository contains examples showing how to building a minimal API (WebAPI 7).

Based on this course [Building ASP.NET Core 7 Minimal APIs](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/exercise-files) and [here](https://github.com/KevinDockx/BuildingAspNetCore7MinimalAPIs).

## Table of Contents

- [Learning ASP.NET Core - WebAPI (.NET 7) Building Minimal APIs](#learning-aspnet-core---webapi-net-7-building-minimal-apis)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Introduction to ASP.NET Core Minimal APIs](#introduction-to-aspnet-core-minimal-apis)
  - [Learning About Core Concepts and Reading Resources](#learning-about-core-concepts-and-reading-resources)
    - [Dependency Injection in Minimal APIs](#dependency-injection-in-minimal-apis)
      - [Inversion of Control (IoC)](#inversion-of-control-ioc)
      - [Dependency Injection (DI)](#dependency-injection-di)
    - [Learning About Routing](#learning-about-routing)
    - [Working with Routing Templates](#working-with-routing-templates)
    - [Why You Shouldn’t Expose the Entity Model](#why-you-shouldnt-expose-the-entity-model)
      - [Entity model](#entity-model)
      - [DTO model](#dto-model)
    - [Adding the DTO Model and Using AutoMapper](#adding-the-dto-model-and-using-automapper)
    - [Parameter Binding](#parameter-binding)
    - [Modelling Common API Functionality](#modelling-common-api-functionality)
      - [Filtering](#filtering)
      - [Searching](#searching)
      - [Sorting](#sorting)
      - [Paging](#paging)
    - [Status Codes and Creating Responses](#status-codes-and-creating-responses)
    - [Creating Correct API Responses](#creating-correct-api-responses)
  - [Manipulating Resources](#manipulating-resources)
    - [Routing, Revisited](#routing-revisited)
    - [Creating a Resource \& Generating Links](#creating-a-resource--generating-links)
    - [Updating a Resource](#updating-a-resource)
    - [Deleting a Resource](#deleting-a-resource)
    - [Grouping Resources](#grouping-resources)
    - [Content Negotiation in Minimal APIs](#content-negotiation-in-minimal-apis)
    - [Validation in Minimal APIs](#validation-in-minimal-apis)
  - [Structuring Your Minimal API](#structuring-your-minimal-api)
    - [Options for Structuring Minimal APIs](#options-for-structuring-minimal-apis)
      - [Using Methods Instead of Inline Handlers](#using-methods-instead-of-inline-handlers)
      - [Separating Handler Methods Out in Classes](#separating-handler-methods-out-in-classes)
      - [Extending `IEndpointRouteBuilder` to Structure Your Minimal API](#extending-iendpointroutebuilder-to-structure-your-minimal-api)
      - [Combine the Previous Approaches](#combine-the-previous-approaches)
      - [3rd Party Libraries](#3rd-party-libraries)
      - [Other Approaches](#other-approaches)
  - [Handling Exceptions and Logging](#handling-exceptions-and-logging)
    - [Handling Exceptions in Minimal APIs](#handling-exceptions-in-minimal-apis)
    - [Using the Developer Exception Page Middleware](#using-the-developer-exception-page-middleware)
    - [Using the Exception Handler Middleware](#using-the-exception-handler-middleware)
    - [Improving Error Responses with Problem Details](#improving-error-responses-with-problem-details)
    - [Logging in Minimal APIs](#logging-in-minimal-apis)
  - [Implementing Business Logic with Endpoint Filters](#implementing-business-logic-with-endpoint-filters)
    - [Filters for Minimal APIs](#filters-for-minimal-apis)
    - [Creating an Endpoint Filter](#creating-an-endpoint-filter)
    - [Making the Endpoint Filter Reusable](#making-the-endpoint-filter-reusable)
    - [Chaining Endpoint Filters and Applying Them to a Group](#chaining-endpoint-filters-and-applying-them-to-a-group)
    - [Applying Business Logic Depending on the Response](#applying-business-logic-depending-on-the-response)
    - [Handling Request Validation](#handling-request-validation)
  - [Securing Your Minimal API](#securing-your-minimal-api)
    - [High-level API Security Overview](#high-level-api-security-overview)
    - [Token-based Security for Minimal APIs](#token-based-security-for-minimal-apis)
    - [Requiring a Bearer Token](#requiring-a-bearer-token)
    - [Generating a Token](#generating-a-token)
    - [Generating a Token with dotnet-user-jwts](#generating-a-token-with-dotnet-user-jwts)
    - [Creating and Applying an Authorization Policy](#creating-and-applying-an-authorization-policy)
  - [Documenting Your Minimal API](#documenting-your-minimal-api)
    - [A Few Words on Swagger / OpenAPI](#a-few-words-on-swagger--openapi)
    - [Adding Support for OpenAPI with Swashbuckle](#adding-support-for-openapi-with-swashbuckle)
    - [Adding Descriptions and Summaries](#adding-descriptions-and-summaries)
    - [Describing Response Types and Status Codes](#describing-response-types-and-status-codes)
    - [Describing Request Types](#describing-request-types)
    - [Gaining Full OpenApiOperation Control](#gaining-full-openapioperation-control)
    - [Describing API Security in Swagger](#describing-api-security-in-swagger)
  - [Extras](#extras)
    - [Testing your local API with ngrok](#testing-your-local-api-with-ngrok)
    - [Catch-all Endpoint](#catch-all-endpoint)
  - [Summary](#summary)

## Setup

To run API:

```cmd
cd .\contacts\backend\api
dotnet restore
dotnet build
dotnet watch run
```

If you want to run selected profile, you can use:

```cmd
dotnet run --launch-profile <profile-name>
```

In my case it could be for example:

```cmd
dotnet run --launch-profile "Contacts API (Development)"
```

## Introduction to ASP.NET Core Minimal APIs

To create a new minimal API project, just choose the "Web API" template in Visual Studio or use the `dotnet new webapi` command like so (just an example):

```cmd
dotnet new webapi --framework net7.0 --language C# --no-https false --auth None --use-minimal-apis true --use-program-main false --output "." --force
```

To test your project you can use:

- :+1: [Postman](https://www.postman.com/) - especially useful for testing REST APIs as presented [here](https://youtu.be/zp5Jh2FIpF0),
- [Insomnia](https://insomnia.rest/),
- [curl](https://curl.se/),
- :+1: [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) - my personal favorite as I am a big fan of Visual Studio Code,
- [RESTer](https://addons.mozilla.org/en-US/firefox/addon/rester/) (Firefox Add-on),
- ... and many more.

If you are using Visual Studio or JetBrains Rider you can use the built-in tools. Latest versions of Visual Studio have built-in support for testing REST APIs, more on that [here](https://youtu.be/ud0wx5mgniI).

I'd like to concentrate on the new/different features of the minimal APIs in .NET 7, so I started with an existing code for my domain classes, DTOs, database contexts, etc. What I'll be using as my example will be a simple API for managing contacts.

As this project is using .NET 7 and C# 11, I've decided to try out new language features provided by the latest version of C#. One of them is `required` keyword for properties (+ `[SetsRequiredMembers]` attribute), more on that [here](https://youtu.be/_hQPSOocXs0). I also decided to refactor DTOs to use `record` keyword, more on that [here](https://youtu.be/9Byvwa9yF-I).

## Learning About Core Concepts and Reading Resources

Basic information about core concepts.

### Dependency Injection in Minimal APIs

#### Inversion of Control (IoC)

> The **IoC** (Inversion of Control) pattern delegates the function of selecting a concrete implementation type for a class's dependencies to an external component.

#### Dependency Injection (DI)

> The **DI** pattern uses an object - the container - to initialize other objects, manage their lifetime, and provide the required dependencies to objects.

I'm assuming that whoever reads this information knows how to inject dependencies in ASP.NET Core and how to build an API using controllers, so I'm only trying to point out the difference between the two approaches.

As its older counterpart (that is controllers) minimal APIs are heavily using DI.

One difference being that instead of using constructor injection, you can use handlers to inject dependencies, like so:

```csharp
// phones

// GET api/contacts/1/phones
app.MapGet("/api/contacts/{contactId:int}/phones", (int contactId, ContactsDbContext dbContext) =>
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
```

In our case we are injecting `ContactsDbContext` into our handler. We can do that because (obviously) we've registered it in `Program.cs`.

### Learning About Routing

> **Routing** is the process of matching an HTTP method & URI to a specific route handler.

`app.MapAction` (where `Action` is the HTTP method), the process of routing is provided by an implementation of `IEndpointRouteBuilder`.

We've got:

- `MapGet`,
- `MapPost`,
- `MapPut`,
- `MapDelete`.

We should use HTTP methods as intended, that improves overall reliability of your system.

Different components in you architecture will rely on correct use of the HTTP standard:

- don't introduce potential inefficiencies (for example caching won't work correctly if you use `POST` instead of `GET`),
- don't introduce potential bugs.

Reference: [RFC9110](https://datatracker.ietf.org/doc/html/rfc9110).

### Working with Routing Templates

Example of such route template was already presented:

```csharp
// GET api/contacts/1/phones
app.MapGet("/api/contacts/{contactId:int}/phones", (int contactId, ContactsDbContext dbContext) =>
{
    // ...
});
```

As you can see, it looks identical to the one that would be used in controllers.

We can use _route parameters_ (`{contactId:int}`) to gather input via the URI. These parameters will be bound to same-name parameters in the handler signature. We can also use _route constraint_ (`int` in this case) to specify the type of parameters.

More on that [here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-7.0).

### Why You Shouldn’t Expose the Entity Model

#### Entity model

> **Entity model** a means to represent database rows as objects (object graphs).

#### DTO model

> **DTO model** a means to represent the data that's sent over the wire.

You shouldn't expose the entity model because there could be potential differences between the entity model and the DTO model. For example, you could have a property in the entity model that you don't want to expose in the DTO model.

> Keeping the entity and DTO model separate leads to more robust, reliably evolvable code.

In general all the reasons for the separation of the two, that were true for controllers, are also true for minimal APIs.

### Adding the DTO Model and Using AutoMapper

We can use AutoMapper to map between the entity model and the DTO model.

Provided that I've already added required dependencies and mapper profiles, then I can use it in my handler like so:

```csharp
// contacts:

// GET api/contacts?search=ski
app.MapGet("/api/contacts", async ([FromQuery] string? search, IContactsRepository repository, IMapper mapper) =>
{
    var contacts = await repository.GetContactsAsync(search);

    var contactsDto = mapper.Map<IEnumerable<ContactDto>>(contacts);

    return Results.Ok(contactsDto);
});
```

You might not notice but this handler is asynchronous. We can use `async` keyword in our handlers. It is preferred to use `async` handlers, because it allows us to use `await` keyword and exploit the benefits of asynchronous programming.

### Parameter Binding

_Parameter binding_ is the process of converting request data into strongly typed parameters that are expressed by route handlers.

There are many binding sources:

- route values (`[FromRoute]`),
- query string (`[FromQuery]`),
- header (`[FromHeader]`),
- body (as JSON) (`[FromBody]`),
- services provided by DI (`[FromServices]`),
- custom.

We can use binding attributes to be explicit about the biding source.

Our previous code with explicit binding will look like this:

```csharp
// contacts:

// GET api/contacts?search=ski
app.MapGet("/api/contacts", async ([FromQuery] string? search, [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
  // ...
});

// phones

// GET api/contacts/1/phones
app.MapGet("/api/contacts/{contactId:int}/phones", ([FromRoute] int contactId, [FromServices] ContactsDbContext dbContext) =>
{
  // ...
});
```

There are rules for selecting the binding source, explicit binding is used first, then special types are used.

Those special types are:

- `HttpContext`,
- `HttpRequest` (`HttpContext.Request`),
- `HttpResponse` (`HttpContext.Response`),
- `ClaimsPrincipal` (`HttpContext.User`),
- `CancellationToken` (`HttpContext.RequestAborted`),
- `IFormFileCollection` (`HttpContext.Request.Form.Files`),
- `IFormFile` (`HttpContext.Request.Form.Files[paramName]`),
- `Stream` (`HttpContext.Request.Body`),
- `PipeReader` (`HttpContext.Request.BodyReader`).

All rules for selecting the binding source (from the most important to the least important):

- explicit binding,
- special types,
- `BindingAsync` (custom binding),
- string or `TryParse` (if the parameter is a string or nullable),
- services,
- request body.

### Modelling Common API Functionality

There are some functionalities that most APIs would need.

There is no built-in support for them in ASP.NET Core, but we can model them ourselves. Truth be told, we can implement them in many different ways (some more elegant than others). I'll show you one (relatively simple) way of doing it.

#### Filtering

> Limiting a collection resource, taking into account a predicate.

Filter via the query string, using name/value combinations for fields to filter on.

```csharp
// GET api/contacts
// GET api/contacts?lastName=Nowak
// GET api/contacts?search=ski
// GET api/contacts?search=ski&orderBy=LastName&desc=true
app.MapGet("/api/contacts", async ([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contacts = await repository.GetContactsAsync(lastName, search, orderBy, desc);

    var contactsDto = mapper.Map<IEnumerable<ContactDto>>(contacts);

    return TypedResults.Ok(contactsDto);
});
```

#### Searching

> Searching for matching items in a collection based on a predefined set of rules..

Search via the query string, passing through a value to search for. It's up to the API to decide what to search
through and how to search.

Showed in previous example.

#### Sorting

Sort via the query string, passing through fields and (optional) direction. Allow sorting on properties of the DTO,
not on entity properties.

Showed in previous example.

#### Paging

Page via the query string, passing through page number & page size. Page by default. Limit the page size. Return pagination metadata in a response header.

Example:

```csharp
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
```

That would work provided that we have `PaginationMetadata` class:

```csharp
public class PaginationMetadata
{
    public int TotalItemCount { get; init; }
    public int TotalPageCount { get; init; }
    public int PageSize { get; init; }
    public int CurrentPage { get; init; }

    public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
    }
}
```

and our `GetContactsAsync` method would look like this (it's been slightly modified):

```csharp
    public async Task<(IEnumerable<Contact>, PaginationMetadata)> GetContactsAsync(string? lastName, string? search, string? orderBy, bool? desc, int pageNumber, int pageSize)
    {
        var query = _dbContext.Contacts.AsQueryable();

        // this solution is not as good as the one with the specification pattern

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(c => c.LastName == lastName);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.LastName.Contains(search));
        }

        var totalItemCount = await query.CountAsync();

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            if (orderBy.Equals(nameof(Contact.LastName), StringComparison.OrdinalIgnoreCase))
            {
                query = desc == true ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName);
            }
            else if (orderBy.Equals(nameof(Contact.FirstName), StringComparison.OrdinalIgnoreCase))
            {
                query = desc == true ? query.OrderByDescending(c => c.FirstName) : query.OrderBy(c => c.FirstName);
            }
            else if (orderBy.Equals(nameof(Contact.Email), StringComparison.OrdinalIgnoreCase))
            {
                query = desc == true ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email);
            }
        }

        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        var collectionToReturn = await query
            .Skip(pageSize * (pageNumber - 1)) // must be last :-)!
            .Take(pageSize)
            .ToListAsync();

        return (collectionToReturn, paginationMetadata);
    }
```

### Status Codes and Creating Responses

> Status codes are inspected by the consumer to know how the request went.

Status codes tell the consumer:

- if the request worked out as expected,
- if something went wrong, who's responsible it is.

> Using the correct status codes is essential.

Common status codes:

- 200 OK,
- 201 Created,
- 204 No Content,
- 400 Bad Request,
- 401 Unauthorized,
- 403 Forbidden,
- 404 Not Found,
- 405 Method Not Allowed,
- 500 Internal Server Error.

### Creating Correct API Responses

From a minimal API endpoint we can return a `string`, any other type of object or `IResult` based types:

- `Results.X` (where `X` is for example `Ok`),
- `TypedResults.X` (preferred if you need to know why, watch [this](https://youtu.be/BmwJkoPnF24) video for more information).

If we know in advance what type of response we want to return, we can add that information to the handler signature, like so (`Task<Results<Ok<ContactDto>, NotFound>>` - as you can see we are returning either `Ok` or `NotFound`):

```csharp
app.MapGet("/api/contacts/{id:int}", async Task<Results<Ok<ContactDto>, NotFound>> ([FromRoute] int id,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
    var contact = await repository.GetContactAsync(id);

    if (contact is null)
    {
        return TypedResults.NotFound();
    }

    var contactDto = mapper.Map<ContactDto>(contact);

    return TypedResults.Ok(contactDto);
}).WithName("GetContact");
```

What is worth noting is that we can now use `TypedResults.NotFound` instead of `Results.NotFound` (and other `Results` methods). Without the method signature we would have to use `Results.NotFound()`.

This is the preferred way of returning responses from minimal APIs. In my example I decided to change only this one handler, but you should change all of them.

## Manipulating Resources

How to perform typical CRUD operations.

### Routing, Revisited

Couple rules:

- make sure the URLs make sense,
- use nouns in URLs, not verbs,
- don't mix plural and singular nouns.

BTW do you know the difference between _URI_ and _URL_? If not look up [here](https://pl.wikipedia.org/wiki/Uniform_Resource_Identifier).

> Naming guidelines are not technical limitations, but they keep your API contract clean and predictable.

### Creating a Resource & Generating Links

To create a resource we can use `MapPost` method, like so:

```csharp
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
```

I had to create first a `MapGet` method to be able to use `CreatedAtRoute` method. Look at use of `TypedResults.Ok` and `WithName` method.

Things to keep in mind when creating a resource:

- when working with parent/chid relationships, validate whether the parent exists,
- don't use the same endpoint for creating one item & a collection of items (create a new endpoint instead: `/itemcollections`).

### Updating a Resource

To update a resource we can use `MapPut` method, like so:

```csharp
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
```

Things to keep in mind when updating a resource:

- check if the resource exists, including hierarchical parents,
- be careful when enabling `PUT` for collections (it's not a common practice & can be very destructive),
- `PUT` is intended for `FULL` updates, `PATCH` is for partial updates.

**Remarks:**

> Change sets for `PATCH` requests are often described as a list of operations (`JsonPatchDocument`). There is
> no support for this for minimal APIs...

### Deleting a Resource

To delete a resource we can use `MapDelete` method, like so:

```csharp
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
```

### Grouping Resources

Sometimes we need some form of organization for our endpoints. For that we can use grouping.

My code before applying grouping (that is up to this point) can be found [here](https://github.com/sswietoniowski/learning-aspnetcore-webapi-7-building-minimal-apis/tree/57b47c31f8f20068fb9c25e8306c84d44e852463).

To group resources we would need first to create our groups, like so:

```csharp
// /api/contacts
var contactsEndpoints = app.MapGroup("/api/contacts");
// /api/contacts/1/phones
var phonesEndpoints = contactsEndpoints.MapGroup("/{contactId:int}/phones");
```

And then we can use it like so:

```csharp
// contacts:

// ...
contactsEndpoints.MapGet("", async Task<Results<Ok<IEnumerable<ContactDto>>, BadRequest>> ([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
    int? pageNumber, int? pageSize,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper, HttpContext context) =>
{
  // ...
});

contactsEndpoints.MapGet("{id:int}", async Task<Results<Ok<ContactDto>, NotFound>> ([FromRoute] int id,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper) =>
{
  // ...
}).WithName("GetContact");

// ...

// phones

phonesEndpoints.MapGet("", Results<Ok<IEnumerable<PhoneDto>>, NotFound> ([FromRoute] int contactId,
    [FromServices] ContactsDbContext dbContext) =>
{
  // ...
});
```

This approach is quite useful when you have a lot of endpoints. More on that [here](https://youtu.be/8c73j7RHoSQ).

My code after refactoring to use grouping can be found [here](https://github.com/sswietoniowski/learning-aspnetcore-webapi-7-building-minimal-apis/tree/3b5f1e45b15e656c7ca69cff00c1a427df6c6002).

### Content Negotiation in Minimal APIs

> **Content negotiation** is the process of selecting the best representation for a given response when there are multiple representations available.

**Remarks:**

> Content negotiation is not supported out of the box by minimal APIs (nor is it planned). If you need it, consider using controllers instead. If you want something like that check out [Carter](https://github.com/CarterCommunity/Carter).

### Validation in Minimal APIs

Input validation is a common requirement for APIs:

- required fields, format, value & length restrictions, cross-field rules, ...

**Remarks:**

> Validation is not supported out of the box by minimal APIs (nor is it planned). If you need it, consider using controllers instead. Alternatively you can use [FluentValidation](https://github.com/FluentValidation/FluentValidation), [MiniValidation](https://github.com/DamianEdwards/MiniValidation) or [o9d-aspnet](https://github.com/benfoster/o9d-aspnet) libraries.

## Structuring Your Minimal API

Having all handlers in on file (`Program.cs`) is not a good idea. We can structure our minimal API in many different ways.

### Options for Structuring Minimal APIs

There are many options for structuring minimal APIs - we can combine them as we see fit.

#### Using Methods Instead of Inline Handlers

Improves maintainability and testability.

Example for a single handler:

```csharp
// GET api/contacts
// GET api/contacts?lastName=Nowak
// GET api/contacts?search=ski
// GET api/contacts?search=ski&orderBy=LastName&desc=true
contactsEndpoints.MapGet("", GetContactsHandler);

// ReSharper disable once InconsistentNaming
const int DefaultContactsPageNumber = 1;
// ReSharper disable once InconsistentNaming
const int DefaultContactsPageSize = 10;
// ReSharper disable once InconsistentNaming
const int MaxContactsPageSize = 50;

async Task<Results<Ok<IEnumerable<ContactDto>>, BadRequest>> GetContactsHandler([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
    int? pageNumber, int? pageSize,
    [FromServices] IContactsRepository repository, [FromServices] IMapper mapper, HttpContext context)
{
    // ...
}
```

This way we can test our handler separately.

#### Separating Handler Methods Out in Classes

Cleans up the `Program.cs` file.

First I created a new directory `Handlers`, then in it I created a new file `ContactsHandlers.cs`:

```csharp
using AutoMapper;
using Contacts.Api.DTOs;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contacts.Api.Domain;

namespace Contacts.Api.Handlers;

public static class ContactsHandlers
{
    // ReSharper disable once InconsistentNaming
    const int DefaultContactsPageNumber = 1;
    // ReSharper disable once InconsistentNaming
    const int DefaultContactsPageSize = 10;
    // ReSharper disable once InconsistentNaming
    const int MaxContactsPageSize = 50;

    public static async Task<Results<Ok<IEnumerable<ContactDto>>, BadRequest>> GetContacts([FromQuery] string? lastName, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] bool? desc,
        int? pageNumber, int? pageSize,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper, HttpContext context)
    {
        // ...
    }

    public static async Task<Results<Ok<ContactDto>, NotFound>> GetContact([FromRoute] int id,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        // ...
    }

    public static async Task<CreatedAtRoute<ContactDto>> CreateContact([FromBody] ContactForCreationDto contactForCreationDto,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        // ...
    }

    public static async Task<Results<NoContent, NotFound>> UpdateContact([FromRoute] int id, [FromBody] ContactForUpdateDto contactForUpdateDto,
        [FromServices] IContactsRepository repository, [FromServices] IMapper mapper)
    {
        // ...
    }

    public static async Task<Results<NoContent, NotFound>> DeleteContact([FromRoute] int id,
        [FromServices] IContactsRepository repository)
    {
        // ...
    }
}
```

Analogically I created a new file `PhonesHandlers.cs` (code omitted for brevity).

Then I modified my `Program.cs` file to use those handlers:

```csharp
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
```

It is a lot cleaner now.

#### Extending `IEndpointRouteBuilder` to Structure Your Minimal API

Improves code structure.

This time I first created a new directory `Extensions` inside `Configurations` directory and then in it I created a new file `EndpointRouteBuilderExtensions`:

```csharp
using Contacts.Api.Handlers;

namespace Contacts.Api.Configurations.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterContactsEndpoints(this IEndpointRouteBuilder app)
    {
        var contactsEndpoints = app.MapGroup("/api/contacts");

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
    }

    public static void RegisterPhonesEndpoints(this IEndpointRouteBuilder app)
    {
        var phonesEndpoints = app.MapGroup("/api/contacts/{contactId:int}/phones");

        // GET api/contacts/1/phones
        phonesEndpoints.MapGet("", PhonesHandlers.GetPhones);

        // GET api/contacts/1/phones/1
        phonesEndpoints.MapGet("{phoneId:int}", PhonesHandlers.GetPhone);
    }
}
```

Now my `Program.cs` file looks like this:

```csharp
// ...

// contacts:
app.RegisterContactsEndpoints();

// phones
app.RegisterPhonesEndpoints();

// ...
```

It is a lot simpler now.

#### Combine the Previous Approaches

Improves testability, maintainability and overall code structure.

And that is exactly what I did.

I combined all the previous approaches :-).

My final version is little changed, I refactored my handler methods to use proper naming (I've added `Async` suffix to them - when applicable) and I've decided to change `Handlers`
directory to `EndpointHandlers` and move it into `Configurations` directory.

My final version can be found [here](https://github.com/sswietoniowski/learning-aspnetcore-webapi-7-building-minimal-apis/tree/d28bf28a66a42403d9e03e7999e58cc08f90e047).

#### 3rd Party Libraries

There are many 3rd party libraries that can help you structure your minimal API.

For example you can use [Carter](https://github.com/CarterCommunity/Carter) library (it was already mentioned in the previous section).

#### Other Approaches

Because there is no built-in support for structuring minimal APIs, you can use any approach you want,
and many people came up with their own solutions. For example something like [this](https://youtu.be/Q_zXFeP-QNI).

## Handling Exceptions and Logging

We need a way to handle exceptions and log them in our applications.

### Handling Exceptions in Minimal APIs

> Handling exceptions is a common task for any ASP.NET Core application and it's not that different for minimal APIs.

To test out how exception handling mechanism works, I've added a new handler:

```csharp
// images -> and endpoint that throws exception (just for demo purposes)
app.MapGet("/api/images", string () => throw new NotImplementedException("This endpoint is not implemented yet!"));
```

To see different ways of handling exceptions, I've changed `launchSettings.json` like so:

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:5000",
      "sslPort": 5001
    }
  },
  "profiles": {
    "Contacts API (Development)": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "launchUrl": "",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "isDefault": true
    },
    "Contacts API (Production)": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "launchUrl": "",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": false,
      "launchUrl": "",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    }
  }
}
```

`Contacts API (Development)` will run in development environment , `Contacts API (Production)` or `IIS Express` will run in production environment.

If you encounter an error while trying to use IIS Express, read [this](https://stackoverflow.com/questions/64880715/https-error-in-asp-net-core-app-running-on-iisexpress-pr-connect-reset-error)
and consider using `Tests\RegenerateIISExpressCertificate.ps1` script.

### Using the Developer Exception Page Middleware

_Developer Exception Page Middleware_ exposes stack traces for unhandled exceptions:

- useful during development,
- should **NOT** be used outside of a development environment.

It is enabled by default when:

- running in the development environment (`"ASPNETCORE_ENVIRONMENT": "Development"`),
- **AND** the app is set up with a call into `WebApplication.CreateBuilder` (which is the case for minimal APIs).

My call (using built-in REST support - `Test\contacts.http`):

```http
### Call images endpoint to receive an error
GET {{baseUri}}/images HTTP/1.1
```

Sample response in development mode:

```text
Response time: 3573 ms
Status code: InternalServerError (500)
Transfer-Encoding: chunked
Date: Sun, 23 Jul 2023 10:17:59 GMT
Server: Kestrel

Content-Type: text/plain; charset=utf-8
Content-Length: 862

------------------------------------------------
Content:
System.NotImplementedException: This endpoint is not implemented yet!
   at Program.<>c.<<Main>$>b__0_0() in ...\contacts\backend\api\Program.cs:line 34
   at lambda_method29(Closure, Object, HttpContext)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
--- End of stack trace from previous location ---
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

HEADERS
=======
Connection: keep-alive
Host: localhost:5001
User-Agent: Visual-Studio/17.6.5+33829.357 WebToolsExtension/17.6.326.62524
```

and in production mode:

```text
Response time: 3413 ms
Status code: InternalServerError (500)
Date: Sun, 23 Jul 2023 10:35:38 GMT
Server: Kestrel

Content-Length: 0
```

### Using the Exception Handler Middleware

_Exception Handler Middleware_ produces an error payload without exposing stack traces:

- useful when **NOT** working in a development environment,
- handles & logs exceptions.

It is **NOT** enabled by default:

- enabled with a call into `app.UseExceptionHandler`.

To use this middleware I've changed `Program.cs` like so:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // should be added at the beginning of the pipeline
    app.UseExceptionHandler(applicationBuilder =>
    {
        applicationBuilder.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";

                await context.Response.WriteAsync("An unexpected problem happened!");
            });
    });
}
```

And now I get the following response:

```text
Response time: 2625 ms
Status code: InternalServerError (500)
Pragma: no-cache
Transfer-Encoding: chunked
Cache-Control: no-store, no-cache
Date: Sun, 23 Jul 2023 10:47:39 GMT
Server: Kestrel

Content-Type: text/html
Expires: -1
Content-Length: 31

------------------------------------------------
Content:
An unexpected problem happened!
```

### Improving Error Responses with Problem Details

Our error responses are not very informative. We can improve them by using [_Problem Details_](https://datatracker.ietf.org/doc/html/rfc7807).

Provided via `IProblemDetailsService`. Default implementation is included with ASP.NET Core.

Effects of enabling the default problem details service:

- the developer exception page middleware will automatically generate a problem details response,
- the exception handler middleware will automatically generate a problem details response,
- the status code page middleware can be configured to generate problem details responses for empty bodies (e.g.: 400, 404, ...).

To enable it I've changed `Program.cs` like so:

```csharp
builder.Services.AddProblemDetails();

// ...

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
```

To see proper results be sure to add `Accept` to your request:

```http
### Call images endpoint to receive an error
GET {{baseUri}}/images HTTP/1.1
Accept: application/json
```

Now I get the following response (`Development` environment):

```text
HTTP/1.1 500 Internal Server Error
Connection: close
Content-Type: application/problem+json
Date: Sun, 23 Jul 2023 14:36:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "System.NotImplementedException",
  "status": 500,
  "detail": "This endpoint is not implemented yet!",
  "exception": {
    "details": "System.NotImplementedException: This endpoint is not implemented yet!\r\n   at Program.<>c.<<Main>$>b__0_0() in ...\\contacts\\backend\\api\\Program.cs:line 49\r\n   at lambda_method29(Closure, Object, HttpContext)\r\n   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)",
    "headers": {
      "Accept": [
        "application/json"
      ],
      "Connection": [
        "close"
      ],
      "Host": [
        "localhost:5001"
      ],
      "User-Agent": [
        "vscode-restclient"
      ],
      "Accept-Encoding": [
        "gzip, deflate"
      ]
    },
    "path": "/api/images",
    "endpoint": "HTTP: GET /api/images",
    "routeValues": {}
  }
}
```

And (`Production` environment):

```text
HTTP/1.1 500 Internal Server Error
Connection: close
Content-Type: application/problem+json
Date: Sun, 23 Jul 2023 14:40:13 GMT
Server: Kestrel
Cache-Control: no-cache,no-store
Expires: -1
Pragma: no-cache
Transfer-Encoding: chunked

{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500
}
```

If you're more interested read [this](https://www.strathweb.com/2022/08/problem-details-responses-everywhere-with-asp-net-core-and-net-7/) and [this](https://meysamhadeli.com/problem-details-in-dotnet-7/) articles.

### Logging in Minimal APIs

> Not different from "regular" ASP.NET Core applications.

To use it:

- configure a `Logger`,
- inject it to log where needed.

Example:

```csharp
public static class PhonesHandlers
{
    public static Results<Ok<IEnumerable<PhoneDto>>, NotFound> GetPhones([FromRoute] int contactId,
        [FromServices] ContactsDbContext dbContext, ILogger<PhoneDto> logger)
    {
        logger.LogInformation("Getting phones for contact with id {contactId}", contactId);

        // ...
    }

    public static Results<Ok<PhoneDto>, NotFound> GetPhone([FromRoute] int contactId, [FromRoute] int phoneId,
        [FromServices] ContactsDbContext dbContext, ILogger<PhoneDto> logger)
    {
        logger.LogInformation("Getting phone with id {phoneId} for contact with id {contactId}", phoneId, contactId);

        // ...
    }
}
```

I had to add `ILogger<PhoneDto> logger` parameter to my handler methods and then I had to add `logger.LogInformation` calls to them. For some it might be strange that I'm using `PhoneDto` as a generic parameter, I did so out of necessity (I couldn't use `ILogger` as a generic parameter or use `PhonesHandlers` as a generic parameter because it's a static class).

## Implementing Business Logic with Endpoint Filters

### Filters for Minimal APIs

### Creating an Endpoint Filter

### Making the Endpoint Filter Reusable

### Chaining Endpoint Filters and Applying Them to a Group

### Applying Business Logic Depending on the Response

### Handling Request Validation

## Securing Your Minimal API

### High-level API Security Overview

### Token-based Security for Minimal APIs

### Requiring a Bearer Token

### Generating a Token

### Generating a Token with dotnet-user-jwts

### Creating and Applying an Authorization Policy

## Documenting Your Minimal API

### A Few Words on Swagger / OpenAPI

### Adding Support for OpenAPI with Swashbuckle

### Adding Descriptions and Summaries

### Describing Response Types and Status Codes

### Describing Request Types

### Gaining Full OpenApiOperation Control

### Describing API Security in Swagger

## Extras

Couple of things that I found interesting.

### Testing your local API with ngrok

[Ngrok](https://ngrok.com/) is a tool that allows you to expose your local API to the Internet. This capability is very useful when you want to test your API (or your website for that matter) on a real device (like a phone).

First you need to install this tool, I'm using Windows and provided that I've got [`Chocolatey`](https://chocolatey.org/) installed, I can install `ngrok` like so:

```powershell
choco install ngrok
```

You also need to create an account on [ngrok.com](https://ngrok.com/).

After that you will need to add your `authtoken` to `ngrok` like so:

```powershell
ngrok authtoken <your_auth_token>
```

Then you need to run your API and then run `ngrok` like so.

First run your API:

```powershell
dotnet run
```

Then run `ngrok`:

```powershell
ngrok http https://localhost:5001 --host-header="localhost:5001"
```

Now you can use the provided URL to test your API.

For example in my case I can use `https://6567-93-181-134-228.ngrok-free.app/api/contacts` (in your case this address would be different) to test my API.

### Catch-all Endpoint

We might want to have a catch-all endpoint that will handle all requests that are not handled by other endpoints.

One way to do it is to add a catch-all endpoint at the end of your `Program.cs` file, like so:

```csharp
// catch all endpoint
app.MapFallback(async Task (context) =>
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    await context.Response.WriteAsync($"The endpoint you are looking for does not exist!");
});
```

Now when calling an endpoint that does not exist:

```http
### Call to non-existing endpoint
GET {{baseUri}}/non-existing HTTP/1.1
Accept: application/json
```

I will get the following response:

```text
HTTP/1.1 404 Not Found
Connection: close
Date: Mon, 24 Jul 2023 11:45:13 GMT
Server: Kestrel
Transfer-Encoding: chunked

The endpoint you are looking for does not exist!
```

Alternatively we can use something like this instead:

```csharp
// catch all endpoint (must be registered after all other endpoints!)
// catch all endpoint (must be registered after all other endpoints)
app.Map("/{*path}", (string path) =>
{
    return Results.Problem(
        title: "The endpoint you are looking for does not exist!",
        detail: path,
        statusCode: StatusCodes.Status404NotFound);
}).ExcludeFromDescription();
```

And our result for the same request will be:

```text
HTTP/1.1 404 Not Found
Connection: close
Content-Type: application/problem+json
Date: Mon, 24 Jul 2023 12:02:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "The endpoint you are looking for does not exist!",
  "status": 404,
  "detail": "api/non-existing"
}
```

## Summary

Now you know how to build a minimal API in .NET 7.0.
