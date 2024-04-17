
using Blogger.Contracts.Request;
using Blogger.Contracts.Responses;
using Refit;

namespace Blogger.Sdk;

[Headers("Authorization: Bearer")]
public interface IBloggerApi
{
    [Get("/api/posts/{id}")]
    Task<ApiResponse<Response<PostDto>>> GetPostAsync(int id);

    [Post("/api/posts")]
    Task<ApiResponse<Response<PostDto>>> CreatePostAsync(CreatePostDto newPost);

    [Put("/api/posts")]
    Task UpdatePostAsync(UpdatePostDto updatePost);

    [Delete("/api/posts/{id}")]
    Task DeletePostAsync(int id);
}
