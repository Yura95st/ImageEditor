namespace ImageEditor.ViewModels
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
            this.MainViewModel = new MainViewModel();
        }

        public MainViewModel MainViewModel
        {
            get;
            private set;
        }
    }
}