using Cayci.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cayci.Entities.Contracts
{
    [ApiRoute(Path = "api/User")]
    public interface IUserContract : IContractBase
    {
        [ApiRoute(Path = "Get", Type = MethodType.Get)]
        Task<ApiResult<User>> GetUserById(string userId);
        [ApiRoute(Path = "GetAll", Type = MethodType.Get)]
        Task<ApiResult<List<User>>> GetAllUsers();
    }
}
