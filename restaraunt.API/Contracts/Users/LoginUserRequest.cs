using System.ComponentModel.DataAnnotations;
namespace restaraunt.API.Contracts
{
    public record class LoginUserRequest(
        [Required] string Email,
        [Required] string Password
    );
}