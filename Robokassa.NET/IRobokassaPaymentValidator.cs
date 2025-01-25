using System.Collections.Generic;

namespace Robokassa.NET
{
    public interface IRobokassaPaymentValidator
    {
        void CheckResult(string sumString, int invId, string signatureValue, bool fromBrowser, bool isTest, params KeyValuePair<string, string>[] shpParams);
    }
}