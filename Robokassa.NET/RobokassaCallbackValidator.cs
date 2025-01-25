using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Robokassa.NET.Exceptions;

namespace Robokassa.NET
{
    public class RobokassaCallbackValidator : IRobokassaPaymentValidator
    {
        private readonly RobokassaOptions options;

        public RobokassaCallbackValidator(RobokassaOptions options)
        {
            this.options = options;
        }

        public void CheckResult(
            string sumString,
            int invId,
            string signatureValue,
            bool fromBrowser,
            bool isTest,
            params KeyValuePair<string, string>[] shpParams)
        {
            if (invId == 0)
                throw new InvalidCallbackRequest(
                    JsonConvert.SerializeObject(
                        new
                        {
                            sumString, invId, signatureValue
                        }));
            var shpParamsStr = string.Join(
                ":",
                shpParams.OrderBy(x => x.Key)
                    .Select(x => $"{x.Key}={x.Value}"));
            var password =
                isTest
                    ? fromBrowser ? options.TestPassword1 : options.TestPassword2
                    : fromBrowser ? options.Password1 : options.Password2;
            var srcBase = $"{sumString}:{invId.ToString()}:{password}"
                          + (string.IsNullOrEmpty(shpParamsStr) ? "" : ":" + shpParamsStr);

            var srcMd5Hash = Md5HashService.GenerateMd5Hash(srcBase);

            if (!signatureValue.ToLower().Equals(srcMd5Hash))
                throw new InvalidSignatureException(signatureValue, srcMd5Hash);
        }
    }
}