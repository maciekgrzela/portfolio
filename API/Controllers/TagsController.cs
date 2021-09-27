using System;
using System.Threading.Tasks;
using Application.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class TagsController : BaseController
    {
        [HttpPost("assign/{tagId}/to/{projectId}")]
        public async Task<IActionResult> AssignTagToProjectAsync(Guid tagId, Guid projectId)
        {
            var tagAssigned = await Mediator.Send(new AssignToProject.Command {TagId = tagId, ProjectId = projectId});
            return HandleResult(tagAssigned);
        }
        
        [HttpPost("remove/{tagId}/from/{projectId}")]
        public async Task<IActionResult> RemoveTagFromProjectAsync(Guid tagId, Guid projectId)
        {
            var tagRemoved = await Mediator.Send(new RemoveFromProject.Command {TagId = tagId, ProjectId = projectId});
            return HandleResult(tagRemoved);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var tagCreated = await Mediator.Send(command);
            return HandleResult(tagCreated);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var tagDeleted = await Mediator.Send(new Delete.Command { Id = id });
            return HandleResult(tagDeleted);
        }
    }
}