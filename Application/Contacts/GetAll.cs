using System;
using System.Collections.Generic;
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
    public class GetAll
    {
        public class Query : IRequest<Response<List<ContactResource>>> { }

        public class Handler : IRequestHandler<Query, Response<List<ContactResource>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<ContactResource>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contacts = await _context.ContactFormRequests.ToListAsync(cancellationToken: cancellationToken);

                var resources = _mapper.Map<List<ContactFormRequest>, List<ContactResource>>(contacts);
                
                return Response<List<ContactResource>>.Success(ResponseResult.DataObtained, resources);
            }
        }
    }
}