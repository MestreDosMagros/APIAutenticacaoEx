using AutenticacaoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutenticacaoAPI.Repositories
{
    public class UserRepository
    {
        private const int FAILED_LOGIN_ATTEMPTS_LIMMIT = 3;

        private readonly IDictionary<Guid, User> _users;

        public UserRepository()
        {
            _users ??= new Dictionary<Guid, User>();
        }

        public LoginResult Login(string username, string password)
        {
            try
            {
                var user = _users.Values.Where(u => u.Username == username && u.Password == password).SingleOrDefault();

                if (user != null)
                {
                    if (user.IsLockout)
                    {
                        if (DateTime.Now <= user.LockoutDate?.AddMinutes(15))
                        {
                            return LoginResult.ErrorResult(InvalidPasswordException.ACCOUNT_LOCKED);
                        }
                        else
                        {
                            user.IsLockout = false;
                            user.FailedAttempts = 0;
                        }
                    }

                    return LoginResult.SuccessResult(user);
                }

                // aqui pode se fazer logica de bloqueio de conta...
                var userExistsForUsername = _users.Values.Where(u => u.Username == username).Any();

                if (userExistsForUsername)
                {
                    user = _users.Values.Where(u => u.Username == username).Single();

                    user.FailedAttempts++;

                    if (user.FailedAttempts >= FAILED_LOGIN_ATTEMPTS_LIMMIT)
                    {
                        user.IsLockout = true;
                        user.LockoutDate = DateTime.Now;

                        return LoginResult.ErrorResult(InvalidPasswordException.ACCOUNT_LOCKED);
                    }

                    return LoginResult.ErrorResult(InvalidPasswordException.INVALID_PASSWORD_EXCEPTION);
                }

                return LoginResult.ErrorResult(InvalidUsernameException.INVALID_USERNAME_EXCEPTION);
            }
            catch (Exception e)
            {
                return LoginResult.ErrorResult(new AuthenticationException(e));
            }
        }

        public User Get(Guid id)
        {
            if (_users.TryGetValue(id, out var user))
                return user;

            throw new Exception("Usuário não encontrado!");
        }

        public IEnumerable<User> Get()
        {
            return _users.Values;
        }

        public User Create(User user)
        {
            user.Id = Guid.NewGuid();

            if (_users.Values.Where(u => u.Username == user.Username).Any())
            {
                throw new Exception($"O nome de usuário já está sendo usado, tente outro!");
            }

            if (_users.TryAdd(user.Id, user))
                return Get(user.Id);

            throw new Exception($"Não foi possível cadastrar o usuário!");
        }

        public bool Remove(Guid id)
        {
            return _users.Remove(id);
        }

        public User Update(Guid id, User user)
        {
            if (_users.TryGetValue(id, out var userToUpdate))
            {
                userToUpdate.Role = user.Role;
                userToUpdate.Password = user.Password;
                userToUpdate.Username = user.Username;
            }

            throw new Exception("Usuário não encontrado!");
        }
    }
}
