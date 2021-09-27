using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Responses;
using FluentValidation;
using MediatR;
using Persistence.Context;

namespace Application.WorkExperiences
{
    public class Update
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public string Title { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty();
                RuleFor(p => p.DateStart).NotEmpty();
                RuleFor(p => p.DateEnd).NotEmpty();
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
                var existingWorkExperience = await _context.WorkExperiences.FindAsync(request.Id);

                if (existingWorkExperience == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }

                existingWorkExperience.Title = request.Title;
                existingWorkExperience.DateStart = request.DateStart;
                existingWorkExperience.DateEnd = request.DateEnd;

                _context.WorkExperiences.Update(existingWorkExperience);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}