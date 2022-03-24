using Microsoft.AspNetCore.Http;

namespace Core.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
    
    public static IFormFile GetFormFile(this byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, bytes.Length, "", "");
        return file;
    }
}