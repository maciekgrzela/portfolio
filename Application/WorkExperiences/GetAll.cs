using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Achievements.Resources;
using Application.Responses;
using Application.WorkExperiences.Resources;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.WorkExperiences
{
    public class GetAll
    {
        public class Query : IRequest<Response<List<WorkExperienceResource>>> { }

        public class Handler : IRequestHandler<Query, Response<List<WorkExperienceResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<WorkExperienceResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var workExperiences = await _context
                    .WorkExperiences
                    .Include(p => p.User)
                    .ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<WorkExperience>, List<WorkExperienceResource>>(workExperiences);
                
                return Response<List<WorkExperienceResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}