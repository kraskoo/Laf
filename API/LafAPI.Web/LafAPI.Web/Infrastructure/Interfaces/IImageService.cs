namespace LafAPI.Web.Infrastructure.Interfaces
{
    using System.IO;

    public interface IImageService
    {
        string ConvertImage(Stream stream);
    }
}