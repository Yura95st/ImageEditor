namespace ImageEditor.Commands.Abstract
{
    using System.Windows.Input;

    public interface ITopPanelCommands
    {
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