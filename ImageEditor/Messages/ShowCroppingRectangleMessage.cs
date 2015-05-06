namespace ImageEditor.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class ShowCroppingRectangleMessage : NotificationMessageAction<string>
    {
        public ShowCroppingRectangleMessage(object sender)
            : base(sender, "ShowCroppingRectangleMessage", callback =>
            {
            })
        {
        }
    }
}