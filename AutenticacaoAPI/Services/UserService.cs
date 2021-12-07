using AutenticacaoAPI.DTOs;
using AutenticacaoAPI.Models;
using AutenticacaoAPI.Repositories;

namespace AutenticacaoAPI.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly TokenService _tokenService;

        public UserService(UserRepository repository, TokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public User Create(User user)
        {
            return _repository.Create(user);
        }

        public LoginResultDTO Login(string username, string password)
        {
            var loginResult = _repository.Login(username, password);

            if (loginResult.Error)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Ocorreu um erro ao autenticar: {loginResult.Exception?.Message}" }
                };
            }

            var token = _tokenService.GenerateToken(loginResult.User);

            return new LoginResultDTO
            {
                Success = true,
                User = new UserLoginResultDTO
                {
                    Token = token,
                    Id = loginResult.User.Id,
                    Role = loginResult.User.Role,
                    Username = loginResult.User.Username
                }
            };
        }
    }
}
