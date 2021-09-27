using System;
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
    public class Get
    {
        public class Query : IRequest<Response<AchievementResource>>
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

        public class Handler : IRequestHandler<Query, Response<AchievementResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<AchievementResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var achievement = await _context.Achievements.Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (achievement == null)
                {
                    return Response<AchievementResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<Achievement, AchievementResource>(achievement);
                
                return Response<AchievementResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}