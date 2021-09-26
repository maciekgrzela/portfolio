using System;
using System.Threading.Tasks;
using Application.Socials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SocialsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var socials = await Mediator.Send(new GetAll.Query());
            return HandleResult(socials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var social = await Mediator.Send(new Get.Query {Id = id});
            return HandleResult(social);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var socialCreated = await Mediator.Send(command);
            return HandleResult(socialCreated);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Update.Command command)
        {
            command.Id = id;
            var socialUpdated = await Mediator.Send(command);
            return HandleResult(socialUpdated);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var socialDeleted = await Mediator.Send(new Update.Command { Id = id });
            return HandleResult(socialDeleted);
        }
    }
}