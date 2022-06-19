using System.Threading.Tasks;
using Api.Controllers;
using ApiTests.TestObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.DTOs.Persons;
using Services.DTOs.Users;

namespace ApiTests.UnitTests
{
    [TestClass]
    public class UserTests
    {
        private UserController _controller = ControllerFactory.CreateUserController();

        [TestMethod]
        public async Task CreateUserTestOk()
        {
            CreateUpdateUserDto user = new CreateUpdateUserDto();
            CreateUpdatePersonDto person = new CreateUpdatePersonDto();
            user.Email = "x@x";

            var response = await _controller.CreateRootUser(user);
            var result = response.Result as OkObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        public async Task RegisterError()
        {
            CreateUpdateUserDto user = new CreateUpdateUserDto();
            CreateUpdatePersonDto person = new CreateUpdatePersonDto();
            user.Email = "x@x";

            var response = await _controller.CreateRootUser(user);
            var result = response.Result as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        public async Task RegisterEmpty()
        {
            CreateUpdateUserDto user = new CreateUpdateUserDto();
            CreateUpdatePersonDto person = new CreateUpdatePersonDto();
            user.Email = "x@x";

            var response = await _controller.CreateRootUser(user);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
        }
    }
}