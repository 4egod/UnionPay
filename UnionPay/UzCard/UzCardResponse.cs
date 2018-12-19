using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.UzCard
{
    using Newtonsoft.Json;

    #region "Not implemented"
    public class LogonResult
    {
        public class Flags
        {
            public bool has_email;
            public bool has_personal;
            public bool has_reserv_phone;
            public bool update_terms;
        }

        public Flags flags;
        public bool success;
    }

    public class ActivationCodeResult
    {
        public string phone;
        public bool sent;
    }
    #endregion

    public class UzCardResponse<T>
    {
        public class Error
        {
            public int code;
            public string data;
            public string message;
        }

        public string jsonrpc;
        public T result;
        public Error error;
    }

    public class UzCardResponse
    {
        public class ChequeActivationResult
        {
            public bool allowed;
            public string phone;
            public bool sent;
        }

        public class Cheque
        {
            [JsonProperty(PropertyName = "cheque", Required = Required.Always)]
            private ChequeData value = new ChequeData();

            public string Id { get { return this.value.Id; } }

            public int Amount { get { return this.value.Amount / 100; } }

            public int Commission { get { return this.value.Commission; } }

            public DateTime CreationTime { get { return this.value.CreationTime; } }

            public DateTime PayTime { get { return this.value.PayTime; } }

            public string PaySubject { get { return this.value.Account[0].Value; } }

            public string PaySubjectName { get { return this.value.Account[0].Title; } }

            public string MerchantId { get { return this.value.Merchant.Id; } }

            public string MerchantName { get { return this.value.Merchant.Name; } }

            public string MerchantCompanyName { get { return this.value.Merchant.Organisation; } }

            public string CardId { get { return this.value.Card.Id; } }

            public string CardNumber { get { return this.value.Card.Number; } }

            public string CardExpire { get { return this.value.Card.Expire; } }
        }

        internal class Account
        {
            [JsonProperty(PropertyName = "name", Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "title", Required = Required.Always)]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "value", Required = Required.Always)]
            public string Value { get; set; }
        }

        internal class Merchant
        {
            [JsonProperty(PropertyName = "_id", Required = Required.Always)]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "name", Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "organization", Required = Required.Always)]
            public string Organisation { get; set; }

            [JsonProperty(PropertyName = "logo", Required = Required.Always)]
            public string Logo { get; set; }

            [JsonProperty(PropertyName = "type", Required = Required.Always)]
            public string Type { get; set; }
        }

        internal class Card
        {
            [JsonProperty(PropertyName = "_id", Required = Required.Always)]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "number", Required = Required.Always)]
            public string Number { get; set; }

            [JsonProperty(PropertyName = "expire", Required = Required.Always)]
            public string Expire { get; set; }
        }

        internal class ChequeData
        {
            [JsonProperty(PropertyName = "create_time", Required = Required.Always)]
            private long creationTime;

            [JsonProperty(PropertyName = "pay_time", Required = Required.Always)]
            private long payTime;

            public ChequeData()
            {
                this.creationTime = 0;
                this.payTime = 0;
            }

            [JsonProperty(PropertyName = "_id", Required = Required.Always)]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "amount", Required = Required.Always)]
            public int Amount { get; set; }

            [JsonProperty(PropertyName = "commission", Required = Required.Always)]
            public int Commission { get; set; }

            public DateTime CreationTime { get { return this.creationTime > 0 ? new DateTime(1970, 1, 1).AddMilliseconds(this.creationTime).ToLocalTime() : new DateTime(); } }

            public DateTime PayTime { get { return this.payTime > 0 ? new DateTime(1970, 1, 1).AddMilliseconds(this.payTime).ToLocalTime() : new DateTime(); } }

            [JsonProperty(PropertyName = "description", Required = Required.Always)]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "account", Required = Required.Always)]
            public Account[] Account { get; set; }

            [JsonProperty(PropertyName = "merchant", Required = Required.Always)]
            public Merchant Merchant { get; set; }

            [JsonProperty(PropertyName = "card", Required = Required.AllowNull)]
            public Card Card { get; set; }
        }
    }
}

