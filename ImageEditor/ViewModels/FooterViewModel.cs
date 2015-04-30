namespace ImageEditor.ViewModels
{
    using GalaSoft.MvvmLight;

    public class FooterViewModel : ObservableObject
    {
        private double _scaleValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FooterViewModel" /> class.
        /// </summary>
        public FooterViewModel()
        {
            this.ScaleMin = 10;
            this.ScaleMax = 400;

            this.ScaleStep = 10;

            this._scaleValue = 100;
        }

        /// <summary>
        ///     Gets the scale maximum.
        /// </summary>
        /// <value>
        ///     The scale maximum.
        /// </value>
        public double ScaleMax
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the scale minimum.
        /// </summary>
        /// <value>
        ///     The scale minimum.
        /// </value>
        public double ScaleMin
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the scale step.
        /// </summary>
        /// <value>
        ///     The scale step.
        /// </value>
        public double ScaleStep
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets or sets the scale value.
        /// </summary>
        /// <value>
        ///     The scale value.
        /// </value>
        public double ScaleValue
        {
            get
            {
                return this._scaleValue;
            }
            set
            {
                if (value != this._scaleValue)
                {
                    this._scaleValue = value;

                    this.RaisePropertyChanged(() => this.ScaleValue);
                }
            }
        }

        /// <summary>
        ///     Increases the scale.
        /// </summary>
        public void IncreaseScaleValue()
        {
            double newScaleValue = this.ScaleValue + this.ScaleStep;

            this.ScaleValue = (newScaleValue > this.ScaleMax) ? this.ScaleMax : newScaleValue;
        }

        /// <summary>
        ///     Reduces the scale.
        /// </summary>
        public void ReduceScaleValue()
        {
            double newScaleValue = this.ScaleValue - this.ScaleStep;

            this.ScaleValue = (newScaleValue < this.ScaleMin) ? this.ScaleMin : newScaleValue;
        }

        /// <summary>
        ///     Resets the scale to default.
        /// </summary>
        public void ResetScaleValueToDefault()
        {
            this.ScaleValue = 100;
        }
    }
}