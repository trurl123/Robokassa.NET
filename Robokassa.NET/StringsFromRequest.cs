namespace Robokassa.NET;

internal class StringsFromRequest
{
    public StringsFromRequest(string customFieldsLine, string amountStr, string invoiceIdStr, string receiptEncodedJson)
    {
        CustomFieldsLine = customFieldsLine;
        AmountStr = amountStr;
        InvoiceIdStr = invoiceIdStr;
        ReceiptEncodedJson = receiptEncodedJson;
    }

    public string CustomFieldsLine { get; private set; }
    public string AmountStr { get; private set; }
    public string InvoiceIdStr { get; private set; }
    public string ReceiptEncodedJson { get; private set; }
}