namespace ImageEditor.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using GalaSoft.MvvmLight.Messaging;

    using ImageEditor.Controls.CroppingAdorner;
    using ImageEditor.Messages;
    using ImageEditor.ViewModels;

    /// <summary>
    ///     Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        private readonly SolidColorBrush _croppingAdornerFillBrush;

        private CroppingAdorner _croppingAdorner;

        public EditorView()
        {
            this.InitializeComponent();

            Color color = Colors.Black;
            color.A = 110;

            this._croppingAdornerFillBrush = new SolidColorBrush(color);

            this.Loaded += this.OnLoaded;
        }

        private void AddCroppingAdornerToElement(FrameworkElement frameworkElement)
        {
            this.RemoveCroppingAdornerFromElement(frameworkElement);

            this._croppingAdorner = new CroppingAdorner(frameworkElement,
                new Rect(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));

            this._croppingAdorner.Fill = this._croppingAdornerFillBrush;

            this._croppingAdorner.CropChanged += this.OnCropChanged;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);

            adornerLayer.Add(this._croppingAdorner);
        }

        private void ImageThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            EditorViewModel viewModel = this.ImageThumb.DataContext as EditorViewModel;

            if (viewModel != null)
            {
                Point newImageLocation = viewModel.ImageLocation;

                if ((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                    {
                        newImageLocation.Offset(0, e.VerticalChange);
                    }
                    else
                    {
                        newImageLocation.Offset(e.HorizontalChange, 0);
                    }
                }
                else
                {
                    newImageLocation.Offset(e.HorizontalChange, e.VerticalChange);
                }

                if (viewModel.Commands.DragCommand.CanExecute(newImageLocation))
                {
                    viewModel.ImageLocation = newImageLocation;
                }
            }
        }

        private void ImageThumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            EditorViewModel viewModel = this.ImageThumb.DataContext as EditorViewModel;

            if (viewModel != null)
            {
                viewModel.Commands.DragCommand.Execute(viewModel.ImageLocation);
            }
        }

        private void ImageThumb_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.RemoveCroppingAdornerFromElement(this.ImageThumb);
        }

        private void ImageThumb_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this._croppingAdorner != null)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                    {
                        Thumb thumb = sender as Thumb;

                        if (thumb != null)
                        {
                            EditorViewModel viewModel = thumb.DataContext as EditorViewModel;

                            if (viewModel != null)
                            {
                                viewModel.Commands.CropCommand.Execute(null);
                            }
                        }

                        break;
                    }

                    case Key.Escape:
                    {
                        this.RemoveCroppingAdornerFromElement(this.ImageThumb);

                        break;
                    }
                }
            }
        }

        private void OnCropChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this._croppingAdorner != null)
            {
                EditorViewModel viewModel = this._croppingAdorner.DataContext as EditorViewModel;

                if (viewModel != null)
                {
                    viewModel.SetCroppingRect(this._croppingAdorner.ClippingRectangle);
                }
            }
        }

        private void OnHideCroppingRectangle(HideCroppingRectangleMessage obj)
        {
            this.RemoveCroppingAdornerFromElement(this.ImageThumb);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SubscribeToMessages();
        }

        private void OnShowCroppingRectangle(ShowCroppingRectangleMessage message)
        {
            this.AddCroppingAdornerToElement(this.ImageThumb);

            this.ImageThumb.Focus();
        }

        private void RemoveCroppingAdornerFromElement(FrameworkElement frameworkElement)
        {
            if (this._croppingAdorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);

                adornerLayer.Remove(this._croppingAdorner);

                this._croppingAdorner.CropChanged -= this.OnCropChanged;

                this._croppingAdorner = null;
            }
        }

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private void SubscribeToMessages()
        {
            Messenger.Default.Register<ShowCroppingRectangleMessage>(this, this.OnShowCroppingRectangle);
            Messenger.Default.Register<HideCroppingRectangleMessage>(this, this.OnHideCroppingRectangle);
        }
    }
}