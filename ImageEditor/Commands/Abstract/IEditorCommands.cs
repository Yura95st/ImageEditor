namespace ImageEditor.Commands.Abstract
{
    using System.Windows.Input;

    public interface IEditorCommands
    {
        ICommand IncreaseScaleValueCommand
        {
            get;
        }

        ICommand ReduceScaleValueCommand
        {
            get;
        }

        ICommand ResetScaleValueToDefaultCommand
        {
            get;
        }
    }
}