using Contact.API.Data;
using Contact.API.Entity.Models;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Services
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {

        private readonly ContactContext _contactContext;
        public MongoContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        
        /// <summary>
        /// 用户好友申请
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public  async Task<bool> AddApplyRequestAsync(ContactApplyRequest request)
        {
            //1.先查询是否存在 存在即做uodate操作
            //2.新增

            var filter = Builders<ContactApplyRequest>.Filter.Where(s => s.UserId.Equals(request.UserId) && s.ApplierId.Equals(request.ApplierId));
            if ((await _contactContext.ContactApplyRequests.CountDocumentsAsync(filter)) > 0)
            {
                var update = Builders<ContactApplyRequest>.Update.Set(s => s.ApplyTime, DateTime.Now);
                var result = await _contactContext.ContactApplyRequests.UpdateManyAsync(filter,update);
                return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
            }

            await _contactContext.ContactApplyRequests.InsertOneAsync(request);
            return true;
        }

        
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="ApplierId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ApprovalRqeuestAsync(int userid,int ApplierId)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(s => s.UserId.Equals(userid) && s.ApplierId.Equals(ApplierId));
            var update = Builders<ContactApplyRequest>.Update.Set(s => s.ApplyTime, DateTime.Now);
            var result = await _contactContext.ContactApplyRequests.UpdateManyAsync(filter, update);
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }   

        
        /// <summary>
        /// 获取通讯录列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userid)
        {
            return (await _contactContext.ContactApplyRequests.FindAsync(s => s.ApplierId.Equals(userid)&& s.Approvaled==0)).ToList();   
        }
    }
}
