using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Achievements.Resources;
using Application.Responses;
using Application.Socials.Resources;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Socials
{
    public class Get
    {
        public class Query : IRequest<Response<SocialResource>>
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

        public class Handler : IRequestHandler<Query, Response<SocialResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<SocialResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var social = await _context.SocialMediaLinks
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (social == null)
                {
                    return Response<SocialResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<SocialMediaLink, SocialResource>(social);
                
                return Response<SocialResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}