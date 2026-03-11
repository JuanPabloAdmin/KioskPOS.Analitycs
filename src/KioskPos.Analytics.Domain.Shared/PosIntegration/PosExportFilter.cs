namespace KioskPos.Analytics.PosIntegration;

/// <summary>
/// Filtros disponibles para exportación de datos del POS.
/// Mapeados a los valores de Agora: Invoices, SalesOrders, etc.
/// </summary>
[Flags]
public enum PosExportFilter
{
    None = 0,
    Invoices = 1,
    SalesOrders = 2,
    DeliveryNotes = 4,
    CashTransactions = 8,
    PosCloseOuts = 16,
    SystemCloseOuts = 32,
    PurchaseOrders = 64,
    IncomingDeliveryNotes = 128,
    PurchaseInvoices = 256,
    All = Invoices | SalesOrders | DeliveryNotes | CashTransactions | PosCloseOuts | SystemCloseOuts
}
