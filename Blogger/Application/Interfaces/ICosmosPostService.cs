using Application.Dto.Cosmos;

namespace Application.Interfaces;

public interface ICosmosPostService
{
    Task<IEnumerable<CosmosPostDto>> GetAllPostAsync();
    Task<CosmosPostDto> GetPostByIdAsync(string id);
    Task<CosmosPostDto> AddNewPostAsync(CreateCosmosPostDto newPost);
    Task UpdatePostAsync(UpdateCosmosPostDto updatePost);
    Task DeletePostAsync(string id);
}
