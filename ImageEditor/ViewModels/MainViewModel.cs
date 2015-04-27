namespace ImageEditor.ViewModels
{
    using ImageEditor.Commands.Concrete;
    using ImageEditor.Components.ImageProcessor.Abstract;
    using ImageEditor.Components.ImageProcessor.Concrete;

    public class MainViewModel
    {
        private readonly MainCommands _commands;

        private readonly IImageProcessor _imageProcessor;

        public MainViewModel()
        {
            this._commands = new MainCommands(this);

            this._imageProcessor = new ImageProcessor();

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
            return false;
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
    }
}