using Cayci.Entities;
using Cayci.Entities.Models;
using Cayci.Provider.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cayci.Api.Controllers
{
    [RoutePrefix("api/Request")]
    public class RequestController : ApiController, IRequestContract
    {
        private readonly RequestRepository _requestRepo;

        public RequestController()
        {
            _requestRepo = new RequestRepository();
        }
        [HttpPost]
        [Route("New")]
        public async Task<ApiResult> NewRequest([FromBody]UserRequest request)
        {
            await _requestRepo.AddAsync(request);
            return new ApiResult();
        }

        [HttpPost]
        [Route("NewType")]
        public async Task<ApiResult> NewType([FromBody]RequestType type)
        {
            await _requestRepo.AddTypeAsync(type);
            return new ApiResult();
        }

        [HttpGet]
        [Route("WaitingRequestsCount")]
        public async Task<ApiResult<int>> GetWaitingRequestCountAsync(string groupId)
        {
            return await _requestRepo.GetWaitingRequestCountAsync(groupId);
        }

        [HttpGet]
        [Route("Types")]
        public async Task<ApiResult<List<RequestType>>> GetTypes()
        {
            return await _requestRepo.GetTypesAsync();
        }
        [HttpGet]
        [Route("WaitingRequests")]
        public async Task<ApiResult<List<UserRequest>>> GetWaitingRequestsAsync(string groupId)
        {
            return await _requestRepo.GetWaitingRequests(groupId);
        }
        [HttpGet]
        [Route("RequestDetail")]
        public async Task<ApiResult<UserRequest>> GetRequestDetailAsync(string id)
        {
            return await _requestRepo.GetDetailById(id);
        }
        [HttpGet]
        [Route("UserRequests")]
        public async Task<ApiResult<List<UserRequest>>> GetUserRequestsAsync(string userId)
        {
            var userRepo = new UserRepository();
            var user = (await userRepo.GetByIdAsync(userId)).Result;
            if (user == null)
                return new ApiResult<List<UserRequest>>(null, "Kullanıcı Bulunamadı", Entities.Enums.ApiState.Error);
            return (await _requestRepo.GetUserRequests(user.DisplayName));
        }
        [HttpGet]
        [Route("Locations")]
        public async Task<ApiResult<List<Location>>> GetLocationsAsync()
        {
            return (await _requestRepo.GetLocations());
        }
    }
}