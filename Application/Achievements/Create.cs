using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Application.Achievements
{
    public class Create
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string Years { get; set; }
            public string Title { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Years).NotEmpty();
                RuleFor(p => p.Title).NotEmpty();
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

                var achievement = new Achievement
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Years = request.Years,
                    User = existingUser
                };

                await _context.Achievements.AddAsync(achievement, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}