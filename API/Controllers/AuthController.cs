﻿using System.Threading.Tasks;
using Application.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login.Query query)
        {
            var userLogged = await Mediator.Send(query);
            return HandleResult(userLogged);
        }
        
        [HttpGet("login/current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var currentUser = await Mediator.Send(new GetCurrentLogged.Query());
            return HandleResult(currentUser);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin(Register.Query query)
        {
            var registeredUser = await Mediator.Send(query);
            return HandleResult(registeredUser);
        }
        
        [HttpGet("introduce")]
        public async Task<IActionResult> IntroduceAsync()
        {
            var owner = await Mediator.Send(new Introduce.Query());
            return HandleResult(owner);
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Update.Command command)
        {
            var owner = await Mediator.Send(command);
            return HandleResult(owner);
        }
        
    }
}