using Contacts.Api.Domain;

namespace Contacts.Api.Infrastructure.Repositories;

public interface IContactsRepository
{
    Task<(IEnumerable<Contact>, PaginationMetadata)> GetContactsAsync(string? lastName, string? search, string? orderBy, bool? desc, int pageNumber, int pageSize);
    Task<Contact?> GetContactAsync(int id);
    Task CreateContactAsync(Contact contact);
    Task<bool> UpdateContactAsync(Contact contact);
    Task<bool> DeleteContactAsync(int id);
}