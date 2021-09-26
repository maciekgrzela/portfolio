using System;
using System.Threading.Tasks;
using Application.Achievements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AchievementsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var achievements = await Mediator.Send(new GetAll.Query());
            return HandleResult(achievements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var achievement = await Mediator.Send(new Get.Query { Id = id });
            return HandleResult(achievement);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var achievementCreated = await Mediator.Send(command);
            return HandleResult(achievementCreated);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Update.Command command)
        {
            command.Id = id;
            var achievementUpdated = await Mediator.Send(command);
            return HandleResult(achievementUpdated);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var achievementDeleted = await Mediator.Send(new Delete.Command { Id = id });
            return HandleResult(achievementDeleted);
        }
    }
}