using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostAsync();
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
    Task UpdatePostAsync(UpdatePostDto updatePost);
    Task DeletePostAsync(int id);
    Task<List<PostDto>> SearachingPostAsync(string searchingTitle);
}
