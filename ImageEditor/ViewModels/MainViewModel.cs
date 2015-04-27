namespace ImageEditor.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
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

        private void InitViewModels()
        {
            this.EditorViewModel = new EditorViewModel();
            this.FooterViewModel = new FooterViewModel();
            this.LeftPanelViewModel = new LeftPanelViewModel();
            this.TopPanelViewModel = new TopPanelViewModel();
        }
    }
}