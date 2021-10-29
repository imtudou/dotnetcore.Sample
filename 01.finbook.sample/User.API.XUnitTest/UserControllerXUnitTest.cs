using FluentAssertions;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;

using User.API.Controllers;
using User.API.Data;
using User.API.Entity.Models;

using Xunit;

namespace User.API.XUnitTest
{
    public class UserControllerXUnitTest
    {

        private UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var userContext = new UserContext(options);

            userContext.Users.Add(new AppUser
            {
                Id = 1,
                Name = "Yuan"
            });

            userContext.SaveChanges();
            return userContext;
        }


        private Tuple<UserController, UserContext> GetUserController()
        {

            var context = this.GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            return new Tuple<UserController, UserContext>(new UserController(context, logger), context);

        }


        [Fact]
        public async void Get_ReturnRightUser_WithExpctedParameters()
        {

            var controller = this.GetUserController().Item1;
            var response = await controller.Get();
            //Assert.IsType<JsonResult>(response);

            //fluentassertions用法推荐：https://fluentassertions.com/introduction
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appuser.Id.Should().Be(1);
            appuser.Name.Should().Be("Yuan");

        }


        [Fact]
        public async void Patch_ReturnNewName_WithExpctedNewNameParameters()
        {
            var allController_Context = this.GetUserController();
            var controller = allController_Context.Item1;
            var userContext = allController_Context.Item2;

            var patchDocument = new JsonPatchDocument<AppUser>();
            patchDocument.Replace(s => s.Name, "lei");
            var response = await controller.Patch(patchDocument);

            //assert response type
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert response 
            var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appuser.Name.Should().NotBeNullOrEmpty();
            appuser.Name.Should().Be("lei");

            //assert ef response
            var appuser_ef = await userContext.Users.FirstOrDefaultAsync(s => s.Id == 1);
            appuser_ef.Name.Should().NotBeNullOrEmpty();
            appuser_ef.Name.Should().Be("lei");

        }


        [Fact]
        public async void Patch_ReturnNewProperties_WithAddNewProperties()
        {
            var allController = this.GetUserController();
            var controller = allController.Item1;
            var userContext = allController.Item2;

            var patchDocument = new JsonPatchDocument<AppUser>();
            patchDocument.Replace(s => s.Properies, new List<UserPropery>{
                new UserPropery{ Key = "fin_industry", Text = "互联网",Value = "互联网" }
            });

            var response = await controller.Patch(patchDocument);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properies.Count.Should().Be(1);
            appUser.Properies.First().Key.Should().Be("fin_industry");
            appUser.Properies.First().Value.Should().Be("互联网");

            var appuser_ef = await userContext.Users.FirstOrDefaultAsync(s => s.Id == 1);
            appuser_ef.Properies.Count.Should().Be(1);
            appuser_ef.Properies.First().Key.Should().Be("fin_industry");
            appuser_ef.Properies.First().Value.Should().Be("互联网");

        }


        [Fact]
        public async void Patch_ReturnNewProperties_WithRemoveProperties()
        {
            var allController = this.GetUserController();
            var controller = allController.Item1;
            var userContext = allController.Item2;

            var patchDocument = new JsonPatchDocument<AppUser>();
            patchDocument.Replace(s => s.Properies, new List<UserPropery>() {
                new UserPropery{ Key = "", Text = "",Value = "" }
            });

            var response = await controller.Patch(patchDocument);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appuser.Properies.Should().NotHaveCount(0);

        }

    }
}
