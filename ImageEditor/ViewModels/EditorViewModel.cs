namespace ImageEditor.ViewModels
{
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    using ImageEditor.Utils;

    public class EditorViewModel : ObservableObject
    {
        private BitmapSource _image;

        public EditorViewModel()
        {
            this._image = null;

            this.ImageHeight = 0;
            this.ImageWidth = 0;
            this.ImageScaleRatio = 1;
        }

        public BitmapSource Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != value)
                {
                    this._image = value;

                    this.UpdateImageHeightAndWidth();

                    this.RaisePropertyChanged(() => this.Image);
                }
            }
        }

        public double ImageHeight
        {
            get;
            private set;
        }

        public double ImageScaleRatio
        {
            get;
            private set;
        }

        public double ImageWidth
        {
            get;
            private set;
        }

        public void SetImageScaleRatio(double imageScaleRatio)
        {
            Guard.GreaterThanZero(imageScaleRatio, "imageScaleRatio");

            this.ImageScaleRatio = imageScaleRatio;

            this.UpdateImageHeightAndWidth();
        }

        private void UpdateImageHeightAndWidth()
        {
            double newImageHeight = 0;
            double newImageWidth = 0;

            if (this._image != null)
            {
                newImageHeight = this._image.PixelHeight * this.ImageScaleRatio;
                newImageWidth = this._image.PixelWidth * this.ImageScaleRatio;
            }

            if (!this.ImageHeight.Equals(newImageHeight))
            {
                this.ImageHeight = newImageHeight;

                this.RaisePropertyChanged(() => this.ImageHeight);
            }

            if (!this.ImageWidth.Equals(newImageWidth))
            {
                this.ImageWidth = newImageWidth;

                this.RaisePropertyChanged(() => this.ImageWidth);
            }
        }
    }
}