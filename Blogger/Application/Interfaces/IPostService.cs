using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    IEnumerable<PostDto> GetAllPosts();

    PostDto GetPostByID(int id);

    PostDto AddNewPost(CreatePostDto newPost);
}
