using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnionPay.Controls;
using UnionPay.Payme;
using UnionPay.Views;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnionPay.ViewModels
{
    public class TransferViewModel : ViewModelBase
    {
        private string _transactionId;

        public TransferViewModel() : base()
        {
            _recipient = string.Empty;
            _amount = string.Empty;

            this.CancelCommand = new RelayCommand(param => this.Cancel());
            this.SubmitCommand = new RelayCommand(param => this.Submit());
            this.ApproveCommand = new RelayCommand(param => this.Approve());

            _isSubmitEnabled = true;
            _isApproveEnabled = false;
            _progressVisibility = Visibility.Collapsed;
        }

        public TextBoxEx RecipientTextBox { get; set; }

        public TextBoxEx AmountTextBox { get; set; }

        public TextBoxEx SMSTextBox { get; set; }

        private string _recipient;
        public string Recipient {
            get { return _recipient; }
            set { _recipient = value; RaisePropertyChanged(nameof(Recipient)); } }

        private string _amount;
        public string Amount { get { return _amount; } set { _amount = value; RaisePropertyChanged(nameof(Amount)); } }

        private string _commission;
        public string Commission { get { return _commission; } set { _commission = value; RaisePropertyChanged(nameof(Commission)); } }

        private string _total;
        public string Total { get { return _total; } set { _total = value; RaisePropertyChanged(nameof(Total)); } }

        private bool _isSubmitEnabled;
        public bool IsSubmitEnabled
        {
            get { return _isSubmitEnabled; }

            set
            {
                _isSubmitEnabled = value;
                RaisePropertyChanged(nameof(IsSubmitEnabled));
            }
        }

        private bool _isApproveEnabled;
        public bool IsApproveEnabled
        {
            get { return _isApproveEnabled; }

            set
            {
                _isApproveEnabled = value;
                RaisePropertyChanged(nameof(IsApproveEnabled));
            }
        }

        private Visibility _progressVisibility;
        public Visibility ProgressVisibility { get { return _progressVisibility; } set { _progressVisibility = value; RaisePropertyChanged(nameof(ProgressVisibility)); } }

        public string SMSCode { get; set; }

        public UzCardViewModel Card { get; set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SubmitCommand { get; set; }

        public RelayCommand ApproveCommand { get; set; }

        public void Cancel()
        {
            SMSTextBox.Focus(FocusState.Programmatic);
            ClearData();
            //(Window.Current.Content as Frame).Navigate(typeof(StoreView));
            (Window.Current.Content as Frame).GoBack();
        }

        public async void Submit()
        {
            ProgressVisibility = Visibility.Visible;
            RecipientTextBox.Type = TextBoxExTypes.CardNumber;

            if (Recipient == null || Recipient == "" || Amount == null || Amount == "")
            {
                MessageDialog msg = new MessageDialog("Счет на оплату не может содержать пустых полей.", "Ошибка:");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                return;
            }

            if (Recipient == "" || GetRecepient().Length != 16 || !Recipient.Contains("8600"))
            {
                var messageDialog = new MessageDialog("Номер карты получателя должен содержать 16 цифр и начинаться с 8600.");
                await messageDialog.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                RecipientTextBox.Focus(FocusState.Programmatic);
                return;
            }

            int amount = 0;
            try
            {
                amount = int.Parse(GetAmount());
                if (amount < 5000 || amount > 1000000)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                MessageDialog msg = new MessageDialog("Значение суммы должно быть числом и быть в пределах от 5 000 до 1 000 000.", "Ошибка:");
                await msg.ShowAsync();
                ProgressVisibility = Visibility.Collapsed;
                AmountTextBox.Text = "";
                AmountTextBox.Focus(FocusState.Programmatic);
                return;
            }

            try
            {
                IsSubmitEnabled = false;
                var data = await PaymeHelper.SubmitTransfer(amount, Card.Number.Replace(" ", ""), Card.ExpirationString, GetRecepient());
                _transactionId = data.TransactionId;
                RecipientTextBox.Type = TextBoxExTypes.Default;
                Recipient = data.DestinationCardOwner;
                Commission = String.Format("{0:#,##0}", data.Commission);//data.Result.Commission.ToString();
                Total = String.Format("{0:#,##0}", data.Amount); //data.Result.Amount.ToString();

                IsApproveEnabled = true;
                RecipientTextBox.Type = TextBoxExTypes.Default;
                SMSTextBox.Focus(FocusState.Programmatic);
            }
            catch (PaymeException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (Payme):");
                await msg.ShowAsync();
                IsSubmitEnabled = true;
            }
            catch (Exception e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка:");
                await msg.ShowAsync();
                IsSubmitEnabled = true;
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

            try
            {
                await PaymeHelper.ApproveTransfer(_transactionId, SMSCode);
            }
            catch (PaymeException e)
            {
                MessageDialog msg = new MessageDialog(e.Message, "Ошибка (Payme):");
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

            MessageDialog msgs = new MessageDialog("Поздравляем! Перевод успешно произведен.", "Успешная операция:");
            await msgs.ShowAsync();

            ClearData();
            (Window.Current.Content as Frame).Navigate(typeof(StoreView));
        }

        private void ClearData()
        {
            RecipientTextBox.Type = TextBoxExTypes.CardNumber;
            Recipient = string.Empty;
            Amount = string.Empty;
            SMSCode = string.Empty;
            Commission = string.Empty;
            Total = string.Empty;
            _isSubmitEnabled = true;
            _isApproveEnabled = false;
        }

        private string GetRecepient()
        {
            return Recipient.Replace(" ", "");
        }

        private string GetAmount()
        {
            return Amount.Replace(" ", "").Replace(" ", "");
        }
    }
}
