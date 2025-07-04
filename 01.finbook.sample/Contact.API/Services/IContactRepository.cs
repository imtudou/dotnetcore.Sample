using Contact.API.Entity.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Threading;
using Contact.API.Entity.Models;

namespace Contact.API.Services
{
    /// <summary>
    /// 通讯录相关Repository
    /// </summary>
    public interface IContactRepository
    {

        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfoAsync(UserInfoDto user);


        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddContactInfoAsync(int userid, UserInfoDto user);

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        Task<List<AppContact>> GetContactsAsync(int userid);

        /// <summary>
        /// 给联系人打标签
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="contactid"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagContactsAsync(int userid,int contactid,List<string> tags);


    }
}
