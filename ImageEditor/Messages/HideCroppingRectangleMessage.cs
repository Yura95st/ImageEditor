namespace ImageEditor.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class HideCroppingRectangleMessage : NotificationMessageAction<string>
    {
        public HideCroppingRectangleMessage(object sender)
            : base(sender, "HideCroppingRectangleMessage", callback =>
            {
            })
        {
        }
    }
}