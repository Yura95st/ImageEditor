namespace ImageEditor.ViewModels
{
    using ImageEditor.Commands;

    public class MainViewModel
    {
        private readonly MainCommands _commands;

        public MainViewModel()
        {
            this._commands = new MainCommands(this);

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
            return true;
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
            this.LeftPanelViewModel = new LeftPanelViewModel();
            this.TopPanelViewModel = new TopPanelViewModel(this._commands);
        }
    }
}