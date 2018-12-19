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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UnionPay.Views
{
    using Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyCardsView : Page
    {
        public MyCardsView()
        {
            this.InitializeComponent();
        }

        private void AppBarAdd_Click(object sender, RoutedEventArgs e)
        {
            UzCardViewModel model = new UzCardViewModel(new UzCard()) { Flags = Flags.New };
            model.DeleteVisibility = Visibility.Collapsed;
            var storeModel = (StoreViewModel)DataContext;
            storeModel.Card = model;

            ((Frame)Window.Current.Content).Navigate(typeof(EditCardView));
        }

        private void AppBarHome_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(StoreView));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var panel = (FrameworkElement)sender;
            var item = (UzCardViewModel)panel.DataContext;
            item.DeleteVisibility = Visibility.Visible;
            item.Flags = Flags.Edit;
            var storeModel = (StoreViewModel)this.DataContext;
            storeModel.Card = item;

            ((Frame)Window.Current.Content).Navigate(typeof(EditCardView));
        }
    }
}
