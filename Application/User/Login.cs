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
    public class Login
    {
        public class Query : IRequest<Response<UserResource>>
        {
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
                var user = await _manager.Users.OrderBy(p => p.FirstName).FirstOrDefaultAsync(p => p.Email == request.Email, cancellationToken);

                if (user == null)
                {
                    return Response<UserResource>.Failure(ResponseResult.ResourceDoesntExist,$"Użytkownik o adresie:{request.Email} nie został znaleziony");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                
                if (!result.Succeeded) 
                    return Response<UserResource>.Failure(ResponseResult.UserIsNotAuthorized, "Dane uwierzytelniające są nieprawidłowe");
            
            
                var loggedUser = new LoggedUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = _webTokenGenerator.CreateToken(user)
                };

                var resource = _mapper.Map<LoggedUser, UserResource>(loggedUser);

                return Response<UserResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}