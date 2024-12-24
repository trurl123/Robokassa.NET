namespace Robokassa.NET.Models;

public class RobokassaInvoiceRequest
{
    public RobokassaInvoiceRequest(decimal totalAmount, int invoiceId, string ipAddress, string email, string culture, string description, RobokassaReceiptRequest receipt = null, CustomShpParameters shpParameters = null)
    {
        TotalAmount = totalAmount;
        InvoiceId = invoiceId;
        IpAddress = ipAddress;
        Email = email;
        Culture = culture;
        Description = description;
        Receipt = receipt;
        ShpParameters = shpParameters;
    }

    public decimal TotalAmount { get; private set; }
    public int InvoiceId { get; private set; }
    public string IpAddress { get; private set; }
    public string Email { get; private set; }
    public string Culture { get; private set; }
    public string Description { get; private set; }
    public RobokassaReceiptRequest Receipt { get; private set; }
    public CustomShpParameters ShpParameters { get; private set; }
}