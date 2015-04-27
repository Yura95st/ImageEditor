namespace ImageEditor.Components.ImageProcessor.Concrete
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    using ImageEditor.Components.ImageProcessor.Abstract;

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
            throw new System.NotImplementedException();
        }

        public BitmapSource Crop(BitmapSource image, Point leftTopCornerPoint, int width, int height)
        {
            throw new System.NotImplementedException();
        }

        public BitmapSource Rotate(BitmapSource image, int angle)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}