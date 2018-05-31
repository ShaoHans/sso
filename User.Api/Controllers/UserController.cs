using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Api.Models;

namespace User.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly List<UserInfo> _users;

        public UserController()
        {
            _users = new List<UserInfo>
            {
                new UserInfo
                {
                    Id = 1,
                    Name = "tom",
                    RegisterDate = DateTime.Now.AddYears(-1)
                },

                new UserInfo
                {
                    Id = 2,
                    Name = "jim",
                    RegisterDate = DateTime.Now.AddDays(-19)
                }
            };
        }

        [HttpGet]
        [Route("claims")]
        public ActionResult Claims()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserInfo> Get(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound("不存在的用户");
            }
            return user;
        }
    }
}
