using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dto;

public class SearchPostDto : IMap
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<SearchPostDto, Post>();
    }
}
