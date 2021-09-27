using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abilities.Resources;
using Application.Projects.Resources;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Projects
{
    public class GetAll
    {
        public class Query : IRequest<Response<List<ProjectResource>>> { }

        public class Handler : IRequestHandler<Query, Response<List<ProjectResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<ProjectResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projects = await _context.Projects
                    .Include(p => p.ProjectImages.OrderBy(p => p.Path))
                    .Include(p => p.ProjectTags)
                    .Include(p => p.User)
                    .OrderByDescending(p => p.Order)
                    .ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<Project>, List<ProjectResource>>(projects);
                
                return Response<List<ProjectResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}