
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Payme
{
    using Newtonsoft.Json;

    internal class Response<T>
    {
        public class ErrorData
        {
            [JsonProperty(PropertyName = "code", Required = Required.Always)]
            public int Code { get; set; }

            [JsonProperty(PropertyName = "data", Required = Required.Default)]
            public string Data { get; set; }

            [JsonProperty(PropertyName = "message", Required = Required.AllowNull)]
            public string Message { get; set; }
        }

        [JsonProperty(PropertyName = "jsonrpc", Required = Required.Always)]
        public string JsonRPC { get; set; }

        [JsonProperty(PropertyName = "result", Required = Required.Default)]
        public T Result { get; set; }

        [JsonProperty(PropertyName = "error", Required = Required.Default)]
        public ErrorData Error { get; set; }
    }

    internal class Response
    {
        public class Transfer
        {
            public class SubmitData
            {
                public class ChequeData
                {
                    [JsonProperty(PropertyName = "amount", Required = Required.Always)]
                    private int _amount = 0;

                    [JsonProperty(PropertyName = "commission", Required = Required.Always)]
                    private int _commission = 0;

                    [JsonProperty(PropertyName = "_id", Required = Required.Always)]
                    public string Id { get; set; }

                    public int Amount { get { return _amount / 100; } }

                    public int Commission { get { return _commission / 100; } }
                }

                public class InfoData
                {
                    [JsonProperty(PropertyName = "owner", Required = Required.Always)]
                    public string Owner { get; set; }
                }

                [JsonProperty(PropertyName = "cheque", Required = Required.Always)]
                public ChequeData Cheque { get; set; }

                [JsonProperty(PropertyName = "p2pInfo", Required = Required.Always)]
                public InfoData Info { get; set; }
            }

            public class ConfirmData
            {
                [JsonProperty(PropertyName = "phone", Required = Required.Always)]
                public string PhoneNumber { get; set; }

                [JsonProperty(PropertyName = "sent", Required = Required.Always)]
                public string IsSent { get; set; }
            }

            public class ApproveData
            {

            }
        }
    }
}
