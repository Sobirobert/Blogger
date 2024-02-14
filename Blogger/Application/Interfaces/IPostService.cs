using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPostService
{
    IEnumerable<PostDto> GetAllPosts();
    PostDto GetPostByID(int id);
}
