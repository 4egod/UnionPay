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
    public sealed class PhoneTextBox : TextBox
    {
        private const int FullLength = 13;

        private const int ShortLength = 9;

        public PhoneTextBox()
        {
            this.DefaultStyleKey = typeof(TextBox);
            PlaceholderText = "(+998) 98 123 4567";
            RegisterPropertyChangedCallback(PhoneNumberProperty, new DependencyPropertyChangedCallback((DependencyObject obj, DependencyProperty dp) =>
            {
                Text = PhoneNumber;
            }));
            //PhoneNumber = Text;// string.Empty;

            this.RegisterPropertyChangedCallback(TextProperty, new DependencyPropertyChangedCallback((DependencyObject obj, DependencyProperty dp) =>
            {
                if (Text != null)
                {
                    PhoneNumber = Text.Replace(" ", "").Replace("+998", "");
                }
            }));
        }

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }

        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PhoneTextBox), new PropertyMetadata(string.Empty));

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (Text.Length == 0)
            {
                MaxLength = 0;
                return;
            }
            
            int spaceCount = Text.Count<Char>(x => x == ' '); 
            if (Text.StartsWith("+"))
            { 
                MaxLength = FullLength + spaceCount;

                if (Text.Length == 4 && SelectionStart == 4 && e.Key != VirtualKey.Back)
                {
                    Text += " ";
                    SelectionStart = 5;
                }

                if (Text.Length == 7 && SelectionStart == 7 && e.Key != VirtualKey.Back)
                {
                    Text += " ";
                    SelectionStart = 8;
                }

                if (Text.Length == 11 && SelectionStart == 11 && e.Key != VirtualKey.Back)
                {
                    Text += " ";
                    SelectionStart = 12;
                }
            }
            else
            {
                MaxLength = ShortLength+spaceCount;

                if (Text.Length == 2 && SelectionStart == 2 && e.Key != VirtualKey.Back)
                {
                    Text += " ";
                    SelectionStart = 3;
                }

                if (Text.Length == 6 && SelectionStart == 6 && e.Key != VirtualKey.Back)
                {
                    Text += " ";
                    SelectionStart = 7;
                }
            }
        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);

            base.SelectAll();
        }
    }
}
