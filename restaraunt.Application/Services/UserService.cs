using restaraunt.Application.Interfaces.Repositories;
using restaraunt.Application.Interfaces.Auth;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.WebSockets;
using restaraunt.Core.Entities;


namespace restaraunt.Application.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        
        public UserService(
            IPasswordHasher passwordHasher,
            IUsersRepository usersRepository,
            IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }
        public async Task Register(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);
            var user = new UserEntity() { UserName = userName, Email = email, HashedPassword = hashedPassword, Id = 99 };
            await _usersRepository.AddAsync(user);

        }
        public async Task<string> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);

            var result = _passwordHasher.Verify(password, user.HashedPassword);
            if (!result)
            {
                throw new Exception("Failed to Login");
            }
            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
    }
}