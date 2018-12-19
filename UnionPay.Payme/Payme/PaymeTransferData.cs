using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Payme
{
    public struct PaymeTransferData
    {
        public string TransactionId { get; set; }

        public string DestinationCardOwner { get; set; }

        public int Amount { get; set; }

        public int Commission { get; set; }

        public string ConfirmationPhoneNumber { get; set; }
    }
}
