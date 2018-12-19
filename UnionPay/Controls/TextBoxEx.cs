using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace UnionPay.Controls
{
    public enum TextBoxExTypes
    {
        Default,
        PhoneNumber,
        CardNumber,
        Curency,
        SmsCode
    }

    public class TextBoxEx : TextBox
    {
        private const int PhoneMaxLength = 13;

        private const int ShortPhoneMaxLength = 9;

        private const int CardMaxLength = 16;

        private const int CurencyMaxLength = 7;

        private const int SmsCodeMaxLength = 6;

        private readonly SolidColorBrush NormalColor = new SolidColorBrush(Colors.White);

        private readonly SolidColorBrush FaultColor = new SolidColorBrush(Colors.LightPink);

        private bool parseError = false;

        public TextBoxEx()
        {
            DefaultStyleKey = typeof(TextBox);
            Type = TextBoxExTypes.Default;
            TextChanging += TextBoxEx_TextChanging;
            SelectionChanged += TextBoxEx_SelectionChanged;
            //TryParse();
            //TryFormat();
        }

        public TextBoxExTypes Type
        {
            get { return (TextBoxExTypes)GetValue(TypeProperty); }

            set
            {
                switch (value)
                {
                    case TextBoxExTypes.PhoneNumber:
                        {
                            MaxLength = PhoneMaxLength;
                            PlaceholderText = "(+998) 98 123 4567";
                            break;
                        }
                    case TextBoxExTypes.CardNumber:
                        {
                            MaxLength = CardMaxLength;
                            PlaceholderText = "8600 1234 5678 9999";
                            break;
                        }
                    case TextBoxExTypes.Curency:
                        {
                            MaxLength = CurencyMaxLength;
                            PlaceholderText = string.Empty;
                            break;
                        }
                    case TextBoxExTypes.SmsCode:
                        {
                            MaxLength = SmsCodeMaxLength;
                            PlaceholderText = "123456";
                            break;
                        }
                    case TextBoxExTypes.Default:
                    default:
                        {
                            MaxLength = 0;
                            PlaceholderText = string.Empty;
                            break;
                        };
                }

                SetValue(TypeProperty, value);
            }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TextBoxExTypes), typeof(TextBoxEx), new PropertyMetadata(TextBoxExTypes.Default));

        public long Value
        {
            get { return (long)GetValue(ValueProperty); }

            set
            {
                SetValue(ValueProperty, value);
                //if (value == 0)
                //{
                //    Text = string.Empty;
                //}
                //else
                //{
                //    Text = value.ToString();
                //    //TryFormat();
                //}
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(TextBoxEx), new PropertyMetadata((long)0));

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);
            base.SelectAll();
        }
        
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Left: return;
                case VirtualKey.V: MaxLength = 0; break;
            }

            base.OnKeyDown(e);            
        }

        private void TextBoxEx_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            TryParse();
            TryFormat();
        }

        private void TextBoxEx_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedText == Text)
            {
                return;
            }

            if (Text != null || Text.Length > 0)
            {
                SelectionStart = Text.Length;
            }
        }

        private void TryParse()
        {           
            if (Text == string.Empty)
            {
                parseError = false;
                Value = 0;
                Background = NormalColor;
                return;
            }

            try
            {
                if (Text != null && Text != string.Empty && Type != TextBoxExTypes.Default)
                {
                    string s = Text.Replace(" ", "").Replace(" ", "");
                    if (Type == TextBoxExTypes.PhoneNumber)
                    {
                        s = s.Replace("+", "");
                    }

                    if (s.Length > 0)
                    {
                        Value = long.Parse(s);
                    }
                }

                parseError = false;
                Background = NormalColor;
            }
            catch (Exception)
            {
                parseError = true;
                Value = 0;
                Background = FaultColor;
                return;
            }

            switch (Type)
            {
                case TextBoxExTypes.PhoneNumber:
                    {
                        if (Value < 0 || Value > 999999999999)
                        {
                            parseError = true;
                            Background = FaultColor;
                        }
                        else
                        {
                            parseError = false;
                            Background = NormalColor;
                        }
                        break;
                    }
                    
                case TextBoxExTypes.CardNumber:
                    {
                        if (Value < 0 || Value > 9999999999999999)
                        {
                            parseError = true;
                            Background = FaultColor;
                        }
                        else
                        {
                            parseError = false;
                            Background = NormalColor;
                        }
                        break;
                    }
                    
                case TextBoxExTypes.Curency:
                    {
                        if (Value < 0 || Value > 9999999)
                        {
                            parseError = true;
                            Background = FaultColor;
                        }
                        else
                        {
                            parseError = false;
                            Background = NormalColor;
                        }
                        break;
                    }

                case TextBoxExTypes.SmsCode:
                    {
                        if (Value < 0 || Value > 999999)
                        {
                            parseError = true;
                            Background = FaultColor;
                        }
                        else
                        {
                            parseError = false;
                            Background = NormalColor;
                        }
                        break;
                    }

                default: break;
            }
        }

        private void TryFormat()
        {
            if (Text.Replace("+", "") == string.Empty)
            {
                return;
            }

            int selectionStart = SelectionStart;

            if (!parseError)
            {
                int spaceCount = Text.Count<char>(x => x == ' ' || x == ' ');

                switch (Type)
                {
                    case TextBoxExTypes.PhoneNumber:
                        {
                            if (Text.StartsWith("+"))
                            {
                                MaxLength = PhoneMaxLength + spaceCount;

                                if (Value > 0 && Value <= 999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{1,3})", "+$1");
                                }

                                if (Value > 999 && Value <= 99999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{3})(\d{1,2})", "+$1 $2");
                                }

                                if (Value > 99999 && Value <= 99999999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{3})(\d{2})(\d{1,3})", "+$1 $2 $3");
                                }

                                if (Value > 99999999 && Value <= 999999999999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{3})(\d{2})(\d{3})(\d{1,4})", "+$1 $2 $3 $4");
                                }

                                string s = Text.Replace("+998", "").Replace("+99", "").Replace("+9", "").
                                    Replace("+", "").Replace(" ", "").Replace(" ", "");

                                if (s != string.Empty) Value = long.Parse(s);
                                else Value = 0;
                            }
                            else
                            {
                                MaxLength = ShortPhoneMaxLength + spaceCount;

                                if (Value > 0  && Value <= 99)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{1,2})", "$1");
                                }

                                if (Value > 99 && Value <= 99999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{2})(\d{1,3})", "$1 $2");
                                }

                                if (Value > 99999)// && Value <= 999999999)
                                {
                                    Text = Regex.Replace(Value.ToString(), @"(\d{2})(\d{3})(\d{1,4})", "$1 $2 $3");
                                }

                                if (Value > 999999999)
                                {
                                    Text = Text.Remove(11);
                                    string s = Text.Replace("+998", "").Replace("+99", "").Replace("+9", "").
                                    Replace("+", "").Replace(" ", "").Replace(" ", "");

                                    if (s != string.Empty) Value = long.Parse(s);
                                    else Value = 0;
                                }
                            }

                            break;
                        }
                        
                    case TextBoxExTypes.CardNumber:
                        {
                            MaxLength = CardMaxLength + spaceCount;

                            if (Value > 0 && Value <= 9999)
                            {
                                Text = Regex.Replace(Value.ToString(), @"(\d{1,4})", "$1");
                            }

                            if (Value > 9999 && Value <= 99999999)
                            {
                                Text = Regex.Replace(Value.ToString(), @"(\d{4})(\d{1,4})", "$1 $2");
                            }

                            if (Value > 99999999 && Value <= 999999999999)
                            {
                                Text = Regex.Replace(Value.ToString(), @"(\d{4})(\d{4})(\d{1,4})", "$1 $2 $3");
                            }

                            if (Value > 999999999999 && Value <= 9999999999999999)
                            {
                                Text = Regex.Replace(Value.ToString(), @"(\d{4})(\d{4})(\d{4})(\d{1,4})", "$1 $2 $3 $4");
                            }

                            break;
                        }

                    case TextBoxExTypes.Curency:
                        {
                            MaxLength = CurencyMaxLength + spaceCount;
                            Text = string.Format("{0:#,###,##0}", Value);
                            break;
                        }

                    default: break;
                }
            }

            TextBoxEx_SelectionChanged(null, null);
        }
    }
}
