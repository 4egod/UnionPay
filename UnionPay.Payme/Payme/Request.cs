using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Payme
{
    using Newtonsoft.Json;

    internal class Request
    {
        [JsonProperty(PropertyName = "method", Required = Required.Always)]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params", Required = Required.Always)]
        public object Params { get; set; }

        public class Transfer
        {
            public class SubmitData
            {
                [JsonProperty(PropertyName = "number", Required = Required.Always)]
                public string DestinationCardNumber { get; set; }

                [JsonProperty(PropertyName = "amount", Required = Required.Always)]
                public int Amount { get; set; }

                public class PayCard
                {
                    [JsonProperty(PropertyName = "expire", Required = Required.Always)]
                    public string ExpirationDate { get; set; }

                    [JsonProperty(PropertyName = "number", Required = Required.Always)]
                    public string Number { get; set; }
                }

                [JsonProperty(PropertyName = "pay_card", Required = Required.Always)]
                public PayCard SourceCard { get; set; }
            }

            public class ConfirmData
            {
                [JsonProperty(PropertyName = "id", Required = Required.Always)]
                public string Id { get; set; }
            }

            public class ApproveData
            {
                [JsonProperty(PropertyName = "id", Required = Required.Always)]
                public string Id { get; set; }

                [JsonProperty(PropertyName = "code", Required = Required.Always)]
                public string Code { get; set; }
            }

            public static Request GetSubmitData(int amount, string sourceCardNumber, string sourceCardExpirationDate, string destinationCardNumber)
            {
                SubmitData data = new SubmitData()
                {
                    Amount = amount,
                    DestinationCardNumber = destinationCardNumber,
                    SourceCard = new SubmitData.PayCard()
                    {
                        ExpirationDate = sourceCardExpirationDate,
                        Number = sourceCardNumber
                    }
                };

                Request res = new Request()
                {
                    Method = "fast_p2p.create",
                    Params = data
                };

                return res;
            }

            public static Request GetConfirmData(string transactionId)
            {
                ConfirmData data = new ConfirmData()
                {
                    Id = transactionId
                };

                Request res = new Request()
                {
                    Method = "fast_p2p.get_pay_code",
                    Params = data
                };

                return res;
            }

            public static Request GetApproveData(string transactionId, string smsCode)
            {
                ApproveData data = new ApproveData()
                {
                    Code = smsCode,
                    Id = transactionId
                };

                Request res = new Request()
                {
                    Method = "fast_p2p.pay",
                    Params = data
                };

                return res;
            }
        }
    }
}
