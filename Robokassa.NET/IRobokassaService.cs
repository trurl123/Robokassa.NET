using Robokassa.NET.Models;

namespace Robokassa.NET
{
    public interface IRobokassaService
    {
        PaymentUrl GenerateAuthLink(
            RobokassaInvoiceRequest request);
    }
}