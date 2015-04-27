namespace ImageEditor
{
    using System;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;

    using ImageEditor.Utils;
    using ImageEditor.ViewModels;
    using ImageEditor.Views;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationViewModel _applicationViewModel;

        private void App_OnDispatcherUnhandledException(object sender,
                                                        DispatcherUnhandledExceptionEventArgs
                                                        dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;

            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.AppendLine("Wow ... we didn't expect You to do that.");
            messageBuilder.AppendLine(
            "Please send us the exception, with information on what You were doing at the time. Since this was unexpected, the application is now in an inconsistent state.");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("The error details are:");

            Exception exception = dispatcherUnhandledExceptionEventArgs.Exception;

            messageBuilder.AppendLine(ExceptionHelper.GetFullExceptionMessage(exception));

            messageBuilder.AppendLine();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("The stack trace:");
            messageBuilder.AppendLine(dispatcherUnhandledExceptionEventArgs.Exception.StackTrace);

            string message = messageBuilder.ToString();

            MessageBoxResult result = MessageBox.Show(message, "Unhandled exception", MessageBoxButton.OK,
            MessageBoxImage.Error);

            if (result == MessageBoxResult.OK)
            {
                this.Shutdown();
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this._applicationViewModel = new ApplicationViewModel();

            ApplicationView applicationView = new ApplicationView { DataContext = this._applicationViewModel };

            applicationView.Show();
        }
    }
}