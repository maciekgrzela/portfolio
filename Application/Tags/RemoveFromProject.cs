using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Tags
{
    public class RemoveFromProject
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
                var existingProjectTag = await _context.ProjectTags.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId && p.TagId == request.TagId, cancellationToken: cancellationToken);

                if (existingProjectTag == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                _context.ProjectTags.Remove(existingProjectTag);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Deleted);
            }
        }
    }
}