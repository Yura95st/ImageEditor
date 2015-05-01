namespace ImageEditor.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight.Messaging;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Commands.Concrete;
    using ImageEditor.Components.ImageProcessor.Abstract;
    using ImageEditor.Components.ImageProcessor.Concrete;
    using ImageEditor.Messages;
    using ImageEditor.Utils;

    public class MainViewModel
    {
        private readonly MainCommands _commands;

        private readonly IImageProcessor _imageProcessor;

        private BitmapSource _openedImage;

        private string _openedImageFilePath;

        public MainViewModel()
        {
            this._commands = new MainCommands(this);

            this._imageProcessor = new ImageProcessor();

            this._openedImage = null;
            this._openedImageFilePath = "";

            this.InitViewModels();
        }

        public IMainCommands Commands
        {
            get
            {
                return this._commands;
            }
        }

        public EditorViewModel EditorViewModel
        {
            get;
            private set;
        }

        public FooterViewModel FooterViewModel
        {
            get;
            private set;
        }

        public LeftPanelViewModel LeftPanelViewModel
        {
            get;
            private set;
        }

        public TopPanelViewModel TopPanelViewModel
        {
            get;
            private set;
        }

        public bool CanChangeBrightness()
        {
            return false;
        }

        public bool CanChangeContrast()
        {
            return false;
        }

        public bool CanChangeOpacity()
        {
            return this.IsImageOpened();
        }

        public bool CanChangeRotationAngle()
        {
            return false;
        }

        public bool CanCrop()
        {
            return false;
        }

        public bool CanRedo()
        {
            return false;
        }

        public bool CanSave()
        {
            return this.IsImageOpened();
        }

        public bool CanSaveAs()
        {
            return this.IsImageOpened();
        }

        public bool CanUndo()
        {
            return false;
        }

        public void ChangeBrightness()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeContrast()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeOpacity()
        {
            this.EditorViewModel.Image = this._imageProcessor.ChangeOpacity(this._openedImage,
            this.LeftPanelViewModel.Opacity);
        }

        public void ChangeRotationAngle()
        {
            throw new System.NotImplementedException();
        }

        public void Crop()
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseScaleValue()
        {
            this.FooterViewModel.IncreaseScaleValue();
        }

        public void Open()
        {
            OpenImageMessage message = new OpenImageMessage(this, imageFilePath =>
            {
                if (!string.IsNullOrEmpty(imageFilePath))
                {
                    try
                    {
                        this._openedImage = MainViewModel.GetBitmapSourceFromFile(imageFilePath);

                        this._openedImageFilePath = imageFilePath;

                        this.EditorViewModel.Image = this._openedImage;

                        this.LeftPanelViewModel.ResetToDefaults();
                    }
                    catch
                    {
                        Messenger.Default.Send(new ErrorMessage(this,
                        string.Format(
                        "{0}{1}ImageEditor can't read this file.{1}This is not a valid bitmap file or its format is not currently supported.",
                        imageFilePath, Environment.NewLine)));
                    }
                }
            });

            Messenger.Default.Send(message);
        }

        public void Redo()
        {
            throw new System.NotImplementedException();
        }

        public void ReduceScaleValue()
        {
            this.FooterViewModel.ReduceScaleValue();
        }

        public void ResetScaleValueToDefault()
        {
            this.FooterViewModel.ResetScaleValueToDefault();
        }

        public void Save()
        {
            try
            {
                MainViewModel.SaveImageToFile(this.EditorViewModel.Image, this._openedImageFilePath);
            }
            catch
            {
                Messenger.Default.Send(new ErrorMessage(this,
                string.Format("{0}{1}ImageEditor can't save image to the file.", this._openedImageFilePath,
                Environment.NewLine)));
            }
        }

        public void SaveAs()
        {
            SaveAsImageMessage message = new SaveAsImageMessage(this,
            Path.GetFileNameWithoutExtension(this._openedImageFilePath), imageFilePath =>
            {
                try
                {
                    MainViewModel.SaveImageToFile(this.EditorViewModel.Image, imageFilePath);
                }
                catch
                {
                    Messenger.Default.Send(new ErrorMessage(this,
                    string.Format("{0}{1}ImageEditor can't save image to the file.", imageFilePath, Environment.NewLine)));
                }
            });

            Messenger.Default.Send(message);
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void EditorViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.EditorViewModel.Image))
            {
                this.FooterViewModel.ImageWidth = this.EditorViewModel.Image.PixelWidth;
                this.FooterViewModel.ImageHeight = this.EditorViewModel.Image.PixelHeight;
            }
        }

        private void FooterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.FooterViewModel.ScaleValue))
            {
                this.EditorViewModel.SetImageScaleRatio(this.FooterViewModel.ScaleValue / 100);
            }
        }

        private static BitmapSource GetBitmapSourceFromFile(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

            bitmapImage.EndInit();

            return bitmapImage;
        }

        private void InitViewModels()
        {
            this.EditorViewModel = new EditorViewModel(this._commands);
            this.EditorViewModel.PropertyChanged += this.EditorViewModelOnPropertyChanged;

            this.FooterViewModel = new FooterViewModel();
            this.FooterViewModel.PropertyChanged += this.FooterViewModel_PropertyChanged;

            this.TopPanelViewModel = new TopPanelViewModel(this._commands);

            this.LeftPanelViewModel = new LeftPanelViewModel(this._commands, ImageProcessor.MinBrightness,
            ImageProcessor.MaxBrightness, ImageProcessor.MinContrast, ImageProcessor.MaxContrast, ImageProcessor.MinOpacity,
            ImageProcessor.MaxOpacity, ImageProcessor.MinRotationAngle, ImageProcessor.MaxRotationAngle);
        }

        private bool IsImageOpened()
        {
            bool result = this._openedImage != null;

            return result;
        }

        private static void SaveImageToFile(BitmapSource image, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(filePath)
                .ToLower())
                {
                    case ".bmp":
                    {
                        encoder = new BmpBitmapEncoder();

                        break;
                    }

                    case ".gif":
                    {
                        encoder = new GifBitmapEncoder();

                        break;
                    }

                    case ".jpg":
                    case ".jpeg":
                    {
                        encoder = new JpegBitmapEncoder();

                        break;
                    }

                    case ".png":
                    {
                        encoder = new PngBitmapEncoder();

                        break;
                    }

                    case ".tif":
                    case ".tiff":
                    {
                        encoder = new TiffBitmapEncoder();

                        break;
                    }

                    default:
                    {
                        encoder = new PngBitmapEncoder();
                        break;
                    }
                }

                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}