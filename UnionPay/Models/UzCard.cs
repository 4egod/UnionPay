using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Models
{
    public class UzCard
    {
        public string Number { get; set; }

        public byte ExpirationYear { get; set; }

        public byte ExpirationMonth { get; set; }

        public string SMSInfoPhoneNumber { get; set; }

        public string ExpirationString
        {
            get
            {
                return this.ExpirationYear.ToString("D2") + this.ExpirationMonth.ToString("D2");
            }
        }
    }
}
