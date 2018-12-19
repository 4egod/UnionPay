using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.MBank
{
    public class MBankException : Exception
    {
        public MBankException(string message) : base(message)
        {

        }

        public MBankException(string message, Exception e) : base(message, e)
        {

        }
    }
}
