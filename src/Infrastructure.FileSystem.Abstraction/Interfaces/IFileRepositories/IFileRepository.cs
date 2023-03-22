namespace Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
public interface IFileRepository
{
    void SaveUserAvatarImage(string id, byte[] image, string extension);
    string GetDefaultImagePath();
    void DeleteImage(string path);
    string GetUserAvatarFilePath(string id, string extension);
}