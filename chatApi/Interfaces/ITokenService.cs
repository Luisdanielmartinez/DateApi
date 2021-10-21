using chatApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
