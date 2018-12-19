using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnionPay.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UnionPay.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoreView : Page
    {
        public StoreView()
        {
            this.InitializeComponent();
        }

        private void page_Loading(FrameworkElement sender, object args)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(680, 560));

            //if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            //{
            //    this.feedbackButton.Visibility = Visibility.Visible;
            //}
        }

        private async void page_Loaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as StoreViewModel;
            if (model.Cards == null || model.Cards.Count <= 0)
            {
                var messageDialog = new MessageDialog("Для начала работы с платежами необходимо добавить хотя бы одну карту для оплаты. Карта в свою очередь должна быть подключена к услуге СМС информирования.");
                await messageDialog.ShowAsync();
                ((Frame)Window.Current.Content).Navigate(typeof(MyCardsView));
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var panel = (FrameworkElement)sender;
            var item = (MerchantViewModel)panel.DataContext;
            var storeModel = (StoreViewModel)this.DataContext;
            storeModel.Merchant = item;

            ((Frame)Window.Current.Content).Navigate(typeof(MerchantView), item);
        }

        private async void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            //await Microsoft.Services.Store.Engagement.Feedback.LaunchFeedbackAsync();

            //var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            //await launcher.LaunchAsync();
            await Task.CompletedTask;
        }

        private void transferButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TransferView), null);
        }

        private void myCardsButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MyCardsView));
        }
    }
}
