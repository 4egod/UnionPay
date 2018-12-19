using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSender
{
    using Microsoft.Azure.NotificationHubs;

    class Program
    {
        static void Main(string[] args)
        {
            SendNotificationAsync("UnionPay", "Some message about some update");
            Console.WriteLine("Sended. Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async void SendNotificationAsync(string header, string message)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://unionpay.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=U3P2xeIBDyD0qRkCdTwPIBLPNs4qNxZsxWp3pFHJvio=", "unionpay");

            string toast = $@"
<toast>
  <visual>
    <binding template=""ToastImageAndText02"">
      <image id=""1"" src=""ms-appx:///Assets/StoreLogo.scale-100.png""/>
      <text id=""1"">{header}</text>    
      <text id=""2"">{message}</text>
    </binding>
  </visual>
</toast>";

            await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}
