using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.ViewModels
{
    using MBank;
    using Models;
    using System.Windows.Input;
    using UzCard;
    using Views;
    using Controls;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class MerchantViewModel : ViewModelBase<Merchant>
    {
        public MerchantViewModel(Merchant model) : base(model)
        {
            this.CancelCommand = new RelayCommand(param => this.Cancel());
            this.SubmintCommand = new RelayCommand(param => this.Submint());
            this.ApprooveCommand = new RelayCommand(param => this.Approve());

            _sendEnabled = true;
            _approoveEnabled = false;
            _progressVisibility = Visibility.Collapsed;
        }

        public string Name { get { return this.Model.Name; } }

        public string MerchantId { get { return this.Model.MerchantId; } }

        public string Logo { get { return this.Model.Logo; } }

        public Merchants MerchantType { get { return this.Model.MerchantType; } }

        public string PaySubjectName
        {
            get
            {
                switch (this.MerchantType)
                {
                    case Merchants.UMS:
                    case Merchants.UCell:
                    case Merchants.Beeline:
                    case Merchants.Perfectum:
                    case Merchants.UzMobile:
                        return "Номер телефона:";
                    case Merchants.TuronTelecom:
                    case Merchants.ISTV:                  
                    case Merchants.TPS:
                    case Merchants.SarkorTelecom:
                    case Merchants.SharkTelecom:
                    case Merchants.FiberNet:
                    case Merchants.NanoTelecom:
                    case Merchants.Sonet:
                    case Merchants.Comnet:
                    case Merchants.BeelineInternet:
                    case Merchants.Netco:
                    case Merchants.FreeLink:
                    case Merchants.TelecomTV:
                    case Merchants.Odnoklassniki:
                        return "Логин:";
                    case Merchants.UzOnline:
                    case Merchants.EVO:
                    case Merchants.UzDigitalTV:
                    case Merchants.ITV:
                        return "Номер счета:";
                    case Merchants.WebSum:
                    case Merchants.MyJob:
                    case Merchants.OLX:
                    case Merchants.ADDirect:
                    case Merchants.KinoPro:
                    case Merchants.OnlineTV:
                    case Merchants.Muvi:
                        return "Идентификационный номер:";
                    case Merchants.Bringo:
                    case Merchants.YaponaMama:
                        return "Номер заказа:";
                    default: return "#PAYSUBJECT#";
                }
            }
        }

        public string PaySubjectHint
        {
            get
            {
                switch (this.MerchantType)
                {
                    case Merchants.UMS:
                    case Merchants.UCell:
                    case Merchants.Beeline:
                    case Merchants.Perfectum:
                    case Merchants.UzMobile:
                        return "(+998) 98 123 4567";
                    case Merchants.TuronTelecom:
                    case Merchants.ISTV:
                    case Merchants.SarkorTelecom:
                    case Merchants.SharkTelecom:
                    case Merchants.FiberNet:
                    case Merchants.NanoTelecom:
                    case Merchants.Sonet:
                    case Merchants.Comnet:
                    case Merchants.Netco:
                    case Merchants.FreeLink:
                    case Merchants.TelecomTV:
                    case Merchants.Odnoklassniki:
                        return "Логин";
                    case Merchants.UzOnline:
                    case Merchants.MyJob:
                    case Merchants.OLX:
                    case Merchants.ADDirect:
                    case Merchants.KinoPro:
                    case Merchants.OnlineTV:
                    case Merchants.Muvi:
                    case Merchants.UzDigitalTV:
                    case Merchants.ITV:
                        return "Идентификационный номер";
                    case Merchants.EVO:
                        return "Номер счета";
                    case Merchants.TPS:
                        return "Логин (tps1234567)";
                    case Merchants.BeelineInternet:
                        return "Логин (001234567)";
                    case Merchants.WebSum:
                        return "WEBSum Id";
                    case Merchants.Bringo:
                    case Merchants.YaponaMama:
                        return "Номер заказа";
                    default: return "#PLACEHOLDER#";
                }
            }
        }

        public TextBoxExTypes PaySubjectType
        {
            get
            {
                switch (MerchantType)
                {
                    case Merchants.UMS:
                    case Merchants.UCell:
                    case Merchants.Beeline:
                    case Merchants.Perfectum:
                    case Merchants.UzMobile: return TextBoxExTypes.PhoneNumber;

                    default: return TextBoxExTypes.Default;
                }
            }
        }

        private string _paySubject;
        public string PaySubject
        {
            get { return _paySubject; }

            set
            {
                _paySubject = value;
                RaisePropertyChanged(nameof(PaySubject));
            }
        }

        private string _amount;
        public string Amount
        {
            get { return _amount; }

            set
            {
                _amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }

        private bool _sendEnabled;
        public bool SendEnabled
        {
            get { return _sendEnabled; }

            set
            {
                _sendEnabled = value;
                RaisePropertyChanged(nameof(SendEnabled));
            }
        }

        private bool _approoveEnabled;
        public bool ApprooveEnabled
        {
            get { return _approoveEnabled; }

            set
            {
                _approoveEnabled = value;
                RaisePropertyChanged(nameof(ApprooveEnabled));
            }
        }

        private Visibility _progressVisibility;
        public Visibility ProgressVisibility { get { return _progressVisibility; } set { _progressVisibility = value; RaisePropertyChanged(nameof(ProgressVisibility)); } }

        public string SMSCode { get; set; }

        public UzCardViewModel Card { get; set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SubmintCommand { get; set; }

        public RelayCommand ApprooveCommand { get; set; }

        public TextBox AmountTextBox { get; set; }

        public TextBox SMSTextBox { get; set; }

        public void Cancel()
        {
            ClearData();
            (Window.Current.Content as Frame).Navigate(typeof(StoreView));
        }

        public async void Submint()
        {
            ProgressVisibility = Visibility.Visible;

            if (PaySubject == null || PaySubject == "" || Amount == null || Amount == "")
            {
                MessageDialog msg = new MessageDialog("Счет на оплату не может содержать пустых полей.", "Ошибка:");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }

            int amount = 0;
            try
            {
                amount = int.Parse(Amount.Replace(" ", ""));
                if (amount < 1000 || amount > 100000)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                MessageDialog msg = new MessageDialog("Значение суммы должно быть числом и быть в пределах от 1 000 до 100 000 сум.", "Ошибка:");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                AmountTextBox.Text = "";
                AmountTextBox.Focus(FocusState.Programmatic);
                return;
            }     

            try
            {
                switch (MerchantType)
                {
                    /// MOBILE
                    case Merchants.UMS:
                    case Merchants.UCell:
                    case Merchants.Beeline:
                    case Merchants.Perfectum:
                    case Merchants.UzMobile:
                        await MBankHelper.SubmintChequeByPhone(MerchantId, DataHelper.GetPhoneNumber(PaySubject),
                            amount, DataHelper.GetPhoneNumber(Card.SMSInfoPhoneNumber), DataHelper.GetCardNumber(Card.Number), Card.ExpirationString);
                        break;

                    /// INTERNET
                    case Merchants.UzOnline:
                    case Merchants.TPS:
                    case Merchants.SarkorTelecom:
                    case Merchants.SharkTelecom:
                    case Merchants.FiberNet:
                    case Merchants.ISTV:
                    case Merchants.BeelineInternet:
                    case Merchants.EVO:
                    case Merchants.NanoTelecom:
                    case Merchants.TuronTelecom:
                    case Merchants.Sonet:
                    case Merchants.Comnet:
                    case Merchants.Netco:
                    case Merchants.FreeLink:

                    //await UzCardHelper.SubmintChequeByLogin(MerchantId, PaySubject, amount, DataHelper.GetCardNumber(Card.Number),
                    //    Card.ExpirationString);
                    //break;




                    /// WEB
                    case Merchants.MyJob:
                    case Merchants.OLX:
                    case Merchants.ADDirect:

                    /// TV/Movie
                    case Merchants.OnlineTV:
                    case Merchants.UzDigitalTV:
                    case Merchants.TelecomTV:
                    case Merchants.ITV:
                        await MBankHelper.SubmintChequeByLogin(MerchantId, PaySubject, amount, DataHelper.GetPhoneNumber(Card.SMSInfoPhoneNumber),
                            DataHelper.GetCardNumber(Card.Number), Card.ExpirationString);
                        break;
                    default:
                        {
                            MessageDialog msg = new MessageDialog("UNBINDED");
                            await msg.ShowAsync();
                        }
                        break;
                }

                SendEnabled = false;
                ApprooveEnabled = true;
                SMSTextBox.Focus(FocusState.Programmatic);
            }
            catch (UzCardException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (UzCard):");
                await msg.ShowAsync();
            }
            catch (MBankException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (MBank):");
                await msg.ShowAsync();
            }
            catch (Exception e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка:");
                await msg.ShowAsync();
            }

            ProgressVisibility = Visibility.Collapsed;
        }

        public async void Approve()
        {
            ProgressVisibility = Visibility.Visible;

            if (SMSCode == null || SMSCode.Length != 6)
            {
                MessageDialog dlg = new MessageDialog("СМС код должен состоять из 6 цифр.", "Ошибка:");
                await dlg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }

            string moneyRemain = string.Empty;
            try
            {
                switch (MerchantType)
                {
                    case Merchants.TuronTelecom:
                        await UzCardHelper.ApprooveCheque(SMSCode);
                        break;
                    case Merchants.UMS:
                    case Merchants.UCell:
                    case Merchants.Beeline:
                    case Merchants.Perfectum:
                    case Merchants.UzMobile:

                    case Merchants.UzOnline:
                    case Merchants.TPS:
                    case Merchants.SarkorTelecom:
                    case Merchants.SharkTelecom:
                    case Merchants.FiberNet:
                    case Merchants.ISTV:
                    case Merchants.BeelineInternet:
                    case Merchants.EVO:
                    case Merchants.NanoTelecom:
                    case Merchants.Sonet:
                    case Merchants.Comnet:
                    case Merchants.Netco:
                    case Merchants.FreeLink:

                    case Merchants.MyJob:
                    case Merchants.OLX:
                    case Merchants.ADDirect:
                    case Merchants.OnlineTV:

                    case Merchants.UzDigitalTV:
                    case Merchants.TelecomTV:
                    case Merchants.ITV:
                        moneyRemain = await MBankHelper.ApprooveCheque(SMSCode);
                        break;
                    default:
                            {
                                MessageDialog msg = new MessageDialog("UNBINDED");
                                await msg.ShowAsync();
                            }
                        break;
                }
            }
            catch (UzCardException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (UzCard):");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }
            catch (MBankException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (MBank):");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }
            catch (Exception e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка:");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }

            ProgressVisibility = Visibility.Collapsed;

            if (moneyRemain != string.Empty)
            {
                moneyRemain = "\nОстаток денежных средств cоставляет:\n" + moneyRemain + " сум.";
            }

            MessageDialog msgs = new MessageDialog("Поздравляем! Оплата успешно произведена." + moneyRemain, "Успешная операция:");
            await msgs.ShowAsync();

            ClearData();
            (Window.Current.Content as Frame).Navigate(typeof(StoreView));
        }

        private void ClearData()
        {
            PaySubject = string.Empty;
            Amount = string.Empty;
            SMSCode = string.Empty;
            _sendEnabled = true;
            _approoveEnabled = false;
        }
    }
}
