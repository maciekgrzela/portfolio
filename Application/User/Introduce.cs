using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Responses;
using Application.User.Resources;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.User
{
    public class Introduce
    {
        public class Query : IRequest<Response<IntroduceResource>> { }
        
        public class Handler : IRequestHandler<Query, Response<IntroduceResource>>
        {
            private readonly UserManager<Domain.Models.User> _manager;
            private readonly IMapper _mapper;

            public Handler(UserManager<Domain.Models.User> manager, IMapper mapper)
            {
                _manager = manager;
                _mapper = mapper;
            }
            
            public async Task<Response<IntroduceResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = await _manager.Users
                    .Include(p => p.Abilities.OrderByDescending(p => p.PercentageValue))
                    .Include(p => p.Achievements.OrderByDescending(p => p.Years))
                    .Include(p => p.Projects)
                    .ThenInclude(p => p.ProjectImages)
                    .Include(p => p.Projects)
                    .ThenInclude(p => p.ProjectTags)
                    .Include(p => p.WorkExperiences.OrderByDescending(p => p.DateStart))
                    .Include(p => p.SocialMediaLinks)
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
                
                if (currentUser == null)
                {
                    return Response<IntroduceResource>.Failure(ResponseResult.ResourceDoesntExist, "Użytkownik nie został znaleziony");
                }

                var resource = _mapper.Map<Domain.Models.User, IntroduceResource>(currentUser);
                return Response<IntroduceResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}