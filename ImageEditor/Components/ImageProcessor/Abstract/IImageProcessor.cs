namespace ImageEditor.Components.ImageProcessor.Abstract
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    public interface IImageProcessor
    {
        /// <summary>Changes the brightness of the specified image to the specified value.</summary>
        /// <param name="image">The image.</param>
        /// <param name="newBrightness">The new brightness value.</param>
        /// <returns>The image with new brightness.</returns>
        BitmapSource ChangeBrightness(BitmapSource image, int newBrightness);

        /// <summary>Changes the brightness of the specified image to the specified value.</summary>
        /// <param name="image">The image.</param>
        /// <param name="newContrast">The new contrast value.</param>
        /// <returns>The image with new contrast.</returns>
        BitmapSource ChangeContrast(BitmapSource image, int newContrast);

        /// <summary>Changes the opacity of the specified image to the specified value.</summary>
        /// <param name="image">The image.</param>
        /// <param name="newOpacity">The new opacity value.</param>
        /// <returns>The image with new opacity.</returns>
        BitmapSource ChangeOpacity(BitmapSource image, int newOpacity);

        /// <summary> Cuts out a rectangular sub-image of specified size from specified image starting from the given point.</summary>
        /// <param name="image">The image.</param>
        /// <param name="leftTopCornerPoint">The sub-image's left top corner point.</param>
        /// <param name="width">The width of sub-image.</param>
        /// <param name="height">The height of sub-image.</param>
        /// <returns>A rectangular sub-image.</returns>
        BitmapSource Crop(BitmapSource image, Point leftTopCornerPoint, double width, double height);

        /// <summary>Rotates the specified image to the specified angle.</summary>
        /// <param name="image">The image.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated image.</returns>
        BitmapSource Rotate(BitmapSource image, int angle);
    }
}