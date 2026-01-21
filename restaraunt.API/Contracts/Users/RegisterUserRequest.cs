using System.ComponentModel.DataAnnotations;
namespace restaraunt.API.Contracts
{
    public record class RegisterUserRequest(
        [Required] string UserName,
        [Required] string Email,
        [Required] string Password
    );
}