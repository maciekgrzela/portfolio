using System;
using System.Threading.Tasks;
using Application.Abilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AbilitiesController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var abilities = await Mediator.Send(new GetAll.Query());
            return HandleResult(abilities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var ability = await Mediator.Send(new Get.Query { Id = id });
            return HandleResult(ability);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var abilityCreated = await Mediator.Send(command);
            return HandleResult(abilityCreated);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Update.Command command)
        {
            command.Id = id;
            var abilityUpdated = await Mediator.Send(command);
            return HandleResult(abilityUpdated);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var abilityDeleted = await Mediator.Send(new Delete.Command { Id = id });
            return HandleResult(abilityDeleted);
        }
    }
}