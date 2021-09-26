using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Abilities
{
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double PercentageValue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
                RuleFor(p => p.Name).NotEmpty();
                RuleFor(p => p.PercentageValue)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(100);
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
                var existingAbilityWithNewName = await _context.Abilities.FirstOrDefaultAsync(p => p.Name.Equals(request.Name), cancellationToken: cancellationToken);

                if (existingAbilityWithNewName != null)
                {
                    return Response<Unit>.Failure(ResponseResult.BadRequestStructure, "Wpis z taką nazwą już istnieje");
                }
                
                var existingAbility = await _context.Abilities.FirstOrDefaultAsync(p => p.Name.Equals(request.Name), cancellationToken: cancellationToken);

                if (existingAbility == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                existingAbility.Name = request.Name;
                existingAbility.PercentageValue = request.PercentageValue;

                _context.Abilities.Update(existingAbility);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}