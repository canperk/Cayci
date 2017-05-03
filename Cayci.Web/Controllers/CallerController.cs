using Cayci.Helpers;
using Cayci.Entities;
using Cayci.Entities.Contracts;
using Cayci.Entities.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cayci.Controllers
{
    public class CallerController : BaseController
    {
        [HttpGet]
        public JsonResult GetTypes()
        {
            var data = Call((IRequestContract c) => c.GetTypes());
            return ToJson(data);
        }

        [HttpPost]
        public JsonResult AddNewRequest(UserRequest request)
        {
            request.UserId = SessionHelper.DisplayName;
            var data = Call((IRequestContract c) => c.NewRequest(request));
            HubContext.Clients.All.addNewRequest();
            return ToJson(data);
        }

        [HttpGet]
        public JsonResult GetRequestCount()
        {
            var count = Call((IRequestContract c) => c.GetWaitingRequestCountAsync());
            return ToJson(count);
        }

        [HttpGet]
        public JsonResult GetWaitingRequests()
        {
            var result = Call((IRequestContract c) => c.GetWaitingRequestsAsync());
            var items = result as ApiResult<List<UserRequest>>;
            items.Result.ForEach(i => {
                HubContext.Clients.All.requestSeen(i.ID);
            });
            return ToJson(result);
        }

        [HttpGet]
        public JsonResult GetRequestDetail(string id)
        {
            var result = Call((IRequestContract c) => c.GetRequestDetailAsync(id));
            return ToJson(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAllUsers()
        {
            var result = Call((IUserContract c) => c.GetAllUsers());
            return ToJson(result);
        }

        [HttpGet]
        public JsonResult GetUserRequests()
        {
            var result = Call((IRequestContract c) => c.GetUserRequestsAsync(SessionHelper.UserId));
            return ToJson(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetLocations()
        {
            var result = Call((IRequestContract c) => c.GetLocationsAsync());
            return ToJson(result);
        }
    }
}