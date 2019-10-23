namespace LafAPI.Web.Infrastructure.Services
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.AspNetCore.Hosting;

    public class ImageService : IImageService
    {
        private const int Size = 150;
        private const int Quality = 72;

        private readonly IWebHostEnvironment environment;

        public ImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public string ConvertImage(Stream stream)
        {
            var rootPath = Path.Combine(this.environment.ContentRootPath, "wwwroot");
            var currentDate = DateTime.UtcNow;
            var filePath = Path.Combine(rootPath, $"{currentDate.Year}{currentDate.Month}{currentDate.Day}");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fileName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}.png";
            int width = Size;
            int height = Size;
            using var image = new Bitmap(stream, true);
            var resized = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(image, 0, 0, width, height);
            var qualityParamId = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1)
                                        {
                                            Param =
                                                {
                                                    [0] = new EncoderParameter(
                                                        qualityParamId,
                                                        Quality)
                                                }
                                        };
            var codec = ImageCodecInfo.GetImageDecoders()
                .FirstOrDefault(codecInfo => codecInfo.FormatID == ImageFormat.Png.Guid);
            var path = Path.Combine(filePath, fileName);
            resized.Save(path, codec, encoderParameters);
            return path;
        }
    }
}