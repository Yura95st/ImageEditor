namespace ImageEditor.Commands
{
    using System.Windows.Input;

    public interface ILeftPanelCommands
    {
        ICommand ChangeBrightnessCommand
        {
            get;
        }

        ICommand ChangeContrastCommand
        {
            get;
        }

        ICommand ChangeOpacityCommand
        {
            get;
        }

        ICommand ChangeRotationAngleCommand
        {
            get;
        }
    }
}