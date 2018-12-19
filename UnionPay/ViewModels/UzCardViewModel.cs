using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.ViewModels
{
    using Models;
    using Windows.UI.Xaml;
    public class UzCardViewModel : ViewModelBase<UzCard>
    {
        public UzCardViewModel(UzCard model) : base(model)
        {
        }

        public string Name { get; set; }

        public string Number { get { return this.Model.Number; }  set { this.Model.Number = value; } }

        public int ExpirationYear { get { return this.Model.ExpirationYear; } set { this.Model.ExpirationYear = (byte)value; } }

        public int ExpirationMonth { get { return this.Model.ExpirationMonth; } set { this.Model.ExpirationMonth = (byte)value; } }

        public string ExpirationString { get { return this.Model.ExpirationString; } }

        public string SMSInfoPhoneNumber { get { return this.Model.SMSInfoPhoneNumber; } set { this.Model.SMSInfoPhoneNumber = value; } }

        public string FormattedNumber
        {
            get
            {
                string res = this.Number.Substring(0, 4) + " " + Number.Substring(4, 4) + " " +
                    Number.Substring(8, 4) + " " + Number.Substring(12, 4); 

                return res;
            }
        }

        public Flags Flags { get; set; }

        private Visibility _deleteVisibility;

        public Visibility DeleteVisibility { get { return _deleteVisibility; } set { _deleteVisibility = value; RaisePropertyChanged(nameof(DeleteVisibility)); } }
    }

    public enum Flags
    {
        Undefined,
        New,
        Edit,
        Delete
    }
}
