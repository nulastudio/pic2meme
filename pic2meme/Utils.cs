using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Security.Cryptography;

using Image = System.Drawing.Image;
using ISImage = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;
using System.Text;
using System.Threading.Tasks;

namespace pic2meme
{
    enum ImageFormat
    {
        Unknown,
        Jpeg,
        Png,
        Bmp,
        Gif,
    }
    class Utils
    {
        public static string md5(string content)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            byte[] result = md5.ComputeHash(data);
            StringBuilder @string = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                @string.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return @string.ToString();
        }

        public static ImageFormat DetectFormat(string filePath)
        {
            try
            {
                var format = ISImage.DetectFormat(filePath);
                if (format == null) return ImageFormat.Unknown;
                if (format is JpegFormat) return ImageFormat.Jpeg;
                if (format is PngFormat) return ImageFormat.Png;
                if (format is BmpFormat) return ImageFormat.Bmp;
                if (format is GifFormat) return ImageFormat.Gif;
            }
            catch (Exception)
            {
                return ImageFormat.Unknown;
            }

            return ImageFormat.Unknown;
        }

        public static (int width, int height) GetImageSize(string filePath)
        {
            int width = 0, height = 0;

            try
            {
                var imageInfo = ISImage.Identify(filePath);
                width = imageInfo.Width;
                height = imageInfo.Height;
            }
            catch (Exception)
            {
            }

            return (width, height);
        }

        public static Task<bool> Any2GIF(string filePath, string savePath, int forceSize = 0)
        {
            try
            {
                return ISImage.LoadAsync(filePath).ContinueWith(task => {
                    var image = task.Result;
                    int max = Math.Max(image.Width, image.Height);
                    if (forceSize > 0 && max != forceSize)
                    {
                        float scale = (float)max / (float)forceSize;
                        image.Mutate(x => x.Resize((int)(image.Width / scale), (int)(image.Height / scale)));
                    }
                    image.SaveAsGif(savePath);
                    image.Dispose();

                    return true;
                });
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public static Task<bool> Any2GIF2(string filePath, string savePath, int forceSize = 0)
        {
            try
            {
                return ISImage.LoadAsync(filePath).ContinueWith(task => {
                    var image = task.Result;
                    int max = Math.Max(image.Width, image.Height);
                    if (forceSize > 0 && max != forceSize)
                    {
                        float scale = (float)max / (float)forceSize;
                        image.Mutate(x => x.Resize((int)(image.Width / scale), (int)(image.Height / scale)));
                    }
                    var gifEncoder = new GifEncoder();
                    gifEncoder.ColorTableMode = GifColorTableMode.Local;
                    gifEncoder.Quantizer = SixLabors.ImageSharp.Processing.KnownQuantizers.Octree;
                    image.SaveAsGif(savePath, gifEncoder);
                    image.Dispose();

                    return true;
                });
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
