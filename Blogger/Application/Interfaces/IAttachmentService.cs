using Application.Dto.AttachmentDto;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAttachmentService
{
    Task<IEnumerable<AttachmentDto>> GetAttachmentsByPostIdAsync(int postId);

    Task<DownloadAttachmentDto> DownloadAttachmentByIdAsync(int id);

    Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile filer);

    Task DelateAttachmentAsync(int id);
}