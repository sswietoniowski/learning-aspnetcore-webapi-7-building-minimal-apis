namespace Contacts.Api.DTOs;

public record ContactDetailsDto(int Id, string FirstName, string LastName, string Email,
    List<PhoneDto> Phones)
{
    public string FullName => $"{FirstName} {LastName}";
}