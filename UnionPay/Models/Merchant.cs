using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Models
{
    using Controls;

    public class Merchant
    {
        public Merchant()
        {
        }

        public string Name { get; set; }

        public string MerchantId { get; set; }

        public string Logo { get; set; }

        public Merchants MerchantType { get; set; }
    }
}
