using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.UzCard
{
    public class UzCardException : Exception
    {
        public UzCardException(string message) : base(message)
        {

        }
    }
}
