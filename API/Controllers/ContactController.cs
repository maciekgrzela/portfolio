using System;
using System.Threading.Tasks;
using Application.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ContactController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var contacts = await Mediator.Send(new GetAll.Query());
            return HandleResult(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var contact = await Mediator.Send(new Get.Query { Id = id });
            return HandleResult(contact);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Create.Command command)
        {
            var contactCreated = await Mediator.Send(command);
            return HandleResult(contactCreated);
        }
    }
}