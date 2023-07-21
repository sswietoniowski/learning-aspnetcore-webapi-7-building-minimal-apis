using Contacts.Api.Domain;

namespace Contacts.Api.Infrastructure.Repositories;

public interface IContactsRepository
{
    Task<IEnumerable<Contact>> GetContactsAsync(string? search);
    Task<Contact?> GetContactAsync(int id);
    Task CreateContactAsync(Contact contact);
    Task<bool> UpdateContactAsync(Contact contact);
    Task<bool> DeleteContactAsync(int id);
}