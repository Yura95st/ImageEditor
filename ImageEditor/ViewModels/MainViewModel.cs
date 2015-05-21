namespace ImageEditor.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Commands.Concrete;
    using ImageEditor.Components.ImageProcessor.Abstract;
    using ImageEditor.Components.ImageProcessor.Concrete;
    using ImageEditor.Enums;
    using ImageEditor.Messages;
    using ImageEditor.Models;
    using ImageEditor.Services.Abstract;
    using ImageEditor.Utils;

    public class MainViewModel : ObservableObject
    {
        private readonly MainCommands _commands;

        private readonly IImageProcessor _imageProcessor;

        private readonly IUndoRedoService<EditAction> _undoRedoService;

        private BitmapSource _editingImage;

        private string _openedImageFilePath;

        private BitmapSource _originalImage;

        public MainViewModel(IImageProcessor imageProcessor, IUndoRedoService<EditAction> undoRedoService)
        {
            Guard.NotNull(imageProcessor, "imageProcessor");
            Guard.NotNull(undoRedoService, "undoRedoService");

            this._imageProcessor = imageProcessor;
            this._undoRedoService = undoRedoService;

            this._commands = new MainCommands(this);

            this._originalImage = null;
            this._editingImage = null;

            this._openedImageFilePath = null;

            this.InitViewModels();
        }

        public IMainCommands Commands
        {
            get
            {
                return this._commands;
            }
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

        public string OpenedImageFilePath
        {
            get
            {
                return this._openedImageFilePath;
            }
            private set
            {
                if (value != this._openedImageFilePath)
                {
                    this._openedImageFilePath = value;

                    this.RaisePropertyChanged(() => this.OpenedImageFilePath);
                }
            }
        }

        public TopPanelViewModel TopPanelViewModel
        {
            get;
            private set;
        }

        public bool CanChangeBrightness()
        {
            return this.IsImageOpened();
        }

        public bool CanChangeContrast()
        {
            return this.IsImageOpened();
        }

        public bool CanChangeOpacity()
        {
            return this.IsImageOpened();
        }

        public bool CanChangeRotationAngle()
        {
            return this.IsImageOpened();
        }

        public bool CanCrop()
        {
            return this.IsImageOpened();
        }

        public bool CanDrag(Point newLocation)
        {
            if (!this.IsImageOpened())
            {
                return false;
            }

            Rect imageRect = new Rect(newLocation,
                new Size(this.EditorViewModel.ImageWidth, this.EditorViewModel.ImageHeight));
            Rect fieldRect =
                new Rect(new Size(this.EditorViewModel.BackgroundLayerWidth, this.EditorViewModel.BackgroundLayerHeight));

            return imageRect.IntersectsWith(fieldRect);
        }

        public bool CanRedo()
        {
            return this._undoRedoService.CanRedo();
        }

        public bool CanResize()
        {
            return this.IsImageOpened();
        }

        public bool CanSave()
        {
            return this.IsImageOpened();
        }

        public bool CanSaveAs()
        {
            return this.IsImageOpened();
        }

        public bool CanShowCroppingRectangle()
        {
            return this.IsImageOpened();
        }

        public bool CanUndo()
        {
            return this._undoRedoService.CanUndo();
        }

        public void ChangeBrightness()
        {
            this.PerformEditActionWithKind(EditActionKind.ChangeBrightness);
        }

        public void ChangeContrast()
        {
            this.PerformEditActionWithKind(EditActionKind.ChangeContrast);
        }

        public void ChangeOpacity()
        {
            this.PerformEditActionWithKind(EditActionKind.ChangeOpacity);
        }

        public void ChangeRotationAngle()
        {
            this.PerformEditActionWithKind(EditActionKind.Rotate);
        }

        public void Crop()
        {
            this.PerformEditActionWithKind(EditActionKind.Crop);

            Messenger.Default.Send(new HideCroppingRectangleMessage(this));
        }

        public void Drag(Point newLocation)
        {
            this.EditorViewModel.ImageLocation = newLocation;

            this.PerformEditActionWithKind(EditActionKind.Drag);
        }

        public void IncreaseScaleValue()
        {
            this.FooterViewModel.IncreaseScaleValue();
        }

        public void Open()
        {
            OpenImageMessage message = new OpenImageMessage(this, imageFilePath =>
            {
                if (!string.IsNullOrEmpty(imageFilePath))
                {
                    try
                    {
                        this.Reset();

                        this._originalImage = ImageHelper.GetBitmapSourceFromFile(imageFilePath);

                        this.OpenedImageFilePath = imageFilePath;

                        this.EditorViewModel.Image = this._originalImage;
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

        public void OpenBackground()
        {
            OpenImageMessage message = new OpenImageMessage(this, imageFilePath =>
            {
                if (!string.IsNullOrEmpty(imageFilePath))
                {
                    try
                    {
                        this.EditorViewModel.BackgroundImage = ImageHelper.GetBitmapSourceFromFile(imageFilePath);
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
            this.UndoOrRedo(false);
        }

        public void ReduceScaleValue()
        {
            this.FooterViewModel.ReduceScaleValue();
        }

        public void ResetScaleValueToDefault()
        {
            this.FooterViewModel.ResetScaleValueToDefault();
        }

        public void Resize()
        {
            this.PerformEditActionWithKind(EditActionKind.Resize);
        }

        public void Save()
        {
            try
            {
                ImageHelper.SaveImageToFile(this.EditorViewModel.Image, this._openedImageFilePath);
            }
            catch
            {
                Messenger.Default.Send(new ErrorMessage(this,
                    string.Format("{0}{1}ImageEditor can't save image to the file.", this._openedImageFilePath,
                        Environment.NewLine)));
            }
        }

        public void SaveAs()
        {
            SaveAsImageMessage message = new SaveAsImageMessage(this,
                Path.GetFileNameWithoutExtension(this._openedImageFilePath), imageFilePath =>
                {
                    try
                    {
                        ImageHelper.SaveImageToFile(this.EditorViewModel.Image, imageFilePath);

                        this.OpenedImageFilePath = imageFilePath;
                    }
                    catch
                    {
                        Messenger.Default.Send(new ErrorMessage(this,
                            string.Format("{0}{1}ImageEditor can't save image to the file.", imageFilePath,
                                Environment.NewLine)));
                    }
                });

            Messenger.Default.Send(message);
        }

        public void ShowCroppingRectangle()
        {
            Messenger.Default.Send(new ShowCroppingRectangleMessage(this));
        }

        public void Undo()
        {
            this.UndoOrRedo(true);
        }

        private void EditImage(BitmapSource imageToEdit, EditAction editAction)
        {
            switch (editAction.Kind)
            {
                case EditActionKind.ChangeBrightness:
                {
                    this.EditorViewModel.Image = this._imageProcessor.ChangeBrightness(imageToEdit,
                        editAction.ImageConfiguration.Brightness);
                    break;
                }

                case EditActionKind.ChangeContrast:
                {
                    this.EditorViewModel.Image = this._imageProcessor.ChangeContrast(imageToEdit,
                        editAction.ImageConfiguration.Contrast);
                    break;
                }

                case EditActionKind.ChangeOpacity:
                {
                    this.EditorViewModel.Image = this._imageProcessor.ChangeOpacity(imageToEdit,
                        editAction.ImageConfiguration.Opacity);
                    break;
                }

                case EditActionKind.Crop:
                {
                    this.EditorViewModel.Image = this._imageProcessor.Crop(imageToEdit,
                        editAction.ImageConfiguration.CroppingRect);
                    break;
                }

                case EditActionKind.Resize:
                {
                    this.EditorViewModel.Image = this._imageProcessor.Resize(imageToEdit, editAction.ImageConfiguration.Width,
                        editAction.ImageConfiguration.Height);
                    break;
                }

                case EditActionKind.Rotate:
                {
                    this.EditorViewModel.Image = this._imageProcessor.Rotate(imageToEdit,
                        editAction.ImageConfiguration.RotationAngle);
                    break;
                }

                case EditActionKind.ResetToInitialState:
                {
                    this.EditorViewModel.Image = imageToEdit;
                    break;
                }
            }
        }

        private void EditorViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.EditorViewModel.Image))
            {
                this.FooterViewModel.ImageWidth = this.EditorViewModel.Image.PixelWidth;
                this.FooterViewModel.ImageHeight = this.EditorViewModel.Image.PixelHeight;

                this.LeftPanelViewModel.SetWidth(this.EditorViewModel.Image.PixelWidth);
                this.LeftPanelViewModel.SetHeight(this.EditorViewModel.Image.PixelHeight);
            }
        }

        private void FooterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExpressionHelper.GetPropertyName(() => this.FooterViewModel.ScaleValue))
            {
                this.EditorViewModel.SetImageScaleRatio(this.FooterViewModel.ScaleValue / 100);
            }
        }

        private Rect GenerateCroppingRect()
        {
            Rect croppingRect = this.EditorViewModel.CroppingRect;

            EditAction lastEditAction = this._undoRedoService.GetLastEntry();

            if (lastEditAction != null)
            {
                croppingRect.Offset(lastEditAction.ImageConfiguration.CroppingRect.X,
                    lastEditAction.ImageConfiguration.CroppingRect.Y);
            }

            return croppingRect;
        }

        private ImageConfiguration GenerateImageConfiguration()
        {
            ImageConfiguration imageConfiguration = new ImageConfiguration
            {
                Brightness = this.LeftPanelViewModel.Brightness, Contrast = this.LeftPanelViewModel.Contrast,
                Opacity = this.LeftPanelViewModel.Opacity, RotationAngle = this.LeftPanelViewModel.RotationAngle,
                Height = this.LeftPanelViewModel.Height, Width = this.LeftPanelViewModel.Width,
                CroppingRect = this.GenerateCroppingRect()
            };

            return imageConfiguration;
        }

        private BitmapSource GenerateImageToEdit(EditAction editAction, EditAction lastEditAction)
        {
            if (editAction.Kind != EditActionKind.ResetToInitialState && lastEditAction != null)
            {
                if (lastEditAction.Kind != editAction.Kind)
                {
                    BitmapSource bitmapSource = this._originalImage;

                    if (editAction.Kind != EditActionKind.ChangeBrightness
                        && editAction.ImageConfiguration.Brightness != ImageProcessor.DefaultBrightness)
                    {
                        bitmapSource = this._imageProcessor.ChangeBrightness(bitmapSource,
                            editAction.ImageConfiguration.Brightness);
                    }

                    if (editAction.Kind != EditActionKind.ChangeContrast
                        && editAction.ImageConfiguration.Contrast != ImageProcessor.DefaultContrast)
                    {
                        bitmapSource = this._imageProcessor.ChangeContrast(bitmapSource,
                            editAction.ImageConfiguration.Contrast);
                    }

                    if (editAction.Kind != EditActionKind.ChangeOpacity
                        && editAction.ImageConfiguration.Opacity != ImageProcessor.DefaultOpacity)
                    {
                        bitmapSource = this._imageProcessor.ChangeOpacity(bitmapSource, editAction.ImageConfiguration.Opacity);
                    }

                    if (editAction.Kind != EditActionKind.Rotate
                        && editAction.ImageConfiguration.RotationAngle != ImageProcessor.DefaultRotationAngle)
                    {
                        bitmapSource = this._imageProcessor.Rotate(bitmapSource, editAction.ImageConfiguration.RotationAngle);
                    }

                    if (editAction.Kind != EditActionKind.Crop)
                    {
                        bitmapSource = this._imageProcessor.Crop(bitmapSource, editAction.ImageConfiguration.CroppingRect);
                    }

                    this._editingImage = bitmapSource;
                }
            }
            else
            {
                this._editingImage = this._originalImage;
            }

            return this._editingImage;
        }

        private void InitViewModels()
        {
            this.EditorViewModel = new EditorViewModel(this._commands);
            this.EditorViewModel.PropertyChanged += this.EditorViewModelOnPropertyChanged;

            this.FooterViewModel = new FooterViewModel();
            this.FooterViewModel.PropertyChanged += this.FooterViewModel_PropertyChanged;

            this.TopPanelViewModel = new TopPanelViewModel(this._commands);

            this.LeftPanelViewModel = new LeftPanelViewModel(this._commands);
        }

        private bool IsImageOpened()
        {
            bool result = this._originalImage != null;

            return result;
        }

        private void PerformEditActionWithKind(EditActionKind editActionKind)
        {
            EditAction editAction = new EditAction(editActionKind, this.GenerateImageConfiguration());

            if (editActionKind != EditActionKind.Drag)
            {
                //BitmapSource imageToEdit = this.GenerateImageToEdit(editAction, this._undoRedoService.GetLastEntry());

                BitmapSource imageToEdit = this._originalImage;

                this.EditImage(imageToEdit, editAction);
            }

            //this._undoRedoService.AddEntry(editAction);
        }

        private void Reset()
        {
            this._editingImage = null;

            this.LeftPanelViewModel.ResetToDefaults();
            this.EditorViewModel.Reset();

            this._undoRedoService.Clear();
        }

        private void UndoOrRedo(bool isUndo)
        {
            EditAction lastEditAction = this._undoRedoService.GetLastEntry();

            if (isUndo)
            {
                this._undoRedoService.Undo();
            }
            else
            {
                this._undoRedoService.Redo();
            }

            EditAction editAction = this._undoRedoService.GetLastEntry();

            if (isUndo && editAction == null)
            {
                editAction = new EditAction(EditActionKind.ResetToInitialState,
                    new ImageConfiguration
                    {
                        Brightness = ImageProcessor.DefaultBrightness, Contrast = ImageProcessor.DefaultContrast,
                        Opacity = ImageProcessor.DefaultOpacity, RotationAngle = ImageProcessor.DefaultRotationAngle
                    });
            }

            BitmapSource imageToEdit = this.GenerateImageToEdit(editAction, lastEditAction);

            this.EditImage(imageToEdit, editAction);

            this.LeftPanelViewModel.SetBrightness(editAction.ImageConfiguration.Brightness);
            this.LeftPanelViewModel.SetContrast(editAction.ImageConfiguration.Contrast);
            this.LeftPanelViewModel.SetOpacity(editAction.ImageConfiguration.Opacity);
            this.LeftPanelViewModel.SetRotationAngle(editAction.ImageConfiguration.RotationAngle);
        }
    }
}