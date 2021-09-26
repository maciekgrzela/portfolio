using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Application.Socials.Resources;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Tags
{
    public class Create
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string Value { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Value).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Response<Unit>>
        {

            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(DataContext context, IUnitOfWork unitOfWork)
            {
                _context = context;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingTag = await _context.Tags
                    .FirstOrDefaultAsync(p => p.Value == request.Value, cancellationToken: cancellationToken);

                if (existingTag != null)
                {
                    return Response<Unit>.Failure(ResponseResult.BadRequestStructure, "Tag dla podanej wartości już istnieje");
                }

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Value = request.Value,
                    ProjectTags = new List<ProjectTag>()
                };

                await _context.Tags.AddAsync(tag, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}