namespace ImageEditor.Commands.Concrete
{
    using System.Windows.Input;

    using GalaSoft.MvvmLight.CommandWpf;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Utils;
    using ImageEditor.ViewModels;

    public class MainCommands : IMainCommands
    {
        private readonly MainViewModel _viewModel;

        private ICommand _changeBrightnessCommand;

        private ICommand _changeContrastCommand;

        private ICommand _changeOpacityCommand;

        private ICommand _changeRotationAngleCommand;

        private ICommand _cropCommand;

        private ICommand _increaseScaleValueCommand;

        private ICommand _openCommand;

        private ICommand _redoCommand;

        private ICommand _reduceScaleValueCommand;

        private ICommand _resetScaleValueToDefaultCommand;

        private ICommand _saveAsCommand;

        private ICommand _saveCommand;

        private ICommand _undoCommand;

        public MainCommands(MainViewModel viewModel)
        {
            Guard.NotNull(viewModel, "viewModel");

            this._viewModel = viewModel;
        }

        #region IMainCommands Members

        public ICommand IncreaseScaleValueCommand
        {
            get
            {
                if (this._increaseScaleValueCommand == null)
                {
                    this._increaseScaleValueCommand = new RelayCommand(this._viewModel.IncreaseScaleValue, () => true);
                }

                return this._increaseScaleValueCommand;
            }
        }

        public ICommand ReduceScaleValueCommand
        {
            get
            {
                if (this._reduceScaleValueCommand == null)
                {
                    this._reduceScaleValueCommand = new RelayCommand(this._viewModel.ReduceScaleValue, () => true);
                }

                return this._reduceScaleValueCommand;
            }
        }

        public ICommand ResetScaleValueToDefaultCommand
        {
            get
            {
                if (this._resetScaleValueToDefaultCommand == null)
                {
                    this._resetScaleValueToDefaultCommand = new RelayCommand(this._viewModel.ResetScaleValueToDefault,
                    () => true);
                }

                return this._resetScaleValueToDefaultCommand;
            }
        }

        public ICommand ChangeBrightnessCommand
        {
            get
            {
                if (this._changeBrightnessCommand == null)
                {
                    this._changeBrightnessCommand = new RelayCommand(this._viewModel.ChangeBrightness,
                    this._viewModel.CanChangeBrightness);
                }

                return this._changeBrightnessCommand;
            }
        }

        public ICommand ChangeContrastCommand
        {
            get
            {
                if (this._changeContrastCommand == null)
                {
                    this._changeContrastCommand = new RelayCommand(this._viewModel.ChangeContrast,
                    this._viewModel.CanChangeContrast);
                }

                return this._changeContrastCommand;
            }
        }

        public ICommand ChangeOpacityCommand
        {
            get
            {
                if (this._changeOpacityCommand == null)
                {
                    this._changeOpacityCommand = new RelayCommand(this._viewModel.ChangeOpacity,
                    this._viewModel.CanChangeOpacity);
                }

                return this._changeOpacityCommand;
            }
        }

        public ICommand ChangeRotationAngleCommand
        {
            get
            {
                if (this._changeRotationAngleCommand == null)
                {
                    this._changeRotationAngleCommand = new RelayCommand(this._viewModel.ChangeRotationAngle,
                    this._viewModel.CanChangeRotationAngle);
                }

                return this._changeRotationAngleCommand;
            }
        }

        public ICommand CropCommand
        {
            get
            {
                if (this._cropCommand == null)
                {
                    this._cropCommand = new RelayCommand(this._viewModel.Crop, this._viewModel.CanCrop);
                }

                return this._cropCommand;
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (this._openCommand == null)
                {
                    this._openCommand = new RelayCommand(this._viewModel.Open, () => true);
                }

                return this._openCommand;
            }
        }

        public ICommand RedoCommand
        {
            get
            {
                if (this._redoCommand == null)
                {
                    this._redoCommand = new RelayCommand(this._viewModel.Redo, this._viewModel.CanRedo);
                }

                return this._redoCommand;
            }
        }

        public ICommand SaveAsCommand
        {
            get
            {
                if (this._saveAsCommand == null)
                {
                    this._saveAsCommand = new RelayCommand(this._viewModel.SaveAs, this._viewModel.CanSaveAs);
                }

                return this._saveAsCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (this._saveCommand == null)
                {
                    this._saveCommand = new RelayCommand(this._viewModel.Save, this._viewModel.CanSave);
                }

                return this._saveCommand;
            }
        }

        public ICommand UndoCommand
        {
            get
            {
                if (this._undoCommand == null)
                {
                    this._undoCommand = new RelayCommand(this._viewModel.Undo, this._viewModel.CanUndo);
                }

                return this._undoCommand;
            }
        }

        #endregion
    }
}