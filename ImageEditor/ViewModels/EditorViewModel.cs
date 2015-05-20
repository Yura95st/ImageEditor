namespace ImageEditor.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Utils;

    public class EditorViewModel : ObservableObject
    {
        private readonly IEditorCommands _commands;

        private BitmapSource _backgroundImage;

        private Rect _croppingRect;

        private BitmapSource _image;

        private Point _imageLocation;

        private Point _realImageLocation;

        public EditorViewModel(IEditorCommands commands)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;

            this._backgroundImage = null;
            this._image = null;

            this.BackgroundImageHeight = 0;
            this.BackgroundImageWidth = 0;

            this.ImageHeight = 0;
            this.ImageWidth = 0;

            this.ImageScaleRatio = 1;

            this._croppingRect = new Rect();
            this._imageLocation = new Point();
            this._realImageLocation = new Point();

            this.PropertyChanged += this.OnPropertyChanged;
        }

        public BitmapSource BackgroundImage
        {
            get
            {
                return this._backgroundImage;
            }
            set
            {
                if (this._backgroundImage != value)
                {
                    this._backgroundImage = value;

                    this.UpdateBackgroundImageHeightAndWidth();

                    this.RaisePropertyChanged(() => this.BackgroundImage);
                }
            }
        }

        public double BackgroundImageHeight
        {
            get;
            private set;
        }

        public double BackgroundImageWidth
        {
            get;
            private set;
        }

        public double BackgroundLayerHeight
        {
            get
            {
                return Math.Max(this.ImageHeight, this.BackgroundImageHeight);
            }
        }

        public double BackgroundLayerWidth
        {
            get
            {
                return Math.Max(this.ImageWidth, this.BackgroundImageWidth);
            }
        }

        public IEditorCommands Commands
        {
            get
            {
                return this._commands;
            }
        }

        public Rect CroppingRect
        {
            get
            {
                return this._croppingRect;
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
                    this.UpdateCroppingRect();
                    this.ImageLocation = new Point();

                    this.RaisePropertyChanged(() => this.Image);
                }
            }
        }

        public double ImageHeight
        {
            get;
            private set;
        }

        public Point ImageLocation
        {
            get
            {
                return this._imageLocation;
            }
            set
            {
                if (this._imageLocation != value)
                {
                    this._imageLocation = value;

                    double ratio = 1 / this.ImageScaleRatio;

                    this._realImageLocation.X = this._imageLocation.X * ratio;
                    this._realImageLocation.Y = this._imageLocation.Y * ratio;

                    this.RaisePropertyChanged(() => this.ImageLocation);
                }
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

        public void SetCroppingRect(Rect croppingRect)
        {
            this._croppingRect.Size = new Size(croppingRect.Width / this.ImageScaleRatio,
                croppingRect.Height / this.ImageScaleRatio);

            this._croppingRect.Location = new Point(croppingRect.X / this.ImageScaleRatio,
                croppingRect.Y / this.ImageScaleRatio);

            this.RaisePropertyChanged(() => this.CroppingRect);
        }

        public void SetImageScaleRatio(double imageScaleRatio)
        {
            Guard.GreaterThanZero(imageScaleRatio, "imageScaleRatio");

            this.ImageScaleRatio = imageScaleRatio;

            this.UpdateBackgroundImageHeightAndWidth();
            this.UpdateImageHeightAndWidth();
            this.UpdateImageLocation();
        }

        private static double GetScaledImageHeight(BitmapSource image, double imageScaleRatio)
        {
            double newImageHeight = 0;

            if (image != null)
            {
                newImageHeight = image.PixelHeight * imageScaleRatio;
            }

            return newImageHeight;
        }

        private static double GetScaledImageWidth(BitmapSource image, double imageScaleRatio)
        {
            double newImageWidth = 0;

            if (image != null)
            {
                newImageWidth = image.PixelWidth * imageScaleRatio;
            }

            return newImageWidth;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.BackgroundImageHeight)
                || e.PropertyName == ExpressionHelper.GetPropertyName(() => this.ImageHeight))
            {
                this.RaisePropertyChanged(() => this.BackgroundLayerHeight);
            }
            else if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.BackgroundImageWidth)
                || e.PropertyName == ExpressionHelper.GetPropertyName(() => this.ImageWidth))
            {
                this.RaisePropertyChanged(() => this.BackgroundLayerWidth);
            }
        }

        private void UpdateBackgroundImageHeightAndWidth()
        {
            double newImageHeight = EditorViewModel.GetScaledImageHeight(this._backgroundImage, this.ImageScaleRatio);
            double newImageWidth = EditorViewModel.GetScaledImageWidth(this._backgroundImage, this.ImageScaleRatio);

            if (!this.BackgroundImageHeight.Equals(newImageHeight))
            {
                this.BackgroundImageHeight = newImageHeight;

                this.RaisePropertyChanged(() => this.BackgroundImageHeight);
            }

            if (!this.BackgroundImageWidth.Equals(newImageWidth))
            {
                this.BackgroundImageWidth = newImageWidth;

                this.RaisePropertyChanged(() => this.BackgroundImageWidth);
            }
        }

        private void UpdateCroppingRect()
        {
            this._croppingRect.Location = new Point(0, 0);
            this._croppingRect.Size = new Size(this._image.PixelWidth, this._image.PixelHeight);

            this.RaisePropertyChanged(() => this.CroppingRect);
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

        private void UpdateImageLocation()
        {
            Point newImageLocation = this._imageLocation;

            newImageLocation.X = this._realImageLocation.X * this.ImageScaleRatio;
            newImageLocation.Y = this._realImageLocation.Y * this.ImageScaleRatio;

            if (!this._imageLocation.Equals(newImageLocation))
            {
                this._imageLocation = newImageLocation;

                this.RaisePropertyChanged(() => this.ImageLocation);
            }
        }
    }
}