using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string subFolder);
}

public class FileSystemStorageService : IFileStorageService
{
    private readonly string _uploadsRoot;

    public FileSystemStorageService(IConfiguration configuration)
    {
        var cfgPath = configuration["FileStorage:Local:UploadPath"];
        _uploadsRoot = string.IsNullOrWhiteSpace(cfgPath) ? Path.Combine(Directory.GetCurrentDirectory(), "uploads") : cfgPath;
        if (!Directory.Exists(_uploadsRoot)) Directory.CreateDirectory(_uploadsRoot);
    }

    public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
    {
        var dir = Path.Combine(_uploadsRoot, subFolder);
        Directory.CreateDirectory(dir);
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var full = Path.Combine(dir, fileName);
        using (var stream = new FileStream(full, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return full;
    }
}
