namespace ImageEditor.Commands.Abstract
{
    using System.Windows.Input;

    public interface IEditorCommands
    {
        ICommand CropCommand
        {
            get;
        }

        ICommand DragCommand
        {
            get;
        }

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