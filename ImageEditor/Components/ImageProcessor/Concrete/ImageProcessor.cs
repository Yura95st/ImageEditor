namespace ImageEditor.Components.ImageProcessor.Concrete
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ImageEditor.Components.ImageProcessor.Abstract;
    using ImageEditor.Utils;

    public class ImageProcessor : IImageProcessor
    {
        public static readonly int MaxBrightness;

        public static readonly int MaxContrast;

        public static readonly int MaxOpacity;

        public static readonly int MaxRotationAngle;

        public static readonly int MinBrightness;

        public static readonly int MinContrast;

        public static readonly int MinOpacity;

        public static readonly int MinRotationAngle;

        static ImageProcessor()
        {
            ImageProcessor.MinBrightness = -100;
            ImageProcessor.MaxBrightness = 100;

            ImageProcessor.MinContrast = -100;
            ImageProcessor.MaxContrast = 100;

            ImageProcessor.MinOpacity = 0;
            ImageProcessor.MaxOpacity = 100;

            ImageProcessor.MinRotationAngle = -180;
            ImageProcessor.MaxRotationAngle = 180;
        }

        #region IImageProcessor Members

        public BitmapSource ChangeBrightness(BitmapSource image, int newBrightness)
        {
            throw new System.NotImplementedException();
        }

        public BitmapSource ChangeContrast(BitmapSource image, int newContrast)
        {
            throw new System.NotImplementedException();
        }

        public BitmapSource ChangeOpacity(BitmapSource image, int newOpacity)
        {
            Guard.NotNull(image, "image");

            if (newOpacity < 0 || newOpacity > 100)
            {
                throw new ArgumentOutOfRangeException("newOpacity", "Opacity must be between 0 and 100.");
            }

            int pixelsCount = image.PixelWidth * image.PixelHeight;

            int[] pixels = new int[pixelsCount];

            int stride = (image.PixelWidth * image.Format.BitsPerPixel + 7) / 8;

            int coef = (int)(2.55 * newOpacity);

            image.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixelsCount; i++)
            {
                int red = (pixels[i] >> 16) & 255;
                int green = (pixels[i] >> 8) & 255;
                int blue = pixels[i] & 255;
                int alpha = (255 - coef) & 255;

                int color = (alpha << 24) + (red << 16) + (green << 8) + blue;

                pixels[i] = color;
            }

            BitmapSource result = BitmapSource.Create(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY,
            PixelFormats.Bgra32, image.Palette, pixels, stride);

            return result;
        }

        public BitmapSource Crop(BitmapSource image, Point leftTopCornerPoint, double width, double height)
        {
            Guard.NotNull(image, "image");
            Guard.GreaterThanZero(width, "width");
            Guard.GreaterThanZero(height, "height");

            if (leftTopCornerPoint.X < 0 || leftTopCornerPoint.Y < 0 || leftTopCornerPoint.X > image.Width
            || leftTopCornerPoint.Y > image.Height)
            {
                throw new ArgumentOutOfRangeException("leftTopCornerPoint",
                "LeftTopCornerPoint must be within the boundaries of the image.");
            }

            double maxWidth = image.Width - leftTopCornerPoint.X;
            double maxHeight = image.Height - leftTopCornerPoint.Y;

            if (width > maxWidth)
            {
                width = maxWidth;
            }

            if (height > maxHeight)
            {
                height = maxHeight;
            }

            CroppedBitmap result = new CroppedBitmap(image,
            new Int32Rect((int)leftTopCornerPoint.X, (int)leftTopCornerPoint.Y, (int)height, (int)width));

            return result;
        }

        public BitmapSource Rotate(BitmapSource image, int angle)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}