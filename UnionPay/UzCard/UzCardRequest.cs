using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.UzCard
{
    public class UzCardRequest
    {
        public string method;
        public object @params;

        #region "Not implemented"

        //public string session;

        //public static UzCardRequest Logon(string phone, string password)
        //{
        //    Dictionary<string, string> data = new Dictionary<string, string>();
        //    data.Add("login", phone);
        //    data.Add("password", password);
        //    UzCardRequest result = new UzCardRequest()
        //    {
        //        method = "users.log_in",
        //        session = "not-active",
        //        @params = data
        //    };

        //    return result;
        //}

        //public static UzCardRequest SendActivationCode()
        //{
        //    UzCardRequest result = new UzCardRequest()
        //    {
        //        method = "sessions.get_activation_code",
        //        session = "not-active",
        //        @params = ""
        //    };

        //    return result;
        //}

        //public static UzCardRequest ActivateCode(string code)
        //{
        //    UzCardRequest result = new UzCardRequest()
        //    {
        //        method = "sessions.activate",
        //        session = "not-active",
        //        @params = code
        //    };

        //    return result;
        //}

        #endregion

        public class CellularPayRecord
        {
            public Dictionary<string, string> account = new Dictionary<string, string>();
            public int amount;
            public string merchant_id;
        }

        public class CardId
        {
            public string number;
            public string expire;
        }

        public class Cheque
        {
            public CardId card_id;
            public string id;
        }

        internal class ActivatedCheque
        {
            public string id;
            public CardId card_id;
            public string code;
        }

        public static UzCardRequest GenerateCheque(Cheque cheque)
        {
            UzCardRequest res = new UzCardRequest()
            {
                method = "cheque.get_pay_code",
                @params = cheque
            };

            return res;
        }

        public static UzCardRequest SubmintCheque(Cheque cheque, string code)
        {
            ActivatedCheque rec = new ActivatedCheque()
            {
                card_id = cheque.card_id,
                code = code,
                id = cheque.id
            };

            UzCardRequest res = new UzCardRequest()
            {
                method = "cheque.pay",
                @params = rec
            };

            return res;
        }

        public static UzCardRequest PayMobile(string phone, int amount, string merchantId)
        {
            CellularPayRecord rec = new CellularPayRecord();
            rec.account.Add("phone", phone);
            rec.amount = amount * 100;
            rec.merchant_id = merchantId;

            UzCardRequest res = new UzCardRequest()
            {
                method = "cheque.create",
                @params = rec
            };

            return res;
        }

        public static UzCardRequest PayInternet(string login, int amount, string merchantId)
        {
            CellularPayRecord rec = new CellularPayRecord();
            rec.account.Add("login", login);
            rec.amount = amount * 100;
            rec.merchant_id = merchantId;

            UzCardRequest res = new UzCardRequest()
            {
                method = "cheque.create",
                @params = rec
            };

            return res;
        }
    }
}
