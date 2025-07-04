using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Entity.Models;
using MongoDB;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Contact.API.Entity.Dtos;
using Microsoft.AspNetCore.Authentication;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Options;

namespace Contact.API.Data
{
    /// <summary>
    /// 基于Mongo存储ContactContext
    /// </summary>
    public class ContactContext
    {
        private readonly IMongoDatabase _database;
        private readonly ContactDataBaseSettings _settings;

        public ContactContext(IOptions<ContactDataBaseSettings> settings)
        {
            _settings = settings.Value;

            var client = new MongoClient(_settings.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_settings.DatabaseName);
            }
        }



        private void CheckCreateCollection(string collectionName)
        {
            var collectionList = _database.ListCollections().ToList();
            var collectionNames = new List<string>();

            collectionList.ForEach(s => collectionNames.Add(s["name"].AsString));
            if (!collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        /// <summary>
        /// 联系人列表
        /// </summary>
        //public IMongoCollection<AppContact> AppContacts 
        //{
        //    get {
        //        CheckCreateCollection("AppContacts");
        //        return _database.GetCollection<AppContact>("AppContacts");
        //    }
        //}

        /// <summary>
        /// 好友申请请求记录
        /// </summary>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get {
                CheckCreateCollection("ContactApplyRequests");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequests");
            }
        }


        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks 
        {
            get {
                CheckCreateCollection("ContactBooks");
                return _database.GetCollection<ContactBook>("ContactBooks");
            }
        }

    }



}
