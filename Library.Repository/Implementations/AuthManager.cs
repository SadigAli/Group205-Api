using Library.Data.DTOs.Auth;
using Library.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Implementations
{
    public class AuthManager : IAuthManager
    {
        public Task Login(LoginDTO login)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task Register(RegisterDTO register)
        {
            throw new NotImplementedException();
        }
    }
}
