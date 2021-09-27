using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Persistence.Context;

namespace Application.Achievements
{
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public string Years { get; set; }
            public string Title { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
                RuleFor(p => p.Years).NotEmpty();
                RuleFor(p => p.Title).NotEmpty();
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
                var existingAchievement = await _context.Achievements.FindAsync(request.Id);

                if (existingAchievement == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                existingAchievement.Title = request.Title;
                existingAchievement.Years = request.Years;

                _context.Achievements.Update(existingAchievement);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}