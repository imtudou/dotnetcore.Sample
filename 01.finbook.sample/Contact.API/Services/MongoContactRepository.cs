using Contact.API.Data;
using Contact.API.Entity.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Threading;
using Contact.API.Entity.Models;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

namespace Contact.API.Services
{
    public class MongoContactRepository : IContactRepository
    {

        private readonly ContactContext _contactContext;
        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }


        /// <summary>
        /// 更新联系人
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContactInfoAsync(UserInfoDto user)
        {
            var contackbook = (await _contactContext.ContactBooks.FindAsync(s => s.UserId.Equals(user.Id))).FirstOrDefault();

            if (contackbook == null) return true;

            var contactIds = contackbook.Contacts.Select(s => s.UserId);

            var filter = Builders<ContactBook>.Filter.And(
                    Builders<ContactBook>.Filter.In(s => s.UserId, contactIds),
                    Builders<ContactBook>.Filter.ElemMatch(s => s.Contacts, c => c.UserId == user.Id)
                );

            var update = Builders<ContactBook>.Update
                .Set(s => s.Contacts[-1].Name, user.Name)
                .Set(s => s.Contacts[-1].Avatar, user.Avatar)
                .Set(s => s.Contacts[-1].Company, user.Company)
                .Set(s => s.Contacts[-1].Title, user.Title);

            var result = await _contactContext.ContactBooks.UpdateManyAsync(filter, update);
            return result.MatchedCount == result.ModifiedCount;
        }


        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AddContactInfoAsync(int userid, UserInfoDto user)
        {
            if ((await _contactContext.ContactBooks.CountDocumentsAsync(s => s.UserId.Equals(userid))) == 0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook { UserId = userid});
            }

            var filter = Builders<ContactBook>.Filter.Where(s => s.UserId.Equals(userid));
            var update = Builders<ContactBook>.Update.Set(s => s.UserId, userid)
                .AddToSet(s => s.Contacts, new AppContact
                {
                    UserId = user.Id,
                    Avatar = user.Avatar,
                    Company = user.Company,
                    Name = user.Name,
                    Title = user.Title
                });

            var result =  await _contactContext.ContactBooks.UpdateOneAsync(filter, update);
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
                
        }

        /// <summary>
        /// 获取所以联系人信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppContact>> GetContactsAsync(int userid)
        {
            var contact = (await _contactContext.ContactBooks.FindAsync(s => s.UserId.Equals(userid))).FirstOrDefault();
            if (contact == null)
                return new List<AppContact>();

            return contact?.Contacts;
        }


        /// <summary>
        /// 给联系人打标签
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="contactid"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<bool> TagContactsAsync(int userid, int contactid, List<string> tags)
        {
            var filter = Builders<ContactBook>.Filter.And(
                    Builders<ContactBook>.Filter.Eq(s => s.UserId, userid),
                    Builders<ContactBook>.Filter.Eq(s => s.Contacts[-1].UserId,contactid)
                );

            var update = Builders<ContactBook>.Update.Set(s => s.Contacts[-1].Tags, tags);

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update);

            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }
    }
}
