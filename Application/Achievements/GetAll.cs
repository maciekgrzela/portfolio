using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abilities.Resources;
using Application.Achievements.Resources;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Achievements
{
    public class GetAll
    {
        public class Query : IRequest<Response<List<AchievementResource>>> { }

        public class Handler : IRequestHandler<Query, Response<List<AchievementResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<AchievementResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var achievements = await _context.Achievements
                    .Include(p => p.User)
                    .ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<Achievement>, List<AchievementResource>>(achievements);
                
                return Response<List<AchievementResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}