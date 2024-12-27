using System.Collections.Generic;

namespace Robokassa.NET
{
    public interface IRobokassaPaymentValidator
    {
        void CheckResult(string sumString, int invId, string signatureValue, bool fromBrowser, params KeyValuePair<string, string>[] shpParams);
    }
}