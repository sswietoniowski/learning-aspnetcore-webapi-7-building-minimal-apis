using Contacts.Api.Infrastructure;
using Contacts.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Contacts.Api.Configurations.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("ContactsDb"));
            options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        });

        builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
    }

    public static void AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static void AddCors(this WebApplicationBuilder builder)
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
    }
}