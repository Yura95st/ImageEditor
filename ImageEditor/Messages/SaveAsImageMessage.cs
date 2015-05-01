namespace ImageEditor.Messages
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    public class SaveAsImageMessage : NotificationMessageAction<string>
    {
        public SaveAsImageMessage(object sender, string imageFileName, Action<string> callback)
        : base(sender, "SaveAsImageMessage", callback)
        {
            this.ImageFileName = imageFileName;
        }

        public string ImageFileName
        {
            get;
            private set;
        }
    }
}