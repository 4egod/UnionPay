using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UnionPay
{
    using ViewModels;
    using Views;
    using Windows.UI.Core;
    using Windows.Networking.PushNotifications;
    using Microsoft.WindowsAzure.Messaging;
    using Windows.UI.Popups;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 680, Width = 560 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.TryUnsnapToFullscreen();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content is Views.MerchantView)
            {
                ((rootFrame.Content as MerchantView).DataContext as ViewModels.StoreViewModel).Merchant.Cancel();
                rootFrame.Navigate(typeof(StoreView));
                e.Handled = true;
            }

            if (rootFrame.Content is Views.TransferView)
            {
                ((rootFrame.Content as TransferView).DataContext as ViewModels.StoreViewModel).Transfer.Cancel();
                rootFrame.Navigate(typeof(StoreView));
                e.Handled = true;
            }

            if (rootFrame.Content is Views.MyCardsView)
            {
                //((rootFrame.Content as SettingsView).DataContext as ViewModels.StoreViewModel).Merchant.Cancel();
                rootFrame.Navigate(typeof(StoreView));
                e.Handled = true;
            }

            if (rootFrame.Content is Views.EditCardView)
            {
                (rootFrame.DataContext as StoreViewModel).LoadCards();
                rootFrame.Navigate(typeof(MyCardsView));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    this.DebugSettings.EnableFrameRateCounter = true;
            //}
#endif
            InitNotificationsAsync();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.DataContext = new StoreViewModel();
                //(rootFrame.DataContext as StoreViewModel).LoadCards();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(StoreView), e.Arguments);
            }
            // Ensure the current window is active

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(680, 560));
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async void InitNotificationsAsync()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                var hub = new NotificationHub("unionpay", "Endpoint=sb://unionpay.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=LudZYRPP/fgl0+2s9HdQdKeMSAYpbLpIH4hfpwr8pUg=");
                var result = await hub.RegisterNativeAsync(channel.Uri);

                // Displays the registration ID so you know it was successful
                //if (result.RegistrationId != null)
                //{
                //    var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
                //    dialog.Commands.Add(new UICommand("OK"));
                //    await dialog.ShowAsync();
                //}
            }
            catch (Exception)
            {
            }
        }
    }
}
