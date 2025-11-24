using ChatApp.Models;
using ChatApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/group")]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyGroups()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var groups = await _groupService.GetGroupsForUser(username);
            return Ok(groups);
        }

        public class GroupCreateDto
        {
            public string Name { get; set; }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreateDto dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var group = new ChatGroup
            {
                Name = dto.Name,
                Admin = username,
                CreatedBy = username,
                Members = new List<string>()
            };

            await _groupService.CreateGroup(group);
            return Ok("Group created");
        }

        [HttpPost("addUser")]
        [Authorize]
        public async Task<IActionResult> AddUserToGroup([FromBody] AddUserDto dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var group = await _groupService.GetGroupByName(dto.GroupName);
            if (group == null)
                return NotFound("Group not found");

            if (group.Admin != username)
                return Unauthorized("Only admin can add users");

            if (!group.Members.Contains(dto.Username))
                group.Members.Add(dto.Username);

            await _groupService.UpdateGroup(group);

            return Ok("User added");
        }

        public class AddUserDto
        {
            public string GroupName { get; set; }
            public string Username { get; set; }
        }


        [HttpGet("{groupName}")]
        [Authorize]
        public async Task<IActionResult> GetGroupByName(string groupName)
        {
            var group = await _groupService.GetGroupByName(groupName);
            if (group == null)
                return NotFound("Group not found");

            return Ok(group);
        }
    }
}
