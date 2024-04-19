using Domain.Entities;

namespace Domain.Interfaces;

public interface IPictureRepository
{
    Task<IEnumerable<Picture>> GetByPostIdAsync(int postId);

    Task<Picture> AddAsync(Picture picture);

    Task<Picture> GetByIdAsync(int id);

    Task SetMainPictureAsync(int postId, int id);

    Task DeleteAsync(Picture picture);
}