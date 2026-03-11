namespace KioskPos.Analytics.PosIntegration;

/// <summary>
/// Tipos de pedido genéricos, mapeados desde el POS específico.
/// En Agora: Standard, Delivery, TakeAway, Table
/// </summary>
public enum PosOrderType
{
    Standard = 0,
    Table = 1,
    TakeAway = 2,
    Delivery = 3
}
