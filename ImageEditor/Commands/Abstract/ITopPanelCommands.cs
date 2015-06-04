namespace ImageEditor.Commands.Abstract
{
    using System.Windows.Input;

    public interface ITopPanelCommands
    {
        ICommand CropBackgroundByImageCommand
        {
            get;
        }

        ICommand OpenBackgroundCommand
        {
            get;
        }

        ICommand OpenCommand
        {
            get;
        }

        ICommand RedoCommand
        {
            get;
        }

        ICommand SaveAsCommand
        {
            get;
        }

        ICommand SaveBackgroundAsCommand
        {
            get;
        }

        ICommand SaveCommand
        {
            get;
        }

        ICommand ShowCroppingRectangleCommand
        {
            get;
        }

        ICommand UndoCommand
        {
            get;
        }
    }
}