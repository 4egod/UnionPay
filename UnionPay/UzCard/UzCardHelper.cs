
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.UzCard
{
    using Newtonsoft.Json;
    using System;
    using Windows.Security.Cryptography.Certificates;
    using Windows.Web.Http;
    using Windows.Web.Http.Filters;

    public static class UzCardHelper
    {
        private const string ApiUrl = "https://my.uzcard.uz/api";

        private static readonly Uri ApiUri = new Uri(ApiUrl);

        private static string _merchantId = string.Empty;

        private static string _chequeId = string.Empty;

        private static string _cardNumber = string.Empty;

        private static string _cardExpire = string.Empty;

        public static async Task SubmintChequeByPhone(string merchantId, string phone, int amount, string cardNumber, string cardExpire)
        {
            _merchantId = merchantId;
            _cardNumber = cardNumber;
            _cardExpire = cardExpire;

            UzCardResponse<UzCardResponse.Cheque> cheque = await UzCardHelper.PostAsync<UzCardResponse<UzCardResponse.Cheque>>
                (UzCardRequest.PayMobile(phone, amount, merchantId));
            if (cheque.error != null)
            {
                throw new UzCardException(cheque.error.message);
            }

            UzCardResponse<UzCardResponse.ChequeActivationResult> chequeRes = 
                await UzCardHelper.PostAsync<UzCardResponse<UzCardResponse.ChequeActivationResult>>
                (UzCardRequest.GenerateCheque(new UzCardRequest.Cheque()
            {
                card_id = new UzCardRequest.CardId() { number = cardNumber, expire = cardExpire },
                id = cheque.result.Id
            }));

            if (chequeRes.error != null)
            {
                throw new UzCardException(chequeRes.error.message);
            }

            _chequeId = cheque.result.Id;
        }

        public static async Task SubmintChequeByLogin(string merchantId, string login, int amount, string cardNumber, string cardExpire)
        {
            _merchantId = merchantId;
            _cardNumber = cardNumber;
            _cardExpire = cardExpire;

            UzCardResponse<UzCardResponse.Cheque> cheque = await UzCardHelper.PostAsync<UzCardResponse<UzCardResponse.Cheque>>
                (UzCardRequest.PayInternet(login, amount, merchantId));
            if (cheque.error != null)
            {
                if ((cheque.error.message == null && cheque.error.data == "login") || cheque.error.message.ToLower() == "login not found")
                {
                    throw new UzCardException("Неверный логин.");
                }
                else throw new UzCardException(cheque.error.message);
            }

            UzCardResponse<UzCardResponse.ChequeActivationResult> chequeRes =
                await UzCardHelper.PostAsync<UzCardResponse<UzCardResponse.ChequeActivationResult>>
                (UzCardRequest.GenerateCheque(new UzCardRequest.Cheque()
                {
                    card_id = new UzCardRequest.CardId() { number = cardNumber, expire = cardExpire },
                    id = cheque.result.Id
                }));

            if (chequeRes.error != null)
            {
                throw new UzCardException(chequeRes.error.message);
            }

            _chequeId = cheque.result.Id;
        }

        public static async Task ApprooveCheque(string smsCode)
        {
            UzCardResponse<UzCardResponse.Cheque> cheque = null;

            cheque = await UzCardHelper.PostAsync<UzCardResponse<UzCardResponse.Cheque>>
                (UzCardRequest.SubmintCheque(new UzCardRequest.Cheque()
            {
                card_id = new UzCardRequest.CardId() { number = _cardNumber, expire = _cardExpire },
                id = _chequeId,
            }, smsCode));

            if (cheque.error != null)
            {
                throw new UzCardException(cheque.error.message);
            }
        }

        private static async Task<T> PostAsync<T>(object command)
        {
            string requestCmd = JsonConvert.SerializeObject(command);

            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            filter.CookieUsageBehavior = HttpCookieUsageBehavior.Default;

            HttpClient client = new HttpClient(filter);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUri);
            HttpStringContent content = new HttpStringContent(requestCmd);
            content.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("text/plain;charset=UTF-8");
            content.Headers.ContentLength = (ulong)Encoding.UTF8.GetBytes(requestCmd).Length;
            request.Content = content;
            string response = string.Empty;

            try
            {
                response = (await client.PostAsync(ApiUri, content)).Content.ToString();
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception)
            {
                throw new UzCardException("Сервис временно не доступен.");
            }
        }
    }
}
