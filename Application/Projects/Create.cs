using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;

namespace Application.Projects
{
    public class Create
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string Title { get; set; }
            public int Order { get; set; }
            public int WorkGroup { get; set; }
            public string Responsibility { get; set; }
            public string Description { get; set; }
            public string RepositoryLink { get; set; }
            public bool IsPrivate { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Title).NotEmpty();
                RuleFor(p => p.WorkGroup).NotEmpty();
                RuleFor(p => p.Responsibility).NotEmpty();
                RuleFor(p => p.Description).NotEmpty();
                RuleFor(p => p.RepositoryLink).NotEmpty();
                RuleFor(p => p.IsPrivate).NotEmpty();
                RuleFor(p => p.Order).NotEmpty();
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

                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Order = request.Order,
                    WorkGroup = request.WorkGroup,
                    Responsibility = request.Responsibility,
                    IsPrivate = request.IsPrivate,
                    ProjectImages = new List<ProjectImage>(),
                    ProjectTags = new List<ProjectTag>(),
                    Description = request.Description,
                    RepositoryLink = request.RepositoryLink,
                    User = existingUser
                };
                
                await _context.Projects.AddAsync(project, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Created);
            }
        }
    }
}