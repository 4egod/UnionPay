using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace UnionPay.MBank
{
    public static class MBankHelper
    {
        private class Response
        {
            public class Submint
            {
                public class ResponseObject
                {
                    [JsonProperty(PropertyName = "MERCHANT_AMOUNT")]
                    public string MerchantAmount { get; set; }

                    [JsonProperty(PropertyName = "MERCHANT_CARD_EXPIRE")]
                    public string CardError { get; set; }

                    [JsonProperty(PropertyName = "MERCHANT_ID")]
                    public string MerchantId { get; set; }

                    [JsonProperty(PropertyName = "MERCHANT_CODE")]
                    public string MerchantCode { get; set; }

                    [JsonProperty(PropertyName = "MERCHANT_CARD_PHONE_CODE")]
                    public string MerchantCardPhoneCode { get; set; }
                }

                /// <summary>
                /// error or success
                /// </summary>
                [JsonProperty(PropertyName = "result", Required = Required.Always)]
                public string ResponseStatus { get; set; }

                /// <summary>
                /// Message of error, if succes it's empty
                /// </summary>
                [JsonProperty(PropertyName = "response", Required = Required.Always)]
                public ResponseObject Response { get; set; }
            }
        }

        private const string ApiUrl = "https://oplata.uz";

        private static readonly Uri ApiUri = new Uri(ApiUrl + "/");

        private static string _merchantId = string.Empty;

        private static string _sessionId = string.Empty;

        private static string _token = string.Empty;

        public static async Task SubmintChequeByLogin(string merchantId, string login, int amount, string cardPhone, string cardNumber, string cardExpire)
        {
            _merchantId = merchantId;
            string cardPhoneCode = cardPhone.Substring(0, 2);
            string cardPhoneId = cardPhone.Substring(2, cardPhone.Length - 2);

            string command = string.Format("MERCHANT_ID={0}&MERCHANT_AMOUNT={1}&MERCHANT_CARD_PHONE_CODE={2}&" +
                "MERCHANT_CARD_PHONE={3}&MERCHANT_CARD_ID={4}&MERCHANT_CARD_EXPIRE={5}&MARCHANT_ACCEPT=on",
                login, amount.ToString(), cardPhoneCode, cardPhoneId, cardNumber, cardExpire);

            await SubmintCheque(command);
        }

        public static async Task SubmintChequeByPhone(string merchantId, string phone, int amount, string cardPhone, string cardNumber, string cardExpire)
        {
            if (phone == null || phone.Length != 9)
            {
                throw new Exception("Неверный номер телефона.");
            }

            _merchantId = merchantId;
            string payPhoneCode = phone.Substring(0, 2);
            string payPhoneId = phone.Substring(2, phone.Length - 2);
            string cardPhoneCode = cardPhone.Substring(0, 2);
            string cardPhoneId = cardPhone.Substring(2, cardPhone.Length - 2);

            string command = string.Format("MERCHANT_CODE={0}&MERCHANT_ID={1}&MERCHANT_AMOUNT={2}&MERCHANT_CARD_PHONE_CODE={3}&" +
                "MERCHANT_CARD_PHONE={4}&MERCHANT_CARD_ID={5}&MERCHANT_CARD_EXPIRE={6}&MARCHANT_ACCEPT=on",
                payPhoneCode, payPhoneId, amount.ToString(), cardPhoneCode, cardPhoneId, cardNumber, cardExpire);

            await SubmintCheque(command);
        }

        public static async Task<string> ApprooveCheque(string smsCode)
        {
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            filter.CookieUsageBehavior = HttpCookieUsageBehavior.Default;
            filter.AllowAutoRedirect = true;

            HttpClient client = new HttpClient(filter);

            string command = string.Format("MERCHANT_SMSCODE={0}&accept=", smsCode);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl + _token));
            request.Headers.Cookie.Add(new HttpCookiePairHeaderValue("sessionid", _sessionId));
            HttpStringContent content = new HttpStringContent(command);
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded; charset=UTF-8");
            request.Content = content;

            try
            {
                var val = await client.SendRequestAsync(request);
                string cnt = val.Content.ToString();

                int pos = cnt.IndexOf("Вы исчерпали 5 попыток ввода СМС кода");
                if (pos != -1)
                {
                    throw new MBankException("Вы исчерпали все попытки ввода СМС кода. Пожалуйста начните заново.");
                }

                pos = cnt.IndexOf("Попытки:");
                if (pos != -1)
                {
                    throw new MBankException("Неверный СМС код. Оставшееся кол-во попыток: " + cnt.Substring(pos + 9, 1));
                }

                pos = cnt.IndexOf("Попытки:");
                if (pos != -1)
                {
                    throw new MBankException("Неверный СМС код. Оставшееся кол-во попыток: " + cnt.Substring(pos + 9, 1));
                }

                pos = cnt.IndexOf("УСПЕШНО ПРОВЕДЕНА!");
                if (pos != -1)
                {
                    pos = cnt.IndexOf("составляет", pos);
                    int pos2 = cnt.IndexOf("сум", pos);

                    if (pos != -1 && pos2 != -1)
                    {
                        pos += 11;
                        pos2 -= 1;
                        string res = cnt.Substring(pos, pos2 - pos);
                        return res;
                    }
                }
                else
                {
                    throw new MBankException("Не было получено подтверждение об оплате.");
                }
            }
            catch (MBankException mbe)
            {
                throw mbe;
            }
            catch (Exception e)
            {
                throw new MBankException("Сервис временно не доступен.", e);
            }

            return string.Empty;
        }

        private static async Task SubmintCheque(string command)
        {
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            filter.CookieUsageBehavior = HttpCookieUsageBehavior.Default;
            filter.AllowAutoRedirect = true;

            HttpClient client = new HttpClient(filter);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiUri);

            try
            {
                var val = await client.SendRequestAsync(request);

                foreach (var item in val.Headers)
                {
                    string s = item.ToString();
                    int i = s.IndexOf("sessionid=");
                    if (i > 0)
                    {
                        _sessionId = s.Substring(i + 10, 32);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new MBankException("Сервис временно не доступен.", e);
            }

            request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl + string.Format("/pay/{0}/", _merchantId)));
            request.Headers.Cookie.Add(new HttpCookiePairHeaderValue("sessionid", _sessionId));
            HttpStringContent content = new HttpStringContent(command + "&AJAX=1");
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded; charset=UTF-8");
            request.Content = content;

            try
            {
                var val = await client.SendRequestAsync(request);
                string response = await val.Content.ReadAsStringAsync();
                Response.Submint obj = JsonConvert.DeserializeObject<Response.Submint>(response);

                if (obj.ResponseStatus == "error")
                {
                    if (obj.Response.MerchantAmount != null)
                    {
                        throw new MBankException(obj.Response.MerchantAmount);
                    }

                    if (obj.Response.CardError != null)
                    {
                        throw new MBankException(obj.Response.CardError);
                    }

                    if (obj.Response.MerchantId != null)
                    {
                        throw new MBankException(obj.Response.MerchantId);
                    }

                    if (obj.Response.MerchantCode != null)
                    {
                        throw new MBankException(obj.Response.MerchantCode);
                    }

                    if (obj.Response.MerchantCardPhoneCode != null)
                    {
                        throw new MBankException("Номер телефона для СМС информирования неверный.");
                    }

                    throw new MBankException("Сервис вернул сообщение об ошибке.");
                }
            }
            catch (MBankException mbe)
            {
                throw mbe;
            }
            catch (Exception e)
            {
                throw new MBankException("Сервис временно не доступен.", e);
            }

            request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl + string.Format("/pay/{0}/", _merchantId)));
            request.Headers.Cookie.Add(new HttpCookiePairHeaderValue("sessionid", _sessionId));
            content = new HttpStringContent(command);
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            try
            {
                var val = await client.SendRequestAsync(request);

                _token = val.RequestMessage.RequestUri.LocalPath;
                foreach (var item in val.Headers)
                {
                    string s = item.ToString();
                    int i = s.IndexOf("token=");
                    if (i > 0)
                    {
                        _token = s.Substring(i + 6, 32);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new MBankException("Сервис временно не доступен.", e);
            }
        }
    }
}
