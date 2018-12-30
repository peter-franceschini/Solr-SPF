using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Database;

namespace Website.Services
{
    public interface IUserService
    {
        AspNetUsers GetUser(string userName);
    }
}
