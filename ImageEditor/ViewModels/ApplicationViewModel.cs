namespace ImageEditor.ViewModels
{
    using System.ComponentModel;
    using System.IO;

    using GalaSoft.MvvmLight;

    using ImageEditor.Utils;

    public class ApplicationViewModel : ObservableObject
    {
        public ApplicationViewModel()
        {
            this.MainViewModel = new MainViewModel();

            this.MainViewModel.PropertyChanged += this.MainViewModelOnPropertyChanged;
        }

        public MainViewModel MainViewModel
        {
            get;
            private set;
        }

        public string Title
        {
            get
            {
                string title = "ImageEditor";

                if (!string.IsNullOrEmpty(this.MainViewModel.OpenedImageFilePath))
                {
                    title = title.Insert(0, string.Format("{0} - ", Path.GetFileName(this.MainViewModel.OpenedImageFilePath)));
                }

                return title;
            }
        }

        private void MainViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.MainViewModel.OpenedImageFilePath))
            {
                this.RaisePropertyChanged(() => this.Title);
            }
        }
    }
}