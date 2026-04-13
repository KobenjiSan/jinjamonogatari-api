using Application.Features.Images.Models;

namespace Application.Features.Images.Services;

public interface IImageService
{
    Task<ImageUploadResultDto> UploadAsync(IFormFile file, string? folder = null, CancellationToken ct = default);

    Task DeleteAsync(string publicId, CancellationToken ct = default);
}