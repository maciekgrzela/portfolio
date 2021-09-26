using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Abilities
{
    public class Create
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string Name { get; set; }
            public double PercentageValue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
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
            private readonly UserManager<Domain.Models.User> _userManager;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUnitOfWork unitOfWork, UserManager<Domain.Models.User> userManager, IUserAccessor userAccessor)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }

            public async Task<Response<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingUser = await _userManager.FindByEmailAsync(_userAccessor.GetUserEmail());

                if (existingUser == null)
                {
                    return Response<Unit>.Failure(ResponseResult.UserIsNotAuthorized, "Użytkownik nie został uwierzytelniony");
                }
                
                var existingAbility = await _context.Abilities.FirstOrDefaultAsync(p => p.Name.Equals(request.Name), cancellationToken: cancellationToken);

                if (existingAbility != null)
                {
                    return Response<Unit>.Failure(ResponseResult.BadRequestStructure, "Wpis z taką nazwą już istnieje");
                }

                var ability = new Ability
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    PercentageValue = request.PercentageValue,
                    User = existingUser
                };

                await _context.Abilities.AddAsync(ability, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}