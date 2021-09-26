using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abilities.Resources;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Abilities
{
    public class Get
    {
        public class Query : IRequest<Response<AbilityResource>>
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

        public class Handler : IRequestHandler<Query, Response<AbilityResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<AbilityResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ability = await _context.Abilities.Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (ability == null)
                {
                    return Response<AbilityResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<Ability, AbilityResource>(ability);
                
                return Response<AbilityResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}