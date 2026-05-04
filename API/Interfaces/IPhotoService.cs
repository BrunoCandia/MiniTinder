using API.DTO;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoResponseDto> UploadPhotoAsync(IFormFile file);
        Task<bool> DeletePhotoAsync(string publicId);
    }
}
