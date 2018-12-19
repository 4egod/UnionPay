using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay
{
    public static class DataHelper
    {
        public static string GetPhoneNumber(string value)
        {
            return value.Replace(" ", "").Replace(" ", "").Replace("+998", "");
        }

        public static string GetCardNumber(string value)
        {
            return value.Replace(" ", "").Replace(" ", "");
        }
    }
}
