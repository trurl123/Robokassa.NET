namespace Robokassa.NET.Models;

public class RobokassaSignatureRequest
{
    public RobokassaSignatureRequest(decimal totalAmount, int invoiceId, string ipAddress, RobokassaReceiptRequest receipt = null, CustomShpParameters shpParameters = null)
    {
        TotalAmount = totalAmount;
        InvoiceId = invoiceId;
        IpAddress = ipAddress;
        Receipt = receipt;
        ShpParameters = shpParameters;
    }

    public decimal TotalAmount { get; private set; }
    public int InvoiceId { get; private set; }
    public string IpAddress { get; private set; }
    public RobokassaReceiptRequest Receipt { get; private set; }
    public CustomShpParameters ShpParameters { get; private set; }
}
public class RobokassaInvoiceRequest : RobokassaSignatureRequest
{
    public RobokassaInvoiceRequest(decimal totalAmount, int invoiceId, string ipAddress, string email, string culture, string description, RobokassaReceiptRequest receipt = null, CustomShpParameters shpParameters = null)
        : base(totalAmount, invoiceId, ipAddress, receipt, shpParameters)
    {
        Email = email;
        Culture = culture;
        Description = description;
    }

    public string Email { get; private set; }
    public string Culture { get; private set; }
    public string Description { get; private set; }
}