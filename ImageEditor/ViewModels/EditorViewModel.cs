namespace ImageEditor.ViewModels
{
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    public class EditorViewModel : ObservableObject
    {
        private readonly double _imageScaleRatio;

        private BitmapSource _image;

        private double _imageHeight;

        private double _imageWidth;

        public EditorViewModel()
        {
            this._image = null;

            this._imageHeight = 0;
            this._imageWidth = 0;
            this._imageScaleRatio = 1;
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

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public double ImageHeight
        {
            get
            {
                return this._imageHeight;
            }
        }

        public double ImageWidth
        {
            get
            {
                return this._imageWidth;
            }
        }

        private void UpdateImageHeightAndWidth()
        {
            double newImageHeight = 0;
            double newImageWidth = 0;

            if (this._image != null)
            {
                newImageHeight = this._image.PixelHeight * this._imageScaleRatio;
                newImageWidth = this._image.PixelWidth * this._imageScaleRatio;
            }

            if (!this._imageHeight.Equals(newImageHeight))
            {
                this._imageHeight = newImageHeight;

                this.RaisePropertyChanged(() => this.ImageHeight);
            }

            if (!this._imageWidth.Equals(newImageWidth))
            {
                this._imageWidth = newImageHeight;

                this.RaisePropertyChanged(() => this.ImageWidth);
            }
        }
    }
}