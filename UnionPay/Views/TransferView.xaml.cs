using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnionPay.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UnionPay.Views
{
    public sealed partial class TransferView : Page
    {
        public TransferView()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CardComboBox.SelectedIndex = 0;
            RecipientTextBox.Focus(FocusState.Programmatic);
            var model = (this.DataContext as StoreViewModel).Transfer;
            model.RecipientTextBox = RecipientTextBox;
            model.AmountTextBox = AmountTextBox;
            model.SMSTextBox = SMSTextBox;
            RecipientTextBox.Focus(FocusState.Programmatic);
        }

        private void RecipientTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CardComboBox.Focus(FocusState.Programmatic);
            }
        }

        private void AmountTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SubmitButton.Focus(FocusState.Programmatic);
            }
        }

        private void SMSTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ApproveButton.Focus(FocusState.Keyboard);
            }
        }
    }
}
