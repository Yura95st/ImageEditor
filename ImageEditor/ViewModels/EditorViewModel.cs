namespace ImageEditor.ViewModels
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Utils;

    public class EditorViewModel : ObservableObject
    {
        private readonly IEditorCommands _commands;

        private Rect _croppingRect;

        private BitmapSource _image;

        public EditorViewModel(IEditorCommands commands)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;

            this._image = null;

            this.ImageHeight = 0;
            this.ImageWidth = 0;

            this.ImageScaleRatio = 1;
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

            this.UpdateImageHeightAndWidth();
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
    }
}