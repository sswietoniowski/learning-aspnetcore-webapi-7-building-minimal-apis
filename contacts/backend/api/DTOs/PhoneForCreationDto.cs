using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record PhoneForCreationDto(string Number, string Description);
