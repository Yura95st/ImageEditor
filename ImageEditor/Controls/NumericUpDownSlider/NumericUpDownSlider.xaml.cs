namespace ImageEditor.Controls.NumericUpDownSlider
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class NumericUpDownSlider : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double),
        typeof(NumericUpDownSlider), new PropertyMetadata(0.0));

        private double _maximun;

        private double _minimun;

        private double _tickFrequency;

        public NumericUpDownSlider()
        {
            this.InitializeComponent();

            this.Minimun = 0.0;
            this.Maximun = 100.0;
            this.TickFrequency = 1.0;
        }

        public double Maximun
        {
            get
            {
                return this._maximun;
            }
            set
            {
                this._maximun = value;

                this.Slider.Maximum = this._maximun;
            }
        }

        public double Minimun
        {
            get
            {
                return this._minimun;
            }
            set
            {
                this._minimun = value;

                this.Slider.Minimum = this._minimun;
            }
        }

        public double TickFrequency
        {
            get
            {
                return this._tickFrequency;
            }
            set
            {
                this._tickFrequency = (value > 0) ? value : 1.0;

                this.Slider.TickFrequency = this._tickFrequency;
            }
        }

        public double Value
        {
            get
            {
                return (double)this.GetValue(NumericUpDownSlider.ValueProperty);
            }
            set
            {
                if (value > this._maximun)
                {
                    this.SetValue(NumericUpDownSlider.ValueProperty, this._maximun);
                }
                else if (value < this._minimun)
                {
                    this.SetValue(NumericUpDownSlider.ValueProperty, this._minimun);
                }
                else
                {
                    this.SetValue(NumericUpDownSlider.ValueProperty, value);
                }

                this.UpdateSliderAndTextBoxValues();
                this.UpdateIncreaseDecreaseButtons();
            }
        }

        private void DecreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value -= this._tickFrequency;
        }

        private void IncreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value += this._tickFrequency;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ValidateMinAndMaxValues();

            this.UpdateSliderAndTextBoxValues();
            this.UpdateIncreaseDecreaseButtons();
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = this.Slider.Value;
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.TextBox.SelectAll();
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            double newValue;

            if (double.TryParse(this.TextBox.Text, out newValue))
            {
                this.Value = newValue;
            }
            else
            {
                this.Value = 0;
            }
        }

        private void TextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^-,0-9,\.]+");

            e.Handled = regex.IsMatch(e.Text);
        }

        private void UpdateIncreaseDecreaseButtons()
        {
            this.IncreaseButton.IsEnabled = this.Value < this._maximun;
            this.DecreaseButton.IsEnabled = this.Value > this._minimun;
        }

        private void UpdateSliderAndTextBoxValues()
        {
            this.Slider.Value = this.Value;

            this.TextBox.Text = this.Value.ToString();
        }

        private void ValidateMinAndMaxValues()
        {
            if (this._minimun > this._maximun)
            {
                this.Minimun = this._maximun;
            }
        }
    }
}