using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionPay.Payme
{
    using Newtonsoft.Json;
    using Windows.Security.Cryptography.Certificates;
    using Windows.Web.Http;
    using Windows.Web.Http.Filters;

    public static class PaymeHelper
    {
        private const string ApiUrl = "https://payme.uz/api";

        private static readonly Uri ApiUri = new Uri(ApiUrl);

        public async static Task<PaymeTransferData> SubmitTransfer(int amount, string sourceCardNumber, string sourceCardExpirationDate, string destinationCardNumber)
        {
            Response<Response.Transfer.SubmitData> data = await PostAsync<Response<Response.Transfer.SubmitData>>(
                Request.Transfer.GetSubmitData(amount * 100, sourceCardNumber, sourceCardExpirationDate, destinationCardNumber));
            if (data.Error != null)
            {
                throw new PaymeException(data.Error.Message);
            }

            PaymeTransferData res = new PaymeTransferData()
            {
                TransactionId = data.Result.Cheque.Id,
                DestinationCardOwner = data.Result.Info.Owner,
                Amount = data.Result.Cheque.Amount,
                Commission = data.Result.Cheque.Commission
            };

            Response<Response.Transfer.ConfirmData> data2 = await PostAsync<Response<Response.Transfer.ConfirmData>>(
                Request.Transfer.GetConfirmData(res.TransactionId));
            if (data2.Error != null)
            {
                throw new PaymeException(data2.Error.Message);
            }

            res.ConfirmationPhoneNumber = data2.Result.PhoneNumber;

            return res;
        }

        public async static Task ApproveTransfer(string transactionId, string smsCode)
        {
            Response<Response.Transfer.ApproveData> data = await PostAsync<Response<Response.Transfer.ApproveData>>(
                Request.Transfer.GetApproveData(transactionId, smsCode));
            if (data.Error != null)
            {
                throw new PaymeException(data.Error.Message);
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

            string header = "UnionPay";
            if (!request.Headers.UserAgent.TryParseAdd(header))
            {
                throw new PaymeException("Invalid header value: " + header);
            }

            HttpStringContent content = new HttpStringContent(requestCmd);
            content.Headers.ContentLength = (ulong)Encoding.UTF8.GetBytes(requestCmd).Length;
            request.Content = content;
            string response = string.Empty;

            try
            {
                response = (await client.SendRequestAsync(request)).Content.ToString();
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception)
            {
                throw new PaymeException("Сервис временно не доступен.");
            }
        }
    }
}
