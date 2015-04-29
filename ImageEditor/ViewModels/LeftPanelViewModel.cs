namespace ImageEditor.ViewModels
{
    using System;

    using GalaSoft.MvvmLight;

    using ImageEditor.Commands.Abstract;
    using ImageEditor.Utils;

    public class LeftPanelViewModel : ObservableObject
    {
        private readonly ILeftPanelCommands _commands;

        private int _brightness;

        private int _contrast;

        private int _opacity;

        private int _rotationAngle;

        public LeftPanelViewModel(ILeftPanelCommands commands, int minBrightness, int maxBrightness, int minContrast,
                                  int maxContrast, int minOpacity, int maxOpacity, int minRotationAngle, int maxRotationAngle)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;

            this.SubscribeToCommandsCanExecuteChanged();

            this.MinBrightness = (minBrightness > maxBrightness) ? maxBrightness : minBrightness;
            this.MaxBrightness = maxBrightness;

            this.MinContrast = (minContrast > maxContrast) ? maxContrast : minContrast;
            this.MaxContrast = maxContrast;

            this.MinOpacity = (minOpacity > maxOpacity) ? maxOpacity : minOpacity;
            this.MaxOpacity = maxOpacity;

            this.MinRotationAngle = (minRotationAngle > maxRotationAngle) ? maxRotationAngle : minRotationAngle;
            this.MaxRotationAngle = maxRotationAngle;

            this.Brightness = 0;
            this.Contrast = 0;
            this.Opacity = 0;
            this.RotationAngle = 0;
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
            get;
            private set;
        }

        public int MaxContrast
        {
            get;
            private set;
        }

        public int MaxOpacity
        {
            get;
            private set;
        }

        public int MaxRotationAngle
        {
            get;
            private set;
        }

        public int MinBrightness
        {
            get;
            private set;
        }

        public int MinContrast
        {
            get;
            private set;
        }

        public int MinOpacity
        {
            get;
            private set;
        }

        public int MinRotationAngle
        {
            get;
            private set;
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

        private void UnsubscribeToCommandsCanExecuteChanged()
        {
            this._commands.ChangeBrightnessCommand.CanExecuteChanged -= this.ChangeBrightnessCommandOnCanExecuteChanged;
            this._commands.ChangeContrastCommand.CanExecuteChanged -= this.ChangeContrastCommandOnCanExecuteChanged;
            this._commands.ChangeOpacityCommand.CanExecuteChanged -= this.ChangeOpacityCommandOnCanExecuteChanged;
            this._commands.ChangeRotationAngleCommand.CanExecuteChanged -= this.ChangeRotationAngleCommandOnCanExecuteChanged;
        }

        ~LeftPanelViewModel()
        {
            this.UnsubscribeToCommandsCanExecuteChanged();
        }
    }
}