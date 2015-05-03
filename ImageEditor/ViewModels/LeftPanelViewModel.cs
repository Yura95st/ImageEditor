namespace ImageEditor.ViewModels
{
    using System;

    using GalaSoft.MvvmLight;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Components.ImageProcessor.Concrete;
    using ImageEditor.Utils;

    public class LeftPanelViewModel : ObservableObject
    {
        private readonly ILeftPanelCommands _commands;

        private int _brightness;

        private int _contrast;

        private int _opacity;

        private int _rotationAngle;

        public LeftPanelViewModel(ILeftPanelCommands commands)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;

            this.SubscribeToCommandsCanExecuteChanged();

            this.ResetToDefaults();
        }

        public int Brightness
        {
            get
            {
                return this._brightness;
            }
            set
            {
                if (value < this.MinBrightness)
                {
                    value = this.MinBrightness;
                }
                else if (value > this.MaxBrightness)
                {
                    value = this.MaxBrightness;
                }

                if (this._brightness != value)
                {
                    this._brightness = value;

                    this._commands.ChangeBrightnessCommand.Execute(null);

                    this.RaisePropertyChanged(() => this.Brightness);
                }
            }
        }

        public bool CanChangeBrightness
        {
            get
            {
                return this._commands.ChangeBrightnessCommand.CanExecute(null);
            }
        }

        public bool CanChangeContrast
        {
            get
            {
                return this._commands.ChangeContrastCommand.CanExecute(null);
            }
        }

        public bool CanChangeOpacity
        {
            get
            {
                return this._commands.ChangeOpacityCommand.CanExecute(null);
            }
        }

        public bool CanChangeRotationAngle
        {
            get
            {
                return this._commands.ChangeRotationAngleCommand.CanExecute(null);
            }
        }

        public int Contrast
        {
            get
            {
                return this._contrast;
            }
            set
            {
                if (value < this.MinContrast)
                {
                    value = this.MinContrast;
                }
                else if (value > this.MaxContrast)
                {
                    value = this.MaxContrast;
                }

                if (this._contrast != value)
                {
                    this._contrast = value;

                    this._commands.ChangeContrastCommand.Execute(null);

                    this.RaisePropertyChanged(() => this.Contrast);
                }
            }
        }

        public int MaxBrightness
        {
            get
            {
                return ImageProcessor.MaxBrightness;
            }
        }

        public int MaxContrast
        {
            get
            {
                return ImageProcessor.MaxContrast;
            }
        }

        public int MaxOpacity
        {
            get
            {
                return ImageProcessor.MaxOpacity;
            }
        }

        public int MaxRotationAngle
        {
            get
            {
                return ImageProcessor.MaxRotationAngle;
            }
        }

        public int MinBrightness
        {
            get
            {
                return ImageProcessor.MinBrightness;
            }
        }

        public int MinContrast
        {
            get
            {
                return ImageProcessor.MinContrast;
            }
        }

        public int MinOpacity
        {
            get
            {
                return ImageProcessor.MinOpacity;
            }
        }

        public int MinRotationAngle
        {
            get
            {
                return ImageProcessor.MinRotationAngle;
            }
        }

        public int Opacity
        {
            get
            {
                return this._opacity;
            }
            set
            {
                if (value < this.MinOpacity)
                {
                    value = this.MinOpacity;
                }
                else if (value > this.MaxOpacity)
                {
                    value = this.MaxOpacity;
                }

                if (this._opacity != value)
                {
                    this._opacity = value;

                    this._commands.ChangeOpacityCommand.Execute(null);

                    this.RaisePropertyChanged(() => this.Opacity);
                }
            }
        }

        public int RotationAngle
        {
            get
            {
                return this._rotationAngle;
            }
            set
            {
                if (value < this.MinRotationAngle)
                {
                    value = this.MinRotationAngle;
                }
                else if (value > this.MaxRotationAngle)
                {
                    value = this.MaxRotationAngle;
                }

                if (this._rotationAngle != value)
                {
                    this._rotationAngle = value;

                    this._commands.ChangeRotationAngleCommand.Execute(null);

                    this.RaisePropertyChanged(() => this.RotationAngle);
                }
            }
        }

        public void ResetToDefaults()
        {
            this._brightness = ImageProcessor.DefaultBrightness;
            this._contrast = ImageProcessor.DefaultContrast;
            this._opacity = ImageProcessor.DefaultOpacity;
            this._rotationAngle = ImageProcessor.DefaultRotationAngle;

            this.RaisePropertyChanged(() => this.Brightness);
            this.RaisePropertyChanged(() => this.Contrast);
            this.RaisePropertyChanged(() => this.Opacity);
            this.RaisePropertyChanged(() => this.RotationAngle);
        }

        private void ChangeBrightnessCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.RaisePropertyChanged(() => this.CanChangeBrightness);
        }

        private void ChangeContrastCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.RaisePropertyChanged(() => this.CanChangeContrast);
        }

        private void ChangeOpacityCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.RaisePropertyChanged(() => this.CanChangeOpacity);
        }

        private void ChangeRotationAngleCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.RaisePropertyChanged(() => this.CanChangeRotationAngle);
        }

        private void SubscribeToCommandsCanExecuteChanged()
        {
            this._commands.ChangeBrightnessCommand.CanExecuteChanged += this.ChangeBrightnessCommandOnCanExecuteChanged;
            this._commands.ChangeContrastCommand.CanExecuteChanged += this.ChangeContrastCommandOnCanExecuteChanged;
            this._commands.ChangeOpacityCommand.CanExecuteChanged += this.ChangeOpacityCommandOnCanExecuteChanged;
            this._commands.ChangeRotationAngleCommand.CanExecuteChanged += this.ChangeRotationAngleCommandOnCanExecuteChanged;
        }
    }
}