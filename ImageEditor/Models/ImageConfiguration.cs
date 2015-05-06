namespace ImageEditor.Models
{
    using System.Windows;

    public class ImageConfiguration
    {
        public int Brightness
        {
            get;
            set;
        }

        public int Contrast
        {
            get;
            set;
        }

        public Rect CroppingRect
        {
            get;
            set;
        }

        public int Opacity
        {
            get;
            set;
        }

        public int RotationAngle
        {
            get;
            set;
        }
    }
}