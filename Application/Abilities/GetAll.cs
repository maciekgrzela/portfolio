using System;
using System.Collections.Generic;
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
    public class GetAll
    {
        public class Query : IRequest<Response<List<AbilityResource>>> { }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator() { }
        }

        public class Handler : IRequestHandler<Query, Response<List<AbilityResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<AbilityResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var abilities = await _context.Abilities.Include(p => p.User)
                    .ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<Ability>, List<AbilityResource>>(abilities);
                
                return Response<List<AbilityResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}