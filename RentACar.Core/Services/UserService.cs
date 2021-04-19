using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Services
{
    
    public class UserService :IUserService // within "User" aggregate root
    {
        public UserService() { }

        public bool UserExists(string id)
        {
            return (id.Length > 1 && id.Length < 50);
        }
    }
}
