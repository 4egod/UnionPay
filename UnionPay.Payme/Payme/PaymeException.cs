using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Payme
{
    public class PaymeException : Exception
    {
        public PaymeException(string message) : base(message)
        {

        }
    }
}
