namespace ImageEditor.Utils
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    public static class ImageHelper
    {
        /// <summary>Gets the bitmap source from the file with specified path.</summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The bitmap source.</returns>
        public static BitmapSource GetBitmapSourceFromFile(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

            bitmapImage.EndInit();

            return bitmapImage;
        }

        /// <summary>Saves the specified image to file with specified path.</summary>
        /// <param name="image">The image.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveImageToFile(BitmapSource image, string filePath)
        {
            Guard.NotNull(image, "image");
            Guard.NotNull(filePath, "filePath");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(filePath)
                .ToLower())
                {
                    case ".bmp":
                    {
                        encoder = new BmpBitmapEncoder();

                        break;
                    }

                    case ".gif":
                    {
                        encoder = new GifBitmapEncoder();

                        break;
                    }

                    case ".jpg":
                    case ".jpeg":
                    {
                        encoder = new JpegBitmapEncoder();

                        break;
                    }

                    case ".png":
                    {
                        encoder = new PngBitmapEncoder();

                        break;
                    }

                    case ".tif":
                    case ".tiff":
                    {
                        encoder = new TiffBitmapEncoder();

                        break;
                    }

                    default:
                    {
                        encoder = new PngBitmapEncoder();
                        break;
                    }
                }

                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}