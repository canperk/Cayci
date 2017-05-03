using Basbakanlik.Strateji.Entities.Models;
using System.Threading.Tasks;
using Basbakanlik.Strateji.Entities;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using System;

namespace Basbakanlik.Strateji.Provider.Repository
{
    public class RequestRepository : RepositoryBase
    {
        public async Task AddAsync(UserRequest request)
        {
            request.Created = DateTime.Now;
            request.Modified = DateTime.Now;
            request.Checked = false;
            await Context.UserRequests.InsertOneAsync(request);
        }

        public async Task AddTypeAsync(RequestType type)
        {
            type.IsActive = true;
            type.Created = DateTime.Now;
            type.Modified = DateTime.Now;
            await Context.RequestTypes.InsertOneAsync(type);
        }

        public async Task<ApiResult<int>> GetWaitingRequestCountAsync()
        {
            var result = (await Context.UserRequests.FindAsync(new ExpressionFilterDefinition<UserRequest>(i => !i.Checked))).ToList().Count();
            return new ApiResult<int>(result);
        }

        public async Task<ApiResult<List<RequestType>>> GetTypesAsync()
        {
            var sort = new FindOptions<RequestType> { Sort = Builders<RequestType>.Sort.Ascending("ListOrder") };
            var result = (await Context.RequestTypes.FindAsync(new ExpressionFilterDefinition<RequestType>(i => i.IsActive), sort)).ToList();
            return new ApiResult<List<RequestType>>(result);
        }

        public async Task<ApiResult<List<UserRequest>>> GetWaitingRequests()
        {
            var sort = new FindOptions<UserRequest> { Sort = Builders<UserRequest>.Sort.Descending("Created") };
            var result = (await Context.UserRequests.FindAsync(new ExpressionFilterDefinition<UserRequest>(i => !i.Checked), sort)).ToList();
            foreach (var item in result)
            {
                if (!item.Seen)
                {
                    item.Seen = true;
                    Context.UserRequests.ReplaceOne(i => i.ID == item.ID, item);
                }
            }
            return new ApiResult<List<UserRequest>>(result);

        }

        public async Task<ApiResult<List<UserRequest>>> GetUserRequests(string username)
        {
            var sort = new FindOptions<UserRequest>
            {
                Sort = Builders<UserRequest>.Sort.Descending("Created"),
                Limit = 10
            };

            var result = (await Context.UserRequests.FindAsync(new ExpressionFilterDefinition<UserRequest>(i => i.UserId == username), sort)).ToList();
            return new ApiResult<List<UserRequest>>(result);
        }

        public async Task<ApiResult<List<Location>>> GetLocations()
        {
            var result = (await Context.Locations.FindAsync(i => true)).ToList();
            return new ApiResult<List<Location>>(result);
        }

        public async Task<ApiResult<UserRequest>> GetDetailById(string id)
        {
            var result = (await Context.UserRequests.FindAsync(new ExpressionFilterDefinition<UserRequest>(i => i.ID == id))).FirstOrDefault();
            if (!result.Checked)
            {
                result.Checked = true;
                Context.UserRequests.ReplaceOne(i => i.ID == result.ID, result);
            }
            return new ApiResult<UserRequest>(result);
        }
    }
}
