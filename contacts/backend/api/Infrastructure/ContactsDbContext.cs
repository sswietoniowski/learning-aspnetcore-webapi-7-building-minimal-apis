using Contacts.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Infrastructure;

public class ContactsDbContext : DbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Phone> Phones => Set<Phone>();

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasData(new List<Contact>
        {
            new Contact
            {
                Id = 1, FirstName = "Jan", LastName = "Kowalski [EF Core]", Email = "jkowalski@u.pl",
            },
            new Contact
            {
                Id = 2, FirstName = "Adam", LastName = "Nowak", Email = "anowak@u.pl"
            }
        });

        modelBuilder.Entity<Phone>().HasData(new List<Phone>
        {
            new Phone
            {
                Id = 1, Number = "111-111-1111", Description = "Domowy", ContactId = 1
            },
            new Phone
            {
                Id = 2, Number = "222-222-2222", Description = "Służbowy", ContactId = 1
            }
        });
    }
}