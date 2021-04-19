using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities
{
    
    public class UserService :IUserService // within "User" aggregate root
    {
        public UserService() { }

        public bool UserExists(string idToCheck)
        {
            return (idToCheck.Length > 1 && idToCheck.Length < 50);
        }
    }
}
