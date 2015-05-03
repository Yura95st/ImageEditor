namespace ImageEditor.Models
{
    using ImageEditor.Enums;
    using ImageEditor.Utils;

    public class EditAction
    {
        public EditAction(EditActionKind kind, ImageConfiguration imageConfiguration)
        {
            Guard.NotNull(imageConfiguration, "imageConfiguration");

            this.Kind = kind;
            this.ImageConfiguration = imageConfiguration;
        }

        public ImageConfiguration ImageConfiguration
        {
            get;
            private set;
        }

        public EditActionKind Kind
        {
            get;
            private set;
        }
    }
}