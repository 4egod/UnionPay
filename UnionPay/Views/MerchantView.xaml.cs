using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnionPay.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UnionPay.Views
{
    public sealed partial class MerchantView : Page
    {
        public MerchantView()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CBCard.SelectedIndex = 0;
            var model = this.DataContext as StoreViewModel;
            model.Merchant.SMSTextBox = TBSMSCode;
            model.Merchant.AmountTextBox = TBAmount;
            TBId.Focus(FocusState.Programmatic);
        }

        private void TBId_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TBAmount.Focus(FocusState.Keyboard);
            }
        }

        private void TBAmount_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CBCard.Focus(FocusState.Keyboard);
            }
        }

        private void TBSMSCode_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                BTNApproove.Focus(FocusState.Keyboard);
            }
        }
    }
}
