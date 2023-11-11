using Contacts.Api.Infrastructure;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Contacts.Api.Configurations.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("ContactsDb"));
            options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        });

        builder.Services.AddScoped<IContactsRepository, ContactsRepository>();

        return builder;
    }

    public static WebApplicationBuilder AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return builder;
    }

    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
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

        return builder;
    }

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication().AddJwtBearer();

        return builder;
    }

    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminFromPoland", policy =>
                policy
                    .RequireRole("admin")
                    .RequireClaim("country", "Poland"));

        return builder;
    }
}