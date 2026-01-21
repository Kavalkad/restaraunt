using restaraunt.Core.Entities;


namespace restaraunt.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(UserEntity user);
    }
}