using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Interfaces
{
    public interface IUserService
    {
        bool UserExists(string id);
    }
}
