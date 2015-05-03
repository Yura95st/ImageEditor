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
        public static readonly int DefaultBrightness;

        public static readonly int DefaultContrast;

        public static readonly int DefaultOpacity;

        public static readonly int DefaultRotationAngle;

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
            ImageProcessor.DefaultBrightness = 0;

            ImageProcessor.MinContrast = -100;
            ImageProcessor.MaxContrast = 100;
            ImageProcessor.DefaultContrast = 0;

            ImageProcessor.MinOpacity = 0;
            ImageProcessor.MaxOpacity = 255;
            ImageProcessor.DefaultOpacity = ImageProcessor.MaxOpacity;

            ImageProcessor.MinRotationAngle = -180;
            ImageProcessor.MaxRotationAngle = 180;
            ImageProcessor.DefaultRotationAngle = 0;
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
            Guard.NotNull(image, "image");

            if (angle < ImageProcessor.MinRotationAngle || angle > ImageProcessor.MaxRotationAngle)
            {
                throw new ArgumentOutOfRangeException("angle",
                string.Format("Angle must be between {0} and {1}.", ImageProcessor.MinRotationAngle,
                ImageProcessor.MaxRotationAngle));
            }

            int oldWidth = image.PixelWidth;
            int oldHeight = image.PixelHeight;

            int bitsPerPixel = image.Format.BitsPerPixel;

            int stride = (oldWidth * bitsPerPixel + 7) / 8;

            byte[] oldImage = new byte[oldHeight * stride];

            image.CopyPixels(oldImage, stride, 0);

            // Convert angle from degrees to radians
            double angleInRadian = ImageProcessor.ConvertDegreeAngleToRadian(angle);

            // Calculate centerPoint of the image to rotate by
            Point rotationCenterPoint = new Point(Math.Floor(oldWidth / 2.0), Math.Floor(oldHeight / 2.0));

            Point topLeftPoint = new Point(0, 0);
            Point bottomLeftPoint = new Point(0, oldHeight);

            Point newTopLeftPoint = ImageProcessor.RotatePoint(topLeftPoint, rotationCenterPoint, angleInRadian);
            Point newBottomLeftPoint = ImageProcessor.RotatePoint(bottomLeftPoint, rotationCenterPoint, angleInRadian);

            // Calculate newWidth and newHeight values
            int newWidth =
            (int)
            Math.Max(Math.Abs(newTopLeftPoint.X - rotationCenterPoint.X),
            Math.Abs(newBottomLeftPoint.X - rotationCenterPoint.X)) * 2;

            int newHeight =
            (int)
            Math.Max(Math.Abs(newTopLeftPoint.Y - rotationCenterPoint.Y),
            Math.Abs(newBottomLeftPoint.Y - rotationCenterPoint.Y)) * 2;

            int newStride = (newWidth * bitsPerPixel + 7) / 8;

            // Calculate centerPoint of the newImage
            Point newRotationCenterPoint = new Point(Math.Floor(newWidth / 2.0), Math.Floor(newHeight / 2.0));

            byte[] newImage = new byte[newHeight * newStride];

            for (int i = 0; i < oldWidth; i++)
            {
                for (int j = 0; j < oldHeight; j++)
                {
                    Point point = new Point(i, j);

                    Point rotatedPoint = ImageProcessor.RotatePoint(point, rotationCenterPoint, angleInRadian);

                    rotatedPoint.X += newRotationCenterPoint.X - rotationCenterPoint.X;
                    rotatedPoint.Y += newRotationCenterPoint.Y - rotationCenterPoint.Y;

                    if (rotatedPoint.X >= 0 && rotatedPoint.X < newWidth && rotatedPoint.Y >= 0 && rotatedPoint.Y < newHeight)
                    {
                        int index = ImageProcessor.GetPixelIndex(point, bitsPerPixel, stride);
                        int newIndex = ImageProcessor.GetPixelIndex(rotatedPoint, bitsPerPixel, newStride);

                        if (newIndex >= 0)
                        {
                            if (newIndex + 7 < newImage.Length)
                            {
                                newImage[newIndex + 4] = oldImage[index];
                                newImage[newIndex + 5] = oldImage[index + 1];
                                newImage[newIndex + 6] = oldImage[index + 2];
                                newImage[newIndex + 7] = oldImage[index + 3];
                            }

                            if (newIndex + 3 < newImage.Length)
                            {
                                newImage[newIndex] = oldImage[index];
                                newImage[newIndex + 1] = oldImage[index + 1];
                                newImage[newIndex + 2] = oldImage[index + 2];
                                newImage[newIndex + 3] = oldImage[index + 3];
                            }
                        }
                    }
                }
            }

            BitmapSource result = BitmapSource.Create(newWidth, newHeight, image.DpiX, image.DpiY, PixelFormats.Bgra32,
            image.Palette, newImage, newStride);

            return result;
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
                    alpha = ImageProcessor.ChangeColorOpacity(alpha, newOpacity.Value);
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
            // Convert contrast value from [-100, 100] to [0.0, 4.0];
            double contrastLevel = (100 + newContrast) / 100.0;
            contrastLevel *= contrastLevel;

            int newColor = (int)(((((color / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0);

            return ImageProcessor.AdjustColorValue(newColor);
        }

        private static int ChangeColorOpacity(int color, int opacity)
        {
            color -= 255 - opacity;

            return ImageProcessor.AdjustColorValue(color);
        }

        private static double ConvertDegreeAngleToRadian(int angle)
        {
            if (angle < 0)
            {
                angle += 360;
            }

            double angleInRadian = angle * Math.PI / 180;

            return angleInRadian;
        }

        private static int GetPixelIndex(Point point, int bitsPerPixel, int stride)
        {
            int index = ((int)point.X * bitsPerPixel + 7) / 8 + (int)point.Y * stride;

            return index;
        }

        private static Point RotatePoint(Point pointToRotate, Point rotationCenterPoint, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            Point newPoint = new Point
            {
                X =
                    rotationCenterPoint.X
                    + Math.Round((pointToRotate.X - rotationCenterPoint.X) * cos
                    + (pointToRotate.Y - rotationCenterPoint.Y) * sin),
                Y =
                    rotationCenterPoint.Y
                    + Math.Round((pointToRotate.Y - rotationCenterPoint.Y) * cos
                    - (pointToRotate.X - rotationCenterPoint.X) * sin)
            };


            return newPoint;
        }
    }
}