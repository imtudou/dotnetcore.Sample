using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Org.BouncyCastle.Utilities.IO;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

using User.API.Data;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using User.API.Entity.Models;
using Newtonsoft.Json;
using User.API.Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    
    public class UserController : BaseController
    {
        private UserContext _userContext;
        private ILogger<UserController> _logger;

        public UserController(UserContext userContext,ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [Authorize(Policy = "User_API_Policy")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var user = await  _userContext.Users
                .AsNoTracking()
                .Include(u => u.Properies)
                .SingleOrDefaultAsync(s => s.Id == UserIdentity.UserId);

            if (user == null)
            {
                throw new UserOperationException($"错误的上下文ID:{UserIdentity.UserId}");
            }
            return Json(user);

        }



        /// <summary>
        /// Method:Patch()
        /// </summary>
        /// <param name="jsonPatch">type:JsonPatchDocument</param>
        /// <returns>returtype:JsonPatchDocument</returns>
        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<AppUser> jsonPatch)
        {
            if (jsonPatch != null)
            {
                var user = await _userContext.Users.FirstOrDefaultAsync(s => s.Id == UserIdentity.UserId);
                jsonPatch.ApplyTo(user);

                var originProperties = await _userContext.UserProperies
                    .Where(s => s.AppUserId == UserIdentity.UserId)
                    .ToListAsync();

                var allProperties = originProperties.Union(user?.Properies).Distinct();
                var removeProperties = originProperties.Except(user?.Properies);
                var newProperties = allProperties.Except(user?.Properies);

                //Excep用法：https://docs.microsoft.com/zh-cn/dotnet/api/system.linq.enumerable.except?view=netcore-3.1

                foreach (var item in removeProperties)
                {
                    _userContext.Remove(item);
                }

                foreach (var item in newProperties)
                {
                    _userContext.Add(item);
                }

                _userContext.Users.Update(user);
                _userContext.SaveChanges();
                return Json(user);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        /// <summary>
        /// 当手机号不存在则创建用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [Route("CheckOrCreate")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate([FromForm] string phone)
        {

            //1.手机号码格式验证
            var userIsExist = _userContext.Users.SingleOrDefault(s => s.Phone == phone);
            if (userIsExist == null)
            {
                userIsExist = new AppUser();
                _userContext.Users.Add(new AppUser { Phone = phone });
                await _userContext.SaveChangesAsync();
                userIsExist = _userContext.Users.AsNoTracking().SingleOrDefault(s => s.Phone == phone);
            }
            return Ok(userIsExist);
            //return BadRequest("测试 error！");//测试Poll

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[Route("CheckOrCreate")]
        //[HttpPost]
        //public async Task<IActionResult> CheckOrCreate([FromBody]CheckOrCreateAppUserViewModel model)
        //{
        //    //1.手机号码格式验证
        //    var userIsExist = await _userContext.Users.SingleOrDefaultAsync(s => s.Phone == model.Phone);
        //    if (userIsExist == null)
        //    {
        //        userIsExist = new AppUser();
        //        _userContext.Users.Add(new AppUser { Phone = model.Phone, Name = model.Name, Company = model.Company });
        //        await _userContext.SaveChangesAsync();

        //        userIsExist.Id = _userContext.Users.AsNoTracking().SingleOrDefault(s => s.Phone == model.Phone).Id;
        //    }
        //    return Ok(userIsExist?.Id);
        //}


        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            var usertags = await _userContext.UserTags.Where(s => s.UserId.Equals(UserIdentity.UserId)).ToListAsync();
            return Json(usertags);
        }


        [HttpPost]
        [Route("userinfo")]
        public async Task<IActionResult> GetUserInfo([FromForm]string userId)
        {
            if (!int.TryParse(userId,out int intUserId))
                throw new ArgumentException("userId");

            var userinfo = await _userContext.Users.Include(s=>s.Properies).SingleOrDefaultAsync(s => s.Id.Equals(intUserId));
            return Json(userinfo);
        }

        
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> UpdateUserTags([FromBody] List<string> tags)
        {
            var currentTags = await _userContext.UserTags.Where(s => s.UserId.Equals(UserIdentity.UserId)).ToListAsync();
            var newTags = tags.Except(currentTags.Select(s => s.Tag));
            await _userContext.AddRangeAsync(newTags.Select(s => new UserTag
            {
                CreateTime = DateTime.Now,
                UserId = UserIdentity.UserId,
                Tag = s
            }));
            await _userContext.SaveChangesAsync();
            return Json("success! updatedate:"+DateTime.Now);
        }



        



    }
}
