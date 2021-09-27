using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string SelfDescription { get; set; }
            public bool LookingForAJob { get; set; }
            public string GithubLink { get; set; }
            public string AzureDevOpsPath { get; set; }
            public string BitbucketPath { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.FirstName).NotEmpty();
                RuleFor(p => p.LastName).NotEmpty();
                RuleFor(p => p.SelfDescription).NotEmpty();
                RuleFor(p => p.LookingForAJob).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Response<Unit>>
        {
            private readonly UserManager<Domain.Models.User> _userManager;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAccessor _userAccessor;

            public Handler(UserManager<Domain.Models.User> userManager, IUnitOfWork unitOfWork, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
                _userAccessor = userAccessor;
            }
            
            public async Task<Response<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingUser = await _userManager.FindByEmailAsync(_userAccessor.GetUserEmail() ?? "");

                if (existingUser == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Użytkownik nie został znaleziony");
                }

                existingUser.FirstName = request.FirstName;
                existingUser.LastName = request.LastName;
                existingUser.SelfDescription = request.SelfDescription;
                existingUser.LookingForAJob = request.LookingForAJob;
                existingUser.GithubPath = request.GithubLink ?? existingUser.GithubPath;
                existingUser.AzureDevOpsPath = request.AzureDevOpsPath ?? existingUser.AzureDevOpsPath;
                existingUser.BitbucketPath = request.BitbucketPath ?? existingUser.BitbucketPath;

                await _userManager.UpdateAsync(existingUser);
                await _unitOfWork.CommitTransactionAsync();

                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}