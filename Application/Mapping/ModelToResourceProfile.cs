using Application.Abilities.Resources;
using Application.Achievements.Resources;
using Application.Contacts.Resources;
using Application.Projects.Resources;
using Application.Socials.Resources;
using Application.User.Resources;
using Application.WorkExperiences.Resources;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.User, IntroduceResource>();
            CreateMap<Domain.Models.User, UserForAbilityResource>();
            CreateMap<Domain.Models.User, UserForAchievementResource>();
            CreateMap<Domain.Models.User, UserForProjectResource>();
            CreateMap<Domain.Models.User, UserForWorkExperienceResource>();
            CreateMap<Ability, AbilityResource>();
            CreateMap<Ability, AbilityForIntroduceResource>();
            CreateMap<Achievement, AchievementResource>();
            CreateMap<ContactFormRequest, ContactResource>();
            CreateMap<Project, ProjectResource>();
            CreateMap<ProjectImage, ImageForProjectResource>();
            CreateMap<ProjectTag, TagForProjectResource>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => p.Tag.Id))
                .ForMember(p => p.Value, opt => opt.MapFrom(p => p.Tag.Value));
            CreateMap<SocialMediaLink, SocialResource>();
            CreateMap<SocialMediaLink, SocialMediaLinkForIntroduceResource>();
            CreateMap<Achievement, AchievementForIntroduceResource>();
            CreateMap<WorkExperience, WorkExperienceResource>();
            CreateMap<WorkExperience, WorkExperienceForIntroduceResource>();
            CreateMap<LoggedUser, UserResource>();
        }
    }
}