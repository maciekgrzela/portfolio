using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Persistence.Context;

namespace Application.Projects
{
    public class RemoveImage
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
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
                var existingImage = await _context.ProjectImages.FindAsync(request.Id);

                if (existingImage == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                _context.ProjectImages.Remove(existingImage);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Deleted);
            }
        }
    }
}