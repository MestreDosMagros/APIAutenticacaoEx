using AutenticacaoAPI.Models;
using System;

namespace AutenticacaoAPI
{
    public class LoginResult
    {
        public User User { get; set; }
        public bool Error { get; set; }
        public AuthenticationException Exception { get; set; }

        public static LoginResult SuccessResult(User user)
        {
            return new LoginResult
            {
                User = user,
                Error = false
            };
        }

        public static LoginResult ErrorResult(AuthenticationException exception)
        {
            return new LoginResult
            {
                User = null,
                Error = true,
                Exception = exception
            };
        }
    }

    public class AuthenticationException : Exception
    {
        private const string MESSAGE = "Não foi possível autenticar!";

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(Exception inner) : base(MESSAGE, inner)
        {
        }

        public AuthenticationException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidUsernameException : AuthenticationException
    {
        private const string MESSAGE = "Usuário informado inválido!";

        public static InvalidUsernameException INVALID_USERNAME_EXCEPTION = new(MESSAGE);

        public InvalidUsernameException(string message) : base(message)
        {
        }

        public InvalidUsernameException(Exception inner) : base(MESSAGE, inner)
        {
        }

        public InvalidUsernameException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidPasswordException : AuthenticationException
    {
        private const string MESSAGE = "Senha informada inválida!";
        private const string ACCOUNT_LOCKED_MESSAGE = "A conta foi bloqueada por exceder o limites de tentativas de login sem sucesso, espere alguns minutos e tente novamente, ou redefina sua senha!";

        public static InvalidPasswordException INVALID_PASSWORD_EXCEPTION = new(MESSAGE);
        public static InvalidPasswordException ACCOUNT_LOCKED = new(ACCOUNT_LOCKED_MESSAGE);

        public InvalidPasswordException(string message) : base(message)
        {
        }

        public InvalidPasswordException(Exception inner) : base(MESSAGE, inner)
        {
        }

        public InvalidPasswordException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
