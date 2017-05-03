using Basbakanlik.Strateji.Entities;
using Basbakanlik.Strateji.Entities.Enums;
using Basbakanlik.Strateji.Entities.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Basbakanlik.Strateji.Provider.Repository
{
    public class UserRepository : RepositoryBase
    {
        public async Task<ApiResult> AddAsync(User user)
        {
            user.Created = DateTime.Now;
            user.Modified = DateTime.Now;
            await Context.Users.InsertOneAsync(user);
            return new ApiResult();
        }

        public async Task<ApiResult<User>> GetByIdAsync(string id)
        {
            var user = (await Context.Users.FindAsync(new ExpressionFilterDefinition<User>(i => i.ID == id))).FirstOrDefault();
            if (user == null)
                return new ApiResult<User>(null, "Kullanıcı Bulunamadı", ApiState.Error);
            return new ApiResult<User>(user);
        }

        public async Task<ApiResult<List<User>>> GetActiveUsersAsync()
        {
            var users = (await Context.Users.FindAsync(new ExpressionFilterDefinition<User>(i => i.IsActive))).ToList();
            return new ApiResult<List<User>>(users);
        }
    }
}
