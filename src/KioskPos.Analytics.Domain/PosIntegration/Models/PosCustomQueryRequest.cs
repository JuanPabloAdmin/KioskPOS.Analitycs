namespace KioskPos.Analytics.PosIntegration.Models;

public class PosCustomQueryRequest
{
    public string QueryIdentifier { get; set; } = string.Empty;
    public Dictionary<string, string> Parameters { get; set; } = new();
}

public class PosCustomQueryResult
{
    public bool Success { get; set; }
    public string? RawJson { get; set; }
    public List<Dictionary<string, object>>? Rows { get; set; }
    public string? ErrorMessage { get; set; }
}
