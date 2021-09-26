using System;
using System.Threading.Tasks;
using Application.WorkExperiences;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class WorkExperiencesController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var workExperiences = await Mediator.Send(new GetAll.Query());
            return HandleResult(workExperiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var workExperience = await Mediator.Send(new Get.Query());
            return HandleResult(workExperience);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var workExperienceCreated = await Mediator.Send(command);
            return HandleResult(workExperienceCreated);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Update.Command command)
        {
            command.Id = id;
            var workExperienceUpdated = await Mediator.Send(command);
            return HandleResult(workExperienceUpdated);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var workExperienceDeleted = await Mediator.Send(new Delete.Command {Id = id});
            return HandleResult(workExperienceDeleted);
        }
    }
}