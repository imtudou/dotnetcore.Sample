using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Contact.API.Services;

using Microsoft.AspNetCore.Mvc;
using Contact.API.Entity.Models;
using Microsoft.AspNetCore.Routing.Tree;
using Contact.API.Entity.Dtos;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contact.API.Controllers
{
    [Authorize(Policy = "Contact_API_Policy")]
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : BaseController
    {
        private readonly IContactApplyRequestRepository _contactApplyRequestRepository;
        private readonly IUserService _userService;
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository,
            IUserService userService,
            IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;

        }



        /// <summary>
        /// 添加好友申请请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequests([FromForm]int userid)
        {
            //var userinfo = await _userService.GetUserInfoAsync(userid);
            //if (userinfo == null)
            //    throw new Exception("用户参数为空！");

            var result = await _contactApplyRequestRepository.AddApplyRequestAsync(new ContactApplyRequest
            {
                ApplierId = UserIdentity.UserId ,
                Nmae = UserIdentity.Name,
                Company = UserIdentity.Company,
                UserId = userid,
                ApplyTime = DateTime.Now,
                Title = UserIdentity.Title,
                Avatar = UserIdentity.Avatar
            });

            if (!result)
            {
                return NotFound(result);
            }

            return Json(result);
        }


        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-request")]
        
        public async Task<IActionResult> ApproveApplyRequests([FromForm]int applierId)
        {
            //TAPD:
            //mongo 未引入事物

            var result = await _contactApplyRequestRepository.ApprovalRqeuestAsync(UserIdentity.UserId, applierId);
            if (!result)
            {
                return NotFound(result);
            }

            var applyInfo = await _userService.GetUserInfoAsync(applierId);
            var userinfo = await _userService.GetUserInfoAsync(UserIdentity.UserId);

            await _contactRepository.AddContactInfoAsync(UserIdentity.UserId, applyInfo);
            await _contactRepository.AddContactInfoAsync(applierId, userinfo);
            

            return Json("success!");
        }


        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> GetApplyRequestList()
        {
            var result = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId);
            return Json(result);
        }


        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        //[Authorize(Policy = "Contact_API_Policy")]
        public async Task<IActionResult> Get()
        {
            var result = await _contactRepository.GetContactsAsync(UserIdentity.UserId);
            if (result == null)
                result = new List<AppContact>();
            return Json(result);
        }


        /// <summary>
        /// 给联系人打标签
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="contactid"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> TagContacts([FromBody]TagContactInputViewModel viewModel)
        {
            return Json(await _contactRepository.TagContactsAsync(viewModel.userid, viewModel.contactid, viewModel.tags));
        }
            
            

       
    }
}
