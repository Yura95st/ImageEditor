namespace ImageEditor.Messages
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    public class OpenImageMessage : NotificationMessageAction<string>
    {
        public OpenImageMessage(object sender, Action<string> callback)
        : base(sender, "OpenImageMessage", callback)
        {
        }
    }
}