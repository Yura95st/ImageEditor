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
            ImageProcessor.MaxOpacity = 255;

            ImageProcessor.MinRotationAngle = -180;
            ImageProcessor.MaxRotationAngle = 180;
        }

        #region IImageProcessor Members

        public BitmapSource ChangeBrightness(BitmapSource image, int newBrightness)
        {
            return this.AdjustImage(image, newBrightness, null, null);
        }

        public BitmapSource ChangeContrast(BitmapSource image, int newContrast)
        {
            return this.AdjustImage(image, null, newContrast, null);
        }

        public BitmapSource ChangeOpacity(BitmapSource image, int newOpacity)
        {
            return this.AdjustImage(image, null, null, newOpacity);
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

        private static int AdjustColorValue(int color)
        {
            if (color > 255)
            {
                color = 255;
            }
            else if (color < 0)
            {
                color = 0;
            }

            return color;
        }

        private BitmapSource AdjustImage(BitmapSource image, int? newBrightness, int? newContrast, int? newOpacity)
        {
            Guard.NotNull(image, "image");

            if (newBrightness.HasValue
            && (newBrightness.Value < ImageProcessor.MinBrightness || newBrightness.Value > ImageProcessor.MaxBrightness))
            {
                throw new ArgumentOutOfRangeException("newBrightness",
                string.Format("Brightness must be between {0} and {1}.", ImageProcessor.MinBrightness,
                ImageProcessor.MaxBrightness));
            }

            if (newContrast.HasValue
            && (newContrast.Value < ImageProcessor.MinContrast || newContrast.Value > ImageProcessor.MaxContrast))
            {
                throw new ArgumentOutOfRangeException("newContrast",
                string.Format("Contrast must be between {0} and {1}.", ImageProcessor.MinContrast, ImageProcessor.MaxContrast));
            }

            if (newOpacity.HasValue
            && (newOpacity.Value < ImageProcessor.MinOpacity || newOpacity.Value > ImageProcessor.MaxOpacity))
            {
                throw new ArgumentOutOfRangeException("newOpacity",
                string.Format("Opacity must be between {0} and {1}.", ImageProcessor.MinOpacity, ImageProcessor.MaxOpacity));
            }

            int pixelsCount = image.PixelWidth * image.PixelHeight;

            int[] pixels = new int[pixelsCount];

            int stride = (image.PixelWidth * image.Format.BitsPerPixel + 7) / 8;

            image.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixelsCount; i++)
            {
                int alpha = (pixels[i] >> 24) & 255;
                int red = (pixels[i] >> 16) & 255;
                int green = (pixels[i] >> 8) & 255;
                int blue = pixels[i] & 255;

                // Adjust brightness
                if (newBrightness.HasValue)
                {
                    red = ImageProcessor.ChangeColorBrightness(red, newBrightness.Value);
                    green = ImageProcessor.ChangeColorBrightness(green, newBrightness.Value);
                    blue = ImageProcessor.ChangeColorBrightness(blue, newBrightness.Value);
                }

                // Adjust contrast
                if (newContrast.HasValue)
                {
                    red = ImageProcessor.ChangeColorContrast(red, newContrast.Value);
                    green = ImageProcessor.ChangeColorContrast(green, newContrast.Value);
                    blue = ImageProcessor.ChangeColorContrast(blue, newContrast.Value);
                }

                // Adjust opacity
                if (newOpacity.HasValue)
                {
                    alpha = newOpacity.Value & 255;
                }

                int color = (alpha << 24) + (red << 16) + (green << 8) + blue;

                pixels[i] = color;
            }

            BitmapSource result = BitmapSource.Create(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY,
            PixelFormats.Bgra32, image.Palette, pixels, stride);

            return result;
        }

        private static int ChangeColorBrightness(int color, int brightness)
        {
            color += (int)(brightness * 2.55);

            return ImageProcessor.AdjustColorValue(color);
        }

        private static int ChangeColorContrast(int color, int newContrast)
        {
            double newColor = color / 255.0;

            // Convert contrast value from [-100, 100] to [0.0, 4.0];
            double contrast = (100 + newContrast) / 100.0;
            contrast *= contrast;

            newColor = newColor - 0.5;
            newColor = newColor * contrast;
            newColor = newColor + 0.5;
            newColor = newColor * 255;

            return ImageProcessor.AdjustColorValue((int)newColor);
        }
    }
}