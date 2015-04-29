namespace ImageEditor.ViewModels
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight.Messaging;

    using ImageEditor.Commands.Concrete;
    using ImageEditor.Components.ImageProcessor.Abstract;
    using ImageEditor.Components.ImageProcessor.Concrete;
    using ImageEditor.Messages;

    public class MainViewModel
    {
        private readonly MainCommands _commands;

        private readonly IImageProcessor _imageProcessor;

        private BitmapSource _openedImage;

        public MainViewModel()
        {
            this._commands = new MainCommands(this);

            this._imageProcessor = new ImageProcessor();

            this._openedImage = null;

            this.InitViewModels();
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
            return false;
        }

        public bool CanSaveAs()
        {
            return false;
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

        public void Open()
        {
            OpenImageMessage message = new OpenImageMessage(this, imageFilePath =>
            {
                if (!string.IsNullOrEmpty(imageFilePath))
                {
                    try
                    {
                        this._openedImage = new BitmapImage(new Uri(imageFilePath, UriKind.Absolute));

                        this.EditorViewModel.Image = this._openedImage;

                        this.LeftPanelViewModel.ResetToDefaults();
                    }
                    catch (FileNotFoundException)
                    {
                        Messenger.Default.Send(new ErrorMessage(this,
                        string.Format("{0}{1}File is not found.", imageFilePath, Environment.NewLine)));
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

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void SaveAs()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void InitViewModels()
        {
            this.EditorViewModel = new EditorViewModel();
            this.FooterViewModel = new FooterViewModel();
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
    }
}