namespace ImageEditor.ViewModels
{
    using ImageEditor.Commands;
    using ImageEditor.Utils;

    public class TopPanelViewModel
    {
        private readonly ITopPanelCommands _commands;

        public TopPanelViewModel(ITopPanelCommands commands)
        {
            Guard.NotNull(commands, "commands");

            this._commands = commands;
        }

        public ITopPanelCommands Commands
        {
            get
            {
                return this._commands;
            }
        }
    }
}