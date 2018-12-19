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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UnionPay.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditCardView : Page
    {
        public EditCardView()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TBName.Focus(FocusState.Programmatic);
        }

        private void TBName_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TBNumber.Focus(FocusState.Keyboard);
            }
        }

        private void TBNumber_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CBYear.Focus(FocusState.Keyboard);
            }
        }

        private void CBYear_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == Windows.System.VirtualKey.Enter)
            //{
            //    CBMonth.Focus(FocusState.Keyboard);
            //}
        }

        private void CBMonth_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == Windows.System.VirtualKey.Enter)
            //{
            //    TBSMSInfoPhoneNumber.Focus(FocusState.Keyboard);
            //}
        }

        private void TBSMSInfoPhoneNumber_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                BTNSave.Focus(FocusState.Keyboard);
            }
        }

        private void AppBarBack_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as StoreViewModel).LoadCards();
            ((Frame)Window.Current.Content).Navigate(typeof(MyCardsView));
        }

        private async void AppBarDelete_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Вы действительно хотите удалить выбранную карту?");
            messageDialog.Commands.Add(new UICommand("Да", new UICommandInvokedHandler(this.CommandInvokedHandler), 1));
            messageDialog.Commands.Add(new UICommand("Нет", new UICommandInvokedHandler(this.CommandInvokedHandler), 2));
            messageDialog.DefaultCommandIndex = 1;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            if ((int)command.Id == 1)
            {
                var model = DataContext as StoreViewModel;
                model.Cards.Remove(model.Card);
                model.SaveCards();
                ((Frame)Window.Current.Content).Navigate(typeof(MyCardsView));
            }
        }

        private async void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            if (TBName.Text == "")
            {
                var messageDialog = new MessageDialog("Название карты не должно быть пустым.");
                await messageDialog.ShowAsync();
                TBName.Focus(FocusState.Programmatic);
                return;
            }

            if (TBNumber.Text == "" || TBNumber.Text.Replace(" ", "").Length != 16 || !TBNumber.Text.Contains("8600"))
            {
                var messageDialog = new MessageDialog("Номер карты должен содержать 16 цифр и начинаться с 8600.");
                await messageDialog.ShowAsync();
                TBNumber.Focus(FocusState.Programmatic);
                return;
            }

            if (CBYear.SelectedIndex < 0)
            {
                var messageDialog = new MessageDialog("Срок окончания действия карты должен быть указан.");
                await messageDialog.ShowAsync();
                CBYear.Focus(FocusState.Programmatic);
                return;
            }

            if (CBMonth.SelectedIndex < 0)
            {
                var messageDialog = new MessageDialog("Срок окончания действия карты должен быть указан.");
                await messageDialog.ShowAsync();
                CBMonth.Focus(FocusState.Programmatic);
                return;
            }

            if (TBSMSInfoPhoneNumber.Text == "")
            {
                var messageDialog = new MessageDialog("Номер телефона, подключенный к услуге СМС информирования, не должен быть пустым.");
                await messageDialog.ShowAsync();
                TBSMSInfoPhoneNumber.Focus(FocusState.Programmatic);
                return;
            }

            if (TBSMSInfoPhoneNumber.Text.Replace(" ", "").Replace("+998", "").Length != 9)
            {
                var messageDialog = new MessageDialog("Номер телефона, подключенный к услуге СМС информирования, указан неверно.");
                await messageDialog.ShowAsync();
                TBSMSInfoPhoneNumber.Focus(FocusState.Programmatic);
                return;
            }

            var model = (DataContext as StoreViewModel);
            if (model.Card.Flags == Flags.New)
            {
                if (model.Cards == null)
                {
                    model.Cards = new System.Collections.ObjectModel.ObservableCollection<UzCardViewModel>();
                }

                model.Cards.Add(model.Card);
            }

            model.SaveCards();
            ((Frame)Window.Current.Content).Navigate(typeof(MyCardsView));
        }
    }
}
