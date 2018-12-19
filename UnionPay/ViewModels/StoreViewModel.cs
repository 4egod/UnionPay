using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;
    using Views;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class StoreViewModel : ViewModelBase
    {
        public StoreViewModel()
        {   
            this.MobileMerchants = new ObservableCollection<MerchantViewModel>();
            this.InternetMerchants = new ObservableCollection<MerchantViewModel>();
            this.WEBMerchants = new ObservableCollection<MerchantViewModel>();
            this.TVMerchants = new ObservableCollection<MerchantViewModel>();

            #region Mobile Merchants

            MobileMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.UMS, Name = "UMS", MerchantId = "40", Logo = "ms-appx:///Assets/Images/ums.png" }));

            MobileMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.UCell, Name = "UCell", MerchantId = "2", Logo = "ms-appx:///Assets/Images/ucell.png" }));

            MobileMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Beeline, Name = "Beeline", MerchantId = "1", Logo = "ms-appx:///Assets/Images/beeline.png" }));

            MobileMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Perfectum, Name = "Perfectum", MerchantId = "3", Logo = "ms-appx:///Assets/Images/perfectum.png" }));

            MobileMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.UzMobile, Name = "UzMobile", MerchantId = "4", Logo = "ms-appx:///Assets/Images/uzmobile.png" }));

            //MobileMerchants.Add(new MerchantViewModel(
            //    new Merchant() { MerchantType = Merchants.UzMobileGSM, Name = "UzMobile GSM", MerchantId = "55478199d2c4830936e6c832", Logo = "ms-appx:///Assets/Images/uzmobile-gsm.png" }));

            #endregion

            #region Internet Merchants

            //InternetMerchants.Add(new MerchantViewModel(
            //    new Merchant() { MerchantType = Merchants.TuronTelecom, Name = "Turon Telecom", MerchantId = "560b980f5c752614212bde8a", Logo = "ms-appx:///Assets/Images/turon-telecom.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.UzOnline, Name = "UzOnline", MerchantId = "10", Logo = "ms-appx:///Assets/Images/uzonline.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.TPS, Name = "TPS", MerchantId = "7", Logo = "ms-appx:///Assets/Images/tps.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.SarkorTelecom, Name = "Sarkor Telecom", MerchantId = "11", Logo = "ms-appx:///Assets/Images/sarkor.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.SharkTelecom, Name = "Shark Telecom", MerchantId = "5", Logo = "ms-appx:///Assets/Images/shark-telecom.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.FiberNet, Name = "Fiber Net", MerchantId = "19", Logo = "ms-appx:///Assets/Images/fibernet.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.ISTV, Name = "ISTV", MerchantId = "24", Logo = "ms-appx:///Assets/Images/istv.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                 new Merchant() { MerchantType = Merchants.BeelineInternet, Name = "Beeline Internet", MerchantId = "15", Logo = "ms-appx:///Assets/Images/ibeeline.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.EVO, Name = "EVO", MerchantId = "16", Logo = "ms-appx:///Assets/Images/evo.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.NanoTelecom, Name = "Nano Telecom", MerchantId = "41", Logo = "ms-appx:///Assets/Images/nano-telecom.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.TuronTelecom, Name = "Turon Telecom", MerchantId = "123", Logo = "ms-appx:///Assets/Images/turon-telecom.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Sonet, Name = "Sonet", MerchantId = "83", Logo = "ms-appx:///Assets/Images/sonet.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Comnet, Name = "Comnet", MerchantId = "21", Logo = "ms-appx:///Assets/Images/comnet.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Netco, Name = "Netco", MerchantId = "75", Logo = "ms-appx:///Assets/Images/netco.png" }));

            InternetMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.FreeLink, Name = "FreeLink", MerchantId = "76", Logo = "ms-appx:///Assets/Images/freelink.png" }));

            #endregion

            #region WEB

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.OLX, Name = "OLX", MerchantId = "30", Logo = "ms-appx:///Assets/Images/olx.png" }));

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.MyJob, Name = "MyJob.uz", MerchantId = "25", Logo = "ms-appx:///Assets/Images/myjob.png" }));

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Odnoklassniki, Name = "Одноклассники", MerchantId = "115", Logo = "ms-appx:///Assets/Images/odnoklassniki.png" }));

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.Bringo, Name = "Bringo", MerchantId = "118", Logo = "ms-appx:///Assets/Images/bringo.png" }));

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.YaponaMama, Name = "Yapona Mama", MerchantId = "90", Logo = "ms-appx:///Assets/Images/yaponamama.png" }));

            WEBMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.ADDirect, Name = "AD Direct", MerchantId = "67", Logo = "ms-appx:///Assets/Images/addirect.png" }));

            #endregion

            #region TV/MOVIE

            TVMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.UzDigitalTV, Name = "UZ Digital TV", MerchantId = "13", Logo = "ms-appx:///Assets/Images/uzdtv.png" }));

            TVMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.TelecomTV, Name = "Telecom TV", MerchantId = "18", Logo = "ms-appx:///Assets/Images/telecomtv.png" }));

            TVMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.ITV, Name = "itv", MerchantId = "81", Logo = "ms-appx:///Assets/Images/itv.png" }));

            TVMerchants.Add(new MerchantViewModel(
    new Merchant() { MerchantType = Merchants.KinoPro, Name = "Kino PRO", MerchantId = "89", Logo = "ms-appx:///Assets/Images/kinopro.png" }));

            TVMerchants.Add(new MerchantViewModel(
                new Merchant() { MerchantType = Merchants.OnlineTV, Name = "Online TV", MerchantId = "70", Logo = "ms-appx:///Assets/Images/onlinetv.png" }));

            TVMerchants.Add(new MerchantViewModel(
new Merchant() { MerchantType = Merchants.Muvi, Name = "МУВИ", MerchantId = "107", Logo = "ms-appx:///Assets/Images/muvi.png" }));

            #endregion

            Transfer = new TransferViewModel();

            LoadCards();
        }

        public ObservableCollection<MerchantViewModel> MobileMerchants { get; set; }

        public ObservableCollection<MerchantViewModel> InternetMerchants { get; set; }

        public ObservableCollection<MerchantViewModel> TVMerchants { get; set; }

        public ObservableCollection<MerchantViewModel> WEBMerchants { get; set; }

        public ObservableCollection<UzCardViewModel> Cards { get; set; }

        public UzCardViewModel Card { get; set; }

        public MerchantViewModel Merchant { get; set; }

        public TransferViewModel Transfer { get; set; }

        public int PivotSelectedIndex { get; set; }

        public void LoadCards()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            try
            {
                int CardsCount = (int)localSettings.Values["CardsCount"];
                Cards = new ObservableCollection<UzCardViewModel>();
                for (int i = 0; i < CardsCount; i++)
                {
                    ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)localSettings.Values["Card_" + i];
                    UzCardViewModel card = new UzCardViewModel(new UzCard()
                    {
                        Number = (string)composite["CardNumber"],
                        ExpirationYear = (byte)composite["ExpirationYear"],
                        ExpirationMonth = (byte)composite["ExpirationMonth"],
                        SMSInfoPhoneNumber = (string)composite["SMSInfoPhoneNumber"],
                    })
                    { Name = (string)composite["CardName"] };

                    Cards.Add(card);
                }
            }
            catch (Exception)
            {
                localSettings.Values["CardsCount"] = 0;
            }
        }

        public void SaveCards()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["CardsCount"] = Cards.Count;

            for (int i = 0; i < Cards.Count; i++)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["CardName"] = Cards[i].Name;
                composite["CardNumber"] = Cards[i].Number;
                composite["ExpirationYear"] = (byte)Cards[i].ExpirationYear;
                composite["ExpirationMonth"] = (byte)Cards[i].ExpirationMonth;
                composite["SMSInfoPhoneNumber"] = Cards[i].SMSInfoPhoneNumber;


                localSettings.Values["Card_" + i] = composite;
            }
        }
    }
}
