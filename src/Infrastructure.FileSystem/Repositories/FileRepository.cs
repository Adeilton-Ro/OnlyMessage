using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;

namespace Infrastructure.FileSystem.Repositories;
public class FileRepository : IFileRepository
{
    public string GetUserAvatarFilePath(string id, string extension)
    {
        return $"/UsersAvatar/{id.Replace("-", "")}{extension}";
    }

    public string GetDefaultImagePath() => "/UsersAvatar/DefaultProfile.jpg";

    public void SaveUserAvatarImage(string id, byte[] image, string extension)
    {
        var files = Directory.GetFiles("wwwroot/Employees/");
        var path = "wwwroot" + GetUserAvatarFilePath(id, extension);
        var existingFile = files.FirstOrDefault(f => f.Split(".")[0] == path.Split(".")[0]);
        if (existingFile is not null)
            File.Delete(existingFile);
        File.WriteAllBytes(path, image);
    }

    public void DeleteImage(string path)
    {
        File.Delete("wwwroot" + path);
    }
}