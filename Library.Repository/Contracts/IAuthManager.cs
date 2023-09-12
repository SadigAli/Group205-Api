using Library.Data.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Contracts
{
    public interface IAuthManager
    {
        public Task Login(LoginDTO login);
        public Task Register(RegisterDTO register);
        public Task Logout();
    }
}
