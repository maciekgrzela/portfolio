using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Application.User.Resources;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class Register
    {
        public class Query : IRequest<Response<UserResource>>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(p => p.Email).EmailAddress().NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
            }
        }
        
        public class QueryHandler : IRequestHandler<Query, Response<UserResource>>
        {
            private readonly UserManager<Domain.Models.User> _manager;
            private readonly SignInManager<Domain.Models.User> _signInManager;
            private readonly IMapper _mapper;
            private readonly IWebTokenGenerator _webTokenGenerator;

            public QueryHandler(UserManager<Domain.Models.User> manager, SignInManager<Domain.Models.User> signInManager, IMapper mapper, IWebTokenGenerator webTokenGenerator)
            {
                _manager = manager;
                _signInManager = signInManager;
                _mapper = mapper;
                _webTokenGenerator = webTokenGenerator;
            }
            
            public async Task<Response<UserResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existingUser = await _manager.Users.CountAsync(cancellationToken: cancellationToken);

                if (existingUser > 0)
                {
                    return Response<UserResource>.Failure(ResponseResult.UserIsNotAuthorized,"Użytkownik jest już zarejestrowany");
                }

                var user = new Domain.Models.User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Abilities = new List<Ability>(),
                    Achievements = new List<Achievement>(),
                    Projects = new List<Project>(),
                    WorkExperiences = new List<WorkExperience>(),
                    SelfDescription = "",
                    LookingForAJob = true
                };

                var result = await _manager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return Response<UserResource>.Failure(ResponseResult.UserIsNotAuthorized, "Podczas rejestracji użytkownika wystąpił błąd");
                }

                var resource = _mapper.Map<Domain.Models.User, UserResource>(user);

                resource.Token = _webTokenGenerator.CreateToken(user);

                return Response<UserResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}