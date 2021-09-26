using System;
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
    public class Get
    {
        public class Query : IRequest<Response<ProjectResource>>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Response<ProjectResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<ProjectResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectImages)
                    .Include(p => p.ProjectTags)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (project == null)
                {
                    return Response<ProjectResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<Project, ProjectResource>(project);
                
                return Response<ProjectResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}