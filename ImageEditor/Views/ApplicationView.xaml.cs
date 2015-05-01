namespace ImageEditor.Views
{
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using ImageEditor.Messages;

    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : Window
    {
        public ApplicationView()
        {
            this.InitializeComponent();

            this.Loaded += this.OnLoaded;

            this.Unloaded += this.OnUnloaded;
        }

        private static void OnError(ErrorMessage message)
        {
            MessageBox.Show(message.Description, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.SubscribeToMessages();
        }

        private static void OnOpenImage(OpenImageMessage message)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckPathExists = true, ValidateNames = true,
                Filter =
                    string.Format(
                    "Image files ({0}, {1}, {2}, {3}, {4}, {5}, {6})|{0};{1};{2};{3};{4};{5};{6}|All files (*.*)|*.*",
                    "*.bmp", "*.gif", "*.jpg", "*.jpeg", "*.png", "*.tif", "*.tiff")
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                message.Execute(openFileDialog.FileName);
            }
        }

        private static void OnSaveAsImage(SaveAsImageMessage message)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = message.ImageFileName, AddExtension = true, CheckPathExists = true, OverwritePrompt = true,
                ValidateNames = true, DefaultExt = ".png", FilterIndex = 4,
                Filter =
                    string.Format(
                    "BMP ({0})|{0}|GIF ({1})|{1}|JPEG ({2}, {3})|{2};{3}|PNG ({4})|{4}|TIFF ({5}, {6})|{5};{6}", "*.bmp",
                    "*.gif", "*.jpg", "*.jpeg", "*.png", "*.tif", "*.tiff")
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                message.Execute(saveFileDialog.FileName);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Unregister the view from all messages on unloading
            Messenger.Default.Unregister(this);
        }

        private void SubscribeToMessages()
        {
            Messenger.Default.Register<ErrorMessage>(this, ApplicationView.OnError);
            Messenger.Default.Register<OpenImageMessage>(this, ApplicationView.OnOpenImage);
            Messenger.Default.Register<SaveAsImageMessage>(this, ApplicationView.OnSaveAsImage);
        }
    }
}