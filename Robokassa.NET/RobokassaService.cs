using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Robokassa.NET.Models;

namespace Robokassa.NET
{
    public class RobokassaService : IRobokassaService
    {
        private readonly RobokassaOptions _options;
        private readonly bool _isTestEnv;


        public RobokassaService(RobokassaOptions options, bool isTestEnv)
        {
            _isTestEnv = isTestEnv;
            _options = options;
        }


        public PaymentUrl GenerateAuthLink(RobokassaInvoiceRequest request)
        {
            var receiptEncodedJson = request.Receipt?.ToString();

            var customFieldsLine = request.ShpParameters?.ToString();

            var amountStr = request.TotalAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            
            var invoiceIdStr = request.InvoiceId.ToString();

            var signatureValue =
                Md5HashService
                    .GenerateMd5Hash(
                        PrepareMd5SumString(amountStr, invoiceIdStr, request.IpAddress, receiptEncodedJson, customFieldsLine));

            return new PaymentUrl(BuildPaymentLink(invoiceIdStr, amountStr, signatureValue, receiptEncodedJson, request));
        }

        private string BuildPaymentLink(
            string invoiceId,
            string amount,
            string signature,
            string receiptEncodedJson,
            RobokassaInvoiceRequest request)
        {
            const string host = "https://auth.robokassa.ru/Merchant/Index.aspx?";

            var parameters = new Collection<string>();

            if (_isTestEnv)
                parameters.Add("isTest=1");

            parameters.Add("MrchLogin=" + _options.ShopName);
            parameters.Add("InvId=" + invoiceId);
            parameters.Add("OutSum=" + amount);

            if (!string.IsNullOrEmpty(receiptEncodedJson))
                parameters.Add("Receipt=" + HttpUtility.UrlEncode(receiptEncodedJson));

            request.ShpParameters?
                .GetParameters?
                .ForEach(parameter =>
                    parameters.Add($"{parameter.Key}={HttpUtility.UrlEncode(HttpUtility.UrlEncode(parameter.Value))}"));

            parameters.Add("SignatureValue=" + signature);
            if (!string.IsNullOrEmpty(request.Culture))
                parameters.Add("Culture=" + HttpUtility.UrlEncode(request.Culture));
            if (!string.IsNullOrEmpty(request.Email))
                parameters.Add("Email=" + HttpUtility.UrlEncode(request.Email));
            if (!string.IsNullOrEmpty(request.IpAddress))
                parameters.Add("UserIp=" + HttpUtility.UrlEncode(request.IpAddress));
            if (!string.IsNullOrEmpty(request.Description))
                parameters.Add("Description=" + HttpUtility.UrlEncode(request.Description));
            parameters.Add("Encoding=utf-8");

            var url = host + string.Join("&", parameters);
            return url;
        }

        private string PrepareMd5SumString(
            string amount,
            string invoiceId,
            string ipAddress,
            string receiptEncodedJson,
            string customParameters)
        {
            var str = string.Join(":", new List<string>
            {
                _options.ShopName,
                amount,
                invoiceId,
                ipAddress,
                receiptEncodedJson,
                _options.Password1,
                customParameters,
            }.Where(x => !string.IsNullOrEmpty(x)));

            return str;
        }
    }
}