using Application.Features.Images.Models;
using Application.Features.Images.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Images;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<CloudinarySettings> config)
    {
        var settings = config.Value;

        var account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResultDto> UploadAsync(
        IFormFile file,
        string? folder = null,
        CancellationToken ct = default)
    {
        if(file is null || file.Length == 0)
            throw new ArgumentException("Image file is required");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder
        };

        var result = await _cloudinary.UploadAsync(uploadParams, ct);

        if(result.Error is not null)
            throw new InvalidOperationException(result.Error.Message);

        return new ImageUploadResultDto
        {
            Url = result.SecureUrl?.ToString() ?? throw new InvalidOperationException("Upload succeeded but no URL was returned."),
            PublicId = result.PublicId
        };
    }

    public async Task DeleteAsync(string publicId, CancellationToken ct = default)
    {
        if(string.IsNullOrWhiteSpace(publicId))
            throw new ArgumentException("Public ID is required");

        var deleteParams = new DeletionParams(publicId){Invalidate = true};
            
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if(result.Error is not null)
            throw new InvalidOperationException(result.Error.Message);
    }
}