namespace ImageEditor.Controls.NumericUpDownSlider
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class NumericUpDownSlider : UserControl
    {
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double),
        typeof(NumericUpDownSlider), new PropertyMetadata(100.0));

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double),
        typeof(NumericUpDownSlider), new PropertyMetadata(0.0));

        public static DependencyProperty TickFrequencyProperty = DependencyProperty.Register("TickFrequency",
        typeof(double), typeof(NumericUpDownSlider), new PropertyMetadata(1.0));

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double),
        typeof(NumericUpDownSlider), new PropertyMetadata(0.0));

        public NumericUpDownSlider()
        {
            this.InitializeComponent();
        }

        public double Maximum
        {
            get
            {
                return (double)this.GetValue(NumericUpDownSlider.MaximumProperty);
            }
            set
            {
                this.SetValue(NumericUpDownSlider.MaximumProperty, value);

                this.ValidateMinAndMaxValues();
                this.UpdateSliderAndTextBoxValues();
            }
        }

        public double Minimum
        {
            get
            {
                return (double)this.GetValue(NumericUpDownSlider.MinimumProperty);
            }
            set
            {
                this.SetValue(NumericUpDownSlider.MinimumProperty, value);

                this.ValidateMinAndMaxValues();
                this.UpdateSliderAndTextBoxValues();
            }
        }

        public double TickFrequency
        {
            get
            {
                return (double)this.GetValue(NumericUpDownSlider.TickFrequencyProperty);
            }
            set
            {
                this.SetValue(NumericUpDownSlider.TickFrequencyProperty, value);

                this.UpdateSliderAndTextBoxValues();
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
                if (value > this.Maximum)
                {
                    this.SetValue(NumericUpDownSlider.ValueProperty, this.Maximum);
                }
                else if (value < this.Minimum)
                {
                    this.SetValue(NumericUpDownSlider.ValueProperty, this.Minimum);
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
            this.Value -= this.TickFrequency;
        }

        private void IncreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value += this.TickFrequency;
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
            this.IncreaseButton.IsEnabled = this.Value < this.Maximum;
            this.DecreaseButton.IsEnabled = this.Value > this.Minimum;
        }

        private void UpdateSliderAndTextBoxValues()
        {
            this.Slider.Maximum = this.Maximum;
            this.Slider.Minimum = this.Minimum;
            this.Slider.TickFrequency = this.TickFrequency;

            this.Slider.Value = this.Value;

            this.TextBox.Text = this.Value.ToString();
        }

        private void ValidateMinAndMaxValues()
        {
            if (this.Minimum > this.Maximum)
            {
                this.Minimum = this.Maximum;
            }
        }
    }
}