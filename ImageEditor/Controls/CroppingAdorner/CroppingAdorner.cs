namespace ImageEditor.Controls.CroppingAdorner
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class CroppingAdorner : Adorner
    {
        // Width of the thumbs.  I know these really aren't "pixels", but px
        // is still a good mnemonic.
        private const int _cpxThumbWidth = 6;

        public static readonly RoutedEvent CropChangedEvent = EventManager.RegisterRoutedEvent("CropChanged",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CroppingAdorner));

        public static DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(CroppingAdorner));

        // Canvas to hold the thumbs so they can be moved in response to the user
        private readonly Canvas _cnvThumbs;

        private readonly CropThumb _crtBottom;

        private readonly CropThumb _crtBottomLeft;

        private readonly CropThumb _crtBottomRight;

        private readonly CropThumb _crtLeft;

        private readonly CropThumb _crtRight;

        private readonly CropThumb _crtTop;

        private readonly CropThumb _crtTopLeft;

        private readonly CropThumb _crtTopRight;

        // PuncturedRect to hold the "Cropping" portion of the adorner
        private readonly PuncturedRect _prCropMask;

        // To store and manage the adorner's visual children.
        private readonly VisualCollection _vc;

        static CroppingAdorner()
        {
            Color clr = Colors.Red;
            clr.A = 80;

            CroppingAdorner.FillProperty.OverrideMetadata(typeof(CroppingAdorner),
                new PropertyMetadata(new SolidColorBrush(clr), CroppingAdorner.FillPropChanged));
        }

        public CroppingAdorner(UIElement adornedElement, Rect rcInit)
            : base(adornedElement)
        {
            this._vc = new VisualCollection(this);
            this._prCropMask = new PuncturedRect();
            this._prCropMask.IsHitTestVisible = false;
            this._prCropMask.RectInterior = rcInit;
            this._prCropMask.Fill = this.Fill;
            this._vc.Add(this._prCropMask);
            this._cnvThumbs = new Canvas();
            this._cnvThumbs.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._cnvThumbs.VerticalAlignment = VerticalAlignment.Stretch;

            this._vc.Add(this._cnvThumbs);
            this.BuildCorner(ref this._crtTop, Cursors.SizeNS);
            this.BuildCorner(ref this._crtBottom, Cursors.SizeNS);
            this.BuildCorner(ref this._crtLeft, Cursors.SizeWE);
            this.BuildCorner(ref this._crtRight, Cursors.SizeWE);
            this.BuildCorner(ref this._crtTopLeft, Cursors.SizeNWSE);
            this.BuildCorner(ref this._crtTopRight, Cursors.SizeNESW);
            this.BuildCorner(ref this._crtBottomLeft, Cursors.SizeNESW);
            this.BuildCorner(ref this._crtBottomRight, Cursors.SizeNWSE);

            // Add handlers for Cropping.
            this._crtBottomLeft.DragDelta += this.HandleBottomLeft;
            this._crtBottomRight.DragDelta += this.HandleBottomRight;
            this._crtTopLeft.DragDelta += this.HandleTopLeft;
            this._crtTopRight.DragDelta += this.HandleTopRight;
            this._crtTop.DragDelta += this.HandleTop;
            this._crtBottom.DragDelta += this.HandleBottom;
            this._crtRight.DragDelta += this.HandleRight;
            this._crtLeft.DragDelta += this.HandleLeft;

            // We have to keep the clipping interior withing the bounds of the adorned element
            // so we have to track it's size to guarantee that...
            FrameworkElement fel = adornedElement as FrameworkElement;

            if (fel != null)
            {
                fel.SizeChanged += this.AdornedElement_SizeChanged;
            }
        }

        public Rect ClippingRectangle
        {
            get
            {
                return this._prCropMask.RectInterior;
            }
        }

        public Brush Fill
        {
            get
            {
                return (Brush)this.GetValue(CroppingAdorner.FillProperty);
            }
            set
            {
                this.SetValue(CroppingAdorner.FillProperty, value);
            }
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount
        {
            get
            {
                return this._vc.Count;
            }
        }

        public event RoutedEventHandler CropChanged
        {
            add
            {
                this.AddHandler(CroppingAdorner.CropChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(CroppingAdorner.CropChangedEvent, value);
            }
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect rcExterior = new Rect(0, 0, this.AdornedElement.RenderSize.Width, this.AdornedElement.RenderSize.Height);
            this._prCropMask.RectExterior = rcExterior;
            Rect rcInterior = this._prCropMask.RectInterior;
            this._prCropMask.Arrange(rcExterior);

            this.SetThumbs(rcInterior);
            this._cnvThumbs.Arrange(rcExterior);
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._vc[index];
        }

        private void AdornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement fel = sender as FrameworkElement;

            Rect rcInterior = this._prCropMask.RectInterior;

            bool fFixupRequired = false;

            double intLeft = rcInterior.Left, intTop = rcInterior.Top, intWidth = rcInterior.Width,
                intHeight = rcInterior.Height;

            if (rcInterior.Left > fel.RenderSize.Width)
            {
                intLeft = fel.RenderSize.Width;
                intWidth = 0;
                fFixupRequired = true;
            }

            if (rcInterior.Top > fel.RenderSize.Height)
            {
                intTop = fel.RenderSize.Height;
                intHeight = 0;
                fFixupRequired = true;
            }

            if (rcInterior.Right > fel.RenderSize.Width)
            {
                intWidth = Math.Max(0, fel.RenderSize.Width - intLeft);
                fFixupRequired = true;
            }

            if (rcInterior.Bottom > fel.RenderSize.Height)
            {
                intHeight = Math.Max(0, fel.RenderSize.Height - intTop);
                fFixupRequired = true;
            }
            if (fFixupRequired)
            {
                this._prCropMask.RectInterior = new Rect(intLeft, intTop, intWidth, intHeight);
            }
        }

        private void BuildCorner(ref CropThumb crt, Cursor crs)
        {
            if (crt != null)
            {
                return;
            }

            crt = new CropThumb(CroppingAdorner._cpxThumbWidth);

            // Set some arbitrary visual characteristics.
            crt.Cursor = crs;

            this._cnvThumbs.Children.Add(crt);
        }

        private static void FillPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            CroppingAdorner crp = d as CroppingAdorner;

            if (crp != null)
            {
                crp._prCropMask.Fill = (Brush)args.NewValue;
            }
        }

        // Handler for Cropping from the bottom.
        private void HandleBottom(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(0, 0, 0, 1, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the bottom-left.
        private void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(1, 0, -1, 1, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the bottom-right.
        private void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(0, 0, 1, 1, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the left.
        private void HandleLeft(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(1, 0, -1, 0, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the right.
        private void HandleRight(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(0, 0, 1, 0, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Generic handler for Cropping
        private void HandleThumb(double drcL, double drcT, double drcW, double drcH, double dx, double dy)
        {
            Rect rcInterior = this._prCropMask.RectInterior;

            if (rcInterior.Width + drcW * dx < 0)
            {
                dx = -rcInterior.Width / drcW;
            }

            if (rcInterior.Height + drcH * dy < 0)
            {
                dy = -rcInterior.Height / drcH;
            }

            rcInterior = new Rect(rcInterior.Left + drcL * dx, rcInterior.Top + drcT * dy, rcInterior.Width + drcW * dx,
                rcInterior.Height + drcH * dy);

            this._prCropMask.RectInterior = rcInterior;
            this.SetThumbs(this._prCropMask.RectInterior);
            this.RaiseEvent(new RoutedEventArgs(CroppingAdorner.CropChangedEvent, this));
        }

        // Handler for Cropping from the top.
        private void HandleTop(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(0, 1, 0, -1, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the top-left.
        private void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(1, 1, -1, -1, args.HorizontalChange, args.VerticalChange);
            }
        }

        // Handler for Cropping from the top-right.
        private void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            if (sender is CropThumb)
            {
                this.HandleThumb(0, 1, 1, -1, args.HorizontalChange, args.VerticalChange);
            }
        }

        private void SetThumbs(Rect rc)
        {
            this._crtBottomRight.SetPos(rc.Right, rc.Bottom);
            this._crtTopLeft.SetPos(rc.Left, rc.Top);
            this._crtTopRight.SetPos(rc.Right, rc.Top);
            this._crtBottomLeft.SetPos(rc.Left, rc.Bottom);
            this._crtTop.SetPos(rc.Left + rc.Width / 2, rc.Top);
            this._crtBottom.SetPos(rc.Left + rc.Width / 2, rc.Bottom);
            this._crtLeft.SetPos(rc.Left, rc.Top + rc.Height / 2);
            this._crtRight.SetPos(rc.Right, rc.Top + rc.Height / 2);
        }

        #region Nested type: CropThumb

        private class CropThumb : Thumb
        {
            #region Private variables

            private readonly int _cpx;

            #endregion

            #region Constructor

            internal CropThumb(int cpx)
            {
                this._cpx = cpx;
            }

            #endregion

            #region Positioning

            internal void SetPos(double x, double y)
            {
                Canvas.SetTop(this, y - (double)this._cpx / 2);
                Canvas.SetLeft(this, x - (double)this._cpx / 2);
            }

            #endregion

            #region Overrides

            protected override Visual GetVisualChild(int index)
            {
                return null;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                drawingContext.DrawRoundedRectangle(Brushes.White, new Pen(Brushes.Black, 1),
                    new Rect(new Size(this._cpx, this._cpx)), 1, 1);
            }

            #endregion
        }

        #endregion
    }
}