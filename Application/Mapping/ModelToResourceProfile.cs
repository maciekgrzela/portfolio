using Application.User.Resources;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.User, MyDataResource>();
            CreateMap<LoggedUser, UserResource>();
        }
    }
}