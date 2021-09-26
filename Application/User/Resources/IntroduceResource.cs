using System;
using System.Collections.Generic;
using Domain.Models;

namespace Application.User.Resources
{
    public class IntroduceResource
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelfDescription { get; set; }
        public bool LookingForAJob { get; set; }
        public List<SocialMediaLinkForIntroduceResource> SocialMediaLinks { get; set; }
        public List<AchievementForIntroduceResource> Achievements { get; set; }
        public List<WorkExperienceForIntroduceResource> WorkExperiences { get; set; }
        public List<AbilityForIntroduceResource> Abilities { get; set; }
        public List<ProjectForIntroduceResource> Projects { get; set; }
        public string GithubPath { get; set; }
        public string AzureDevOpsPath { get; set; }
        public string BitbucketPath { get; set; }
    }

    public class SocialMediaLinkForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string DisplayedName { get; set; }
        public string Path { get; set; }
    }

    public class AchievementForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Years { get; set; }
        public string Title { get; set; }
    }

    public class WorkExperienceForIntroduceResource
    {
        public Guid Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
    }

    public class AbilityForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double PercentageValue { get; set; }
    }

    public class ProjectForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int WorkGroup { get; set; }
        public string Responsibility { get; set; }
        public List<ProjectTagForIntroduceResource> ProjectTags { get; set; }
        public string Description { get; set; }
        public RepositoryLinkForIntroduceResource RepositoryLink { get; set; }
        public bool IsPrivate { get; set; }
        public List<ProjectImageForIntroduceResource> ProjectImages { get; set; }
    }

    public class ProjectTagForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }

    public class ProjectImageForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }

    public class RepositoryLinkForIntroduceResource
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string Path { get; set; }
    }
}