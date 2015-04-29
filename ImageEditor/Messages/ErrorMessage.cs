namespace ImageEditor.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class ErrorMessage : NotificationMessageAction
    {
        public ErrorMessage(object sender, string description)
        : base(sender, "ErrorMessage", () =>
        {
        })
        {
            this.Description = description;
        }

        public string Description
        {
            get;
            private set;
        }
    }
}