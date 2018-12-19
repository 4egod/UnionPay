using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace UnionPay.Controls
{
    public sealed class CardTextBox : TextBox
    {
        public CardTextBox()
        {
            DefaultStyleKey = typeof(TextBox);
            MaxLength = 19;
            PlaceholderText = "8600 1234 5678 9999";
            CardNumber = string.Empty;

            RegisterPropertyChangedCallback(TextProperty, new DependencyPropertyChangedCallback((DependencyObject obj, DependencyProperty dp) =>
            {
                if (Text!=null)
                {
                    CardNumber = Text.Replace(" ", "");
                }                
            }));
        }

        public string CardNumber
        {
            get { return ((string)GetValue(CardNumberProperty)).Trim(); }
            set { SetValue(CardNumberProperty, value); }
        }

        public static readonly DependencyProperty CardNumberProperty =
            DependencyProperty.Register("CardNumber", typeof(string), typeof(CardTextBox), new PropertyMetadata(string.Empty));

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (Text.Length == 4 && SelectionStart == 4 && e.Key != VirtualKey.Back)
            {
                Text += " ";
                SelectionStart = 5;
            }

            if (Text.Length == 9 && SelectionStart == 9 && e.Key != VirtualKey.Back)
            {
                Text += " ";
                SelectionStart = 10;
            }

            if (Text.Length == 14 && SelectionStart == 14 && e.Key != VirtualKey.Back)
            {
                Text += " ";
                SelectionStart = 15;
            }

            if (SelectionStart >= 1 && e.Key == VirtualKey.Back && Text[SelectionStart - 1] == ' ')
            {
                int selStart = SelectionStart;
                Text = Text.Remove(selStart - 1);
                SelectionStart = selStart - 1;
            }
        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);

            base.SelectAll();
        }
    }
}
