using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Projects.Resources;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Projects
{
    public class UploadImage
    {
        public class Command : IRequest<Response<Unit>>
        {
            public Guid Id { get; set; }
            public IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.File).NotEmpty();
                RuleFor(p => p.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Response<Unit>>
        {

            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DataContext context, IUnitOfWork unitOfWork, IPhotoAccessor photoAccessor)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _photoAccessor = photoAccessor;
            }

            public async Task<Response<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingProject = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (existingProject == null)
                {
                    return Response<Unit>.Failure(ResponseResult.ResourceDoesntExist, "Nie znaleziono wpisu dla podanego identyfikatora");
                }
                
                PhotoUploadResult uploadedImage;
                
                try
                {
                    uploadedImage = _photoAccessor.UploadImage(request.File);
                }
                catch (Exception e)
                {
                    return Response<Unit>.Failure(ResponseResult.BadRequestStructure, "Nie udało się załadować zdjęcia");
                }

                var projectImage = new ProjectImage
                {
                    Id = Guid.NewGuid(),
                    Path = uploadedImage.Url,
                    Project = existingProject
                };

                await _context.ProjectImages.AddAsync(projectImage, cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return Response<Unit>.Success(ResponseResult.Updated);
            }
        }
    }
}