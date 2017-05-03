using Cayci.Entities;
using Cayci.Entities.Contracts;
using Cayci.Entities.Models;
using Cayci.Provider.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cayci.Api.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController, IUserContract
    {
        private readonly UserRepository _userRepo;
        public UserController()
        {
            _userRepo = new UserRepository();
        }
        [HttpPost]
        [Route("New")]
        public async Task<ApiResult> NewUser([FromBody]User user)
        {
            return await _userRepo.AddAsync(user);
        }
        [HttpGet]
        [Route("Get")]
        public async Task<ApiResult<User>> GetUserById(string userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResult<List<User>>> GetAllUsers()
        {
            return await _userRepo.GetActiveUsersAsync();
        }
    }
}