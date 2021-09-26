using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Tags
{
    public class AssignToProject
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid ProjectId { get; set; }
            public Guid TagId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.TagId).NotEmpty();
                RuleFor(p => p.ProjectId).NotEmpty();
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
                var existingProjectTagAssociation = await _context.ProjectTags
                        .FirstOrDefaultAsync(p =>
                        p.ProjectId == request.ProjectId && p.TagId == request.TagId, cancellationToken: cancellationToken);

                if (existingProjectTagAssociation != null)
                {
                    return Response<Unit>.Failure(ResponseResult.BadRequestStructure, "Tag o podanym identyfikatorze jest już powiązany z projektem");
                }

                var projectTag = new ProjectTag
                {
                    ProjectId = request.ProjectId,
                    TagId = request.TagId
                };

                await _context.ProjectTags.AddAsync(projectTag, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}