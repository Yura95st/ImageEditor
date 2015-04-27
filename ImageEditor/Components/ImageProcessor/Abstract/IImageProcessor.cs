namespace ImageEditor.Components.ImageProcessor.Abstract
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    public interface IImageProcessor
    {
        BitmapSource ChangeBrightness(BitmapSource image, int newBrightness);

        BitmapSource ChangeContrast(BitmapSource image, int newContrast);

        BitmapSource ChangeOpacity(BitmapSource image, int newOpacity);

        BitmapSource Crop(BitmapSource image, Point leftTopCornerPoint, int width, int height);

        BitmapSource Rotate(BitmapSource image, int angle);
    }
}