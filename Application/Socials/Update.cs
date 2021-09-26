using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Persistence.Context;

namespace Application.Socials
{
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public string Platform { get; set; }
            public string DisplayedName { get; set; }
            public string Path { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
                RuleFor(p => p.Platform).NotEmpty();
                RuleFor(p => p.DisplayedName).NotEmpty();
                RuleFor(p => p.Path).NotEmpty();
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
                var existingSocial = await _context.SocialMediaLinks.FindAsync(request.Id);

                if (existingSocial == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                existingSocial.Platform = request.Platform;
                existingSocial.DisplayedName = request.DisplayedName;
                existingSocial.Path = request.Path;

                _context.SocialMediaLinks.Update(existingSocial);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}