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

namespace Application.Socials
{
    public class Create
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string Platform { get; set; }
            public string DisplayedName { get; set; }
            public string Path { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Platform).NotEmpty();
                RuleFor(p => p.DisplayedName).NotEmpty();
                RuleFor(p => p.Path).NotEmpty();
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

                var socialLink = new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    Platform = request.Platform,
                    Path = request.Path,
                    DisplayedName = request.DisplayedName,
                    User = existingUser
                };

                await _context.SocialMediaLinks.AddAsync(socialLink, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}