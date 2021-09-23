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
    public class GetMyData
    {
        public class Query : IRequest<Response<MyDataResource>> { }
        
        public class Handler : IRequestHandler<Query, Response<MyDataResource>>
        {
            private readonly UserManager<Domain.Models.User> _manager;
            private readonly IMapper _mapper;

            public Handler(UserManager<Domain.Models.User> manager, IMapper mapper)
            {
                _manager = manager;
                _mapper = mapper;
            }
            
            public async Task<Response<MyDataResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = await _manager.Users.SingleOrDefaultAsync(cancellationToken: cancellationToken);
                if (currentUser == null)
                {
                    return Response<MyDataResource>.Failure(ResponseResult.ResourceDoesntExist, "Użytkownik nie został znaleziony");
                }

                var resource = _mapper.Map<Domain.Models.User, MyDataResource>(currentUser);
                return Response<MyDataResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}