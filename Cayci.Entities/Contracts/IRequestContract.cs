using Cayci.Entities;
using Cayci.Entities.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cayci.Entities
{
    [ApiRoute(Path = "api/Request")]
    public interface IRequestContract : IContractBase
    {
        [ApiRoute(Path = "New", Type = MethodType.Post)]
        Task<ApiResult> NewRequest(UserRequest request);
        [ApiRoute(Path = "NewType", Type = MethodType.Post)]
        Task<ApiResult> NewType(RequestType type);
        [ApiRoute(Path = "WaitingRequestsCount", Type = MethodType.Get)]
        Task<ApiResult<int>> GetWaitingRequestCountAsync();
        [ApiRoute(Path = "WaitingRequests", Type = MethodType.Get)]
        Task<ApiResult<List<UserRequest>>> GetWaitingRequestsAsync();
        [ApiRoute(Path = "Types", Type = MethodType.Get)]
        Task<ApiResult<List<RequestType>>> GetTypes();
        [ApiRoute(Path = "RequestDetail", Type = MethodType.Get)]
        Task<ApiResult<UserRequest>> GetRequestDetailAsync(string id);
        [ApiRoute(Path = "UserRequests", Type = MethodType.Get)]
        Task<ApiResult<List<UserRequest>>> GetUserRequestsAsync(string userId);
        [ApiRoute(Path = "Locations", Type = MethodType.Get)]
        Task<ApiResult<List<Location>>> GetLocationsAsync();
    }
}
