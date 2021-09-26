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
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
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
                RuleFor(p => p.Id).NotEmpty();
                RuleFor(p => p.Title).NotEmpty();
                RuleFor(p => p.WorkGroup).NotEmpty();
                RuleFor(p => p.Responsibility).NotEmpty();
                RuleFor(p => p.Description).NotEmpty();
                RuleFor(p => p.RepositoryLink).NotEmpty();
                RuleFor(p => p.IsPrivate).NotEmpty();
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
                var existingProject = await _context.Projects.FindAsync(request.Id);

                if (existingProject == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                existingProject.Title = request.Title;
                existingProject.WorkGroup = request.WorkGroup;
                existingProject.Responsibility = request.Responsibility;
                existingProject.Description = request.Description;
                existingProject.RepositoryLink = request.RepositoryLink;
                existingProject.IsPrivate = request.IsPrivate;

                _context.Projects.Update(existingProject);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}