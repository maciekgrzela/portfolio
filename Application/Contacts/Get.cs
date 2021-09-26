using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abilities.Resources;
using Application.Contacts.Resources;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Contacts
{
    public class Get
    {
        public class Query : IRequest<Response<ContactResource>>
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

        public class Handler : IRequestHandler<Query, Response<ContactResource>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<ContactResource>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contact = await _context.ContactFormRequests
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (contact == null)
                {
                    return Response<ContactResource>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                var resource = _mapper.Map<ContactFormRequest, ContactResource>(contact);
                
                return Response<ContactResource>.Success(ResponseResult.DataObtained, resource);
            }
        }
    }
}