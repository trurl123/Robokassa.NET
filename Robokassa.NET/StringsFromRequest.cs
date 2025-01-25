namespace Robokassa.NET;

internal class StringsFromRequest
{
    public StringsFromRequest(string customFieldsLine, string amountStr, string invoiceIdStr, string receiptEncodedJson, string ipAddress)
    {
        CustomFieldsLine = customFieldsLine;
        AmountStr = amountStr;
        InvoiceIdStr = invoiceIdStr;
        ReceiptEncodedJson = receiptEncodedJson;
        IpAddress = ipAddress;
    }

    public string CustomFieldsLine { get; private set; }
    public string AmountStr { get; private set; }
    public string InvoiceIdStr { get; private set; }
    public string ReceiptEncodedJson { get; private set; }
    public string IpAddress { get; }
}