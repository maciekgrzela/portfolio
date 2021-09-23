using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Application.User.Resources;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class GetCurrentLogged
    {
        public class Query : IRequest<Response<UserResource>> { }
        
        public class Handler : IRequestHandler<Query, Response<UserResource>>
        {
            private readonly UserManager<Domain.Models.User> _userManager;
            private readonly IUserAccessor _accessor;
            private readonly IMapper _mapper;
            private readonly IWebTokenGenerator _webTokenGenerator;

            public Handler(UserManager<Domain.Models.User> userManager, IUserAccessor accessor, IMapper mapper, IWebTokenGenerator webTokenGenerator)
            {
                _userManager = userManager;
                _accessor = accessor;
                _mapper = mapper;
                _webTokenGenerator = webTokenGenerator;
            }
            
            public async Task<Response<UserResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existingUser = await _userManager.FindByEmailAsync(_accessor.GetUserEmail() ?? "");

                if (existingUser == null)
                {
                    return Response<UserResource>.Failure(ResponseResult.UserIsNotAuthorized, "Użytkownik nie jest obecnie zalogowany");
                }
                
                var loggedUser = new LoggedUser
                {
                    Id = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email,
                    Token = _webTokenGenerator.CreateToken(existingUser)
                };

                var resource = _mapper.Map<LoggedUser, UserResource>(loggedUser);

                return Response<UserResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}