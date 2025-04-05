using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Robokassa.NET.Models;

namespace Robokassa.NET
{
    public class RobokassaService : IRobokassaService
    {
        private readonly RobokassaOptions options;

        public RobokassaService(RobokassaOptions options)
        {
            this.options = options;
        }

        public PaymentUrl GenerateAuthLink(RobokassaInvoiceRequest request)
        {
            var stringsFromRequest = GetStringsFromRequest(request);
            var signatureValue = Md5HashService.GenerateMd5Hash(PrepareMd5SumString(stringsFromRequest, request.IsTest));
            return new PaymentUrl(BuildPaymentLink(stringsFromRequest, signatureValue, request));
        }

        public (string signature, string sum, string shopName) GetSignature(RobokassaSignatureRequest request)
        {
            var stringsFromRequest = GetStringsFromRequest(request);
            return (Md5HashService.GenerateMd5Hash(PrepareMd5SumString(stringsFromRequest, request.IsTest)), 
                stringsFromRequest.AmountStr, options.ShopName);
        }

        private static StringsFromRequest GetStringsFromRequest(RobokassaSignatureRequest request)
        {
            var receiptEncodedJson = request.Receipt?.ToString();

            var customFieldsLine = request.ShpParameters?.ToString();

            var amountStr = request.TotalAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

            var invoiceIdStr = request.InvoiceId.ToString();
            return new StringsFromRequest(customFieldsLine, amountStr, invoiceIdStr, receiptEncodedJson, request.IpAddress);
        }

        private string BuildPaymentLink(
            StringsFromRequest strings,
            string signature,
            RobokassaInvoiceRequest request)
        {
            const string host = "https://auth.robokassa.ru/Merchant/Index.aspx?";

            var parameters = new Collection<string>();

            if (request.IsTest)
                parameters.Add("isTest=1");

            parameters.Add("MrchLogin=" + options.ShopName);
            parameters.Add("InvId=" + strings.InvoiceIdStr);
            parameters.Add("OutSum=" + strings.AmountStr);

            if (!string.IsNullOrEmpty(strings.ReceiptEncodedJson))
                parameters.Add("Receipt=" + HttpUtility.UrlEncode(strings.ReceiptEncodedJson));

            request.ShpParameters?
                .GetParameters?
                .ForEach(
                    parameter =>
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

        private string PrepareMd5SumString(StringsFromRequest stringsFromRequest, bool isTest)
        {
            var str = string.Join(
                ":",
                new List<string>
                {
                    options.ShopName,
                    stringsFromRequest.AmountStr,
                    stringsFromRequest.InvoiceIdStr,
                    stringsFromRequest.IpAddress,
                    stringsFromRequest.ReceiptEncodedJson,
                    isTest ? options.TestPassword1 : options.Password1,
                    stringsFromRequest.CustomFieldsLine,
                }.Where(x => !string.IsNullOrEmpty(x)));

            return str;
        }
    }
}