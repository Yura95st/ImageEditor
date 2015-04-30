namespace ImageEditor.ViewModels
{
    using System;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Utils;

    public class EditorViewModel : ObservableObject
    {
        private readonly IEditorCommands _commands;

        private BitmapSource _image;

        private double _imageOpacity;

        public EditorViewModel(IEditorCommands commands)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;

            this._image = null;

            this.ImageHeight = 0;
            this.ImageWidth = 0;

            this.ImageOpacity = 1;

            this.ImageScaleRatio = 1;
        }

        public IEditorCommands Commands
        {
            get
            {
                return this._commands;
            }
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

        public double ImageOpacity
        {
            get
            {
                return this._imageOpacity;
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("value", "Opacity value must be between 0 and 1.");
                }

                this._imageOpacity = value;

                this.RaisePropertyChanged(() => this.ImageOpacity);
            }
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