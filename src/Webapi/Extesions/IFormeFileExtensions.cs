namespace Webapi.Extesions;
public static class IFormeFileExtensions
{
    public static byte[] ToBytes(this IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms.ToArray();
    }
}