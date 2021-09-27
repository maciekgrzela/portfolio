using System;
using System.Threading.Tasks;
using Application.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var projects = await Mediator.Send(new GetAll.Query());
            return HandleResult(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var project = await Mediator.Send(new Get.Query { Id = id });
            return HandleResult(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var projectCreated = await Mediator.Send(command);
            return HandleResult(projectCreated);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Update.Command command)
        {
            command.Id = id;
            var projectUpdated = await Mediator.Send(command);
            return HandleResult(projectUpdated);
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImageAsync(Guid id, IFormFile file)
        {
            var imageUploaded = await Mediator.Send(new UploadImage.Command {Id = id, File = file});
            return HandleResult(imageUploaded);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var projectDeleted = await Mediator.Send(new Delete.Command { Id = id });
            return HandleResult(projectDeleted);
        }

        [HttpDelete("image/{id}")]
        public async Task<IActionResult> RemoveImageAsync(Guid id)
        {
            var imageDeleted = await Mediator.Send(new RemoveImage.Command { Id = id });
            return HandleResult(imageDeleted);
        }
    }
}