using System;
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
    public class Get
    {
        public class Query : IRequest<Response<WorkExperienceResource>>
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

        public class Handler : IRequestHandler<Query, Response<WorkExperienceResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<WorkExperienceResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var workExperience = await _context.WorkExperiences
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (workExperience == null)
                {
                    return Response<WorkExperienceResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<WorkExperience, WorkExperienceResource>(workExperience);
                
                return Response<WorkExperienceResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}