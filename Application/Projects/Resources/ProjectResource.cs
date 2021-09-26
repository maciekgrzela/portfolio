using System;
using System.Collections.Generic;
using Application.User.Resources;
using Domain.Models;

namespace Application.Projects.Resources
{
    public class ProjectResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int WorkGroup { get; set; }
        public string Responsibility { get; set; }
        public List<TagForProjectResource> Tags { get; set; }
        public string Description { get; set; }
        public string RepositoryLink { get; set; }
        public bool IsPrivate { get; set; }
        public List<ImageForProjectResource> ProjectImages { get; set; }
        public UserForProjectResource User { get; set; }
    }

    public class TagForProjectResource
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }

    public class ImageForProjectResource
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }

    public class UserForProjectResource : BasicUserResource { }
}