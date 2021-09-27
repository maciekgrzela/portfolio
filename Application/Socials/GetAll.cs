using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Achievements.Resources;
using Application.Responses;
using Application.Socials.Resources;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Socials
{
    public class GetAll
    {
        public class Query : IRequest<Response<List<SocialResource>>> { }

        public class Handler : IRequestHandler<Query, Response<List<SocialResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<SocialResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var socials = await _context.SocialMediaLinks
                    .Include(p => p.User)
                    .ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<SocialMediaLink>, List<SocialResource>>(socials);
                
                return Response<List<SocialResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}