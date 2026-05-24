namespace AeroFuelHub.Web.ViewModels.FuelTransaction;

public class FuelTransactionHistoryViewModel
{
    public const int DefaultPageSize = 10;

    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = DefaultPageSize;
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public List<FuelTransactionListItemViewModel> Items { get; set; } = [];
}
