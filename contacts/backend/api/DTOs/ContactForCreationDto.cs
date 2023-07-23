using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record ContactForCreationDto(string FirstName, string LastName, string Email);
