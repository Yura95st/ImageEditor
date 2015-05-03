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
                this.SetBrightness(value, true);
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
                this.SetContrast(value, true);
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
                this.SetOpacity(value, true);
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
                this.SetRotationAngle(value, true);
            }
        }

        public void ResetToDefaults()
        {
            this.SetBrightness(ImageProcessor.DefaultBrightness);
            this.SetContrast(ImageProcessor.DefaultContrast);
            this.SetOpacity(ImageProcessor.DefaultOpacity);
            this.SetRotationAngle(ImageProcessor.DefaultRotationAngle);
        }

        public void SetBrightness(int newBrightness, bool withChangeCommandExecuting = false)
        {
            if (newBrightness < this.MinBrightness)
            {
                newBrightness = this.MinBrightness;
            }
            else if (newBrightness > this.MaxBrightness)
            {
                newBrightness = this.MaxBrightness;
            }

            if (this._brightness != newBrightness)
            {
                this._brightness = newBrightness;

                if (withChangeCommandExecuting)
                {
                    this._commands.ChangeBrightnessCommand.Execute(null);
                }

                this.RaisePropertyChanged(() => this.Brightness);
            }
        }

        public void SetContrast(int newContrast, bool withChangeCommandExecuting = false)
        {
            if (newContrast < this.MinContrast)
            {
                newContrast = this.MinContrast;
            }
            else if (newContrast > this.MaxContrast)
            {
                newContrast = this.MaxContrast;
            }

            if (this._contrast != newContrast)
            {
                this._contrast = newContrast;

                if (withChangeCommandExecuting)
                {
                    this._commands.ChangeContrastCommand.Execute(null);
                }

                this.RaisePropertyChanged(() => this.Contrast);
            }
        }

        public void SetOpacity(int newOpacity, bool withChangeCommandExecuting = false)
        {
            if (newOpacity < this.MinOpacity)
            {
                newOpacity = this.MinOpacity;
            }
            else if (newOpacity > this.MaxOpacity)
            {
                newOpacity = this.MaxOpacity;
            }

            if (this._opacity != newOpacity)
            {
                this._opacity = newOpacity;

                if (withChangeCommandExecuting)
                {
                    this._commands.ChangeOpacityCommand.Execute(null);
                }

                this.RaisePropertyChanged(() => this.Opacity);
            }
        }

        public void SetRotationAngle(int newRotationAngle, bool withChangeCommandExecuting = false)
        {
            if (newRotationAngle < this.MinRotationAngle)
            {
                newRotationAngle = this.MinRotationAngle;
            }
            else if (newRotationAngle > this.MaxRotationAngle)
            {
                newRotationAngle = this.MaxRotationAngle;
            }

            if (this._rotationAngle != newRotationAngle)
            {
                this._rotationAngle = newRotationAngle;

                if (withChangeCommandExecuting)
                {
                    this._commands.ChangeRotationAngleCommand.Execute(null);
                }

                this.RaisePropertyChanged(() => this.RotationAngle);
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
    }
}