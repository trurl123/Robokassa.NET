using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Robokassa.NET.Exceptions;

namespace Robokassa.NET
{
    public class RobokassaCallbackValidator : IRobokassaPaymentValidator
    {
        private readonly string password1;
        private readonly string password2;


        public RobokassaCallbackValidator(string password1, string password2)
        {
            this.password1 = password1;
            this.password2 = password2;
        }

        public void CheckResult(
            string sumString,
            int invId,
            string signatureValue,
            bool fromBrowser,
            params KeyValuePair<string, string>[] shpParams)
        {
            if (invId == 0)
                throw new InvalidCallbackRequest(JsonConvert.SerializeObject(new
                {
                    sumString, invId, signatureValue
                }));
            var shpParamsStr = string.Join(":", shpParams.OrderBy(x=>x.Key)
                    .Select(x => $"{x.Key}={x.Value}"));
            var passwword = fromBrowser ? password1 : password2;
            var srcBase = $"{sumString}:{invId.ToString()}:{passwword}" 
                          + (string.IsNullOrEmpty(shpParamsStr) ? "" : ":" + shpParamsStr) ;

            var srcMd5Hash = Md5HashService.GenerateMd5Hash(srcBase);

            if (!signatureValue.ToLower().Equals(srcMd5Hash))
                throw new InvalidSignatureException(signatureValue, srcMd5Hash);
        }
    }
}