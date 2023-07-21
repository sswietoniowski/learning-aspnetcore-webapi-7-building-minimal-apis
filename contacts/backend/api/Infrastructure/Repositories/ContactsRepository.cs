using Contacts.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Infrastructure.Repositories;

public class ContactsRepository : IContactsRepository
{
    private readonly ContactsDbContext _dbContext;

    public ContactsRepository(ContactsDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync(string? search)
    {
        var query = _dbContext.Contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.LastName.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Contact?> GetContactAsync(int id)
    {
        return await _dbContext
            .Contacts.Include(c => c.Phones)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateContactAsync(Contact contact)
    {
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateContactAsync(Contact contact)
    {
        var contactFromDb = await _dbContext
            .Contacts
            .FirstOrDefaultAsync(c => c.Id == contact.Id);

        if (contactFromDb is null)
        {
            return false;
        }

        contactFromDb.FirstName = contact.FirstName;
        contactFromDb.LastName = contact.LastName;
        contactFromDb.Email = contact.Email;
        var quantity = await _dbContext.SaveChangesAsync();

        return quantity == 1;
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await _dbContext
            .Contacts
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contact is null)
        {
            return false;
        }

        _dbContext.Contacts.Remove(contact);
        var quantity = await _dbContext.SaveChangesAsync();

        return quantity == 1;
    }
}