namespace Contacts.Api.DTOs;

public record ContactDto(int Id, string FirstName, string LastName, string Email)
{
    public string FullName => $"{FirstName} {LastName}";
}