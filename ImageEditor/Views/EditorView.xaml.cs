namespace ImageEditor.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using ImageEditor.ViewModels;

    /// <summary>
    ///     Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        public EditorView()
        {
            this.InitializeComponent();
        }

        private void Zoom_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;

                if (scrollViewer != null)
                {
                    EditorViewModel viewModel = scrollViewer.DataContext as EditorViewModel;

                    if (viewModel != null)
                    {
                        if (e.Delta > 0)
                        {
                            viewModel.Commands.IncreaseScaleValueCommand.Execute(null);
                        }
                        else if (e.Delta < 0)
                        {
                            viewModel.Commands.ReduceScaleValueCommand.Execute(null);
                        }

                        e.Handled = true;
                    }
                }
            }
        }
    }
}