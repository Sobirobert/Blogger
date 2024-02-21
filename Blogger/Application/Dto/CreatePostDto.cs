
using AutoMapper;
using Domain.Entities;

namespace Application.Dto;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePostDto, Post>();
    }
}
