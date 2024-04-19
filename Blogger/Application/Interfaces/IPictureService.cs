using Application.Dto;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPictureService
{
    Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file);

    Task<IEnumerable<PictureDto>> GetPicturesByPostIdAsync(int postId);

    Task<PictureDto> GetPictureByIdAsync(int id);

    Task SetMainPicture(int postId, int id);

    Task DeletePictureAsync(int id);
}