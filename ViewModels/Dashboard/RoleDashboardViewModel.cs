namespace AeroFuelHub.Web.ViewModels.Dashboard;

public class RoleDashboardViewModel
{
    public string Title { get; set; }
        = string.Empty;

    public int TotalTransactions { get; set; }

    public decimal TotalFuelQuantity { get; set; }

    public decimal TotalRevenue { get; set; }

    public List<RecentTransactionViewModel>
        RecentTransactions
    { get; set; } = [];
}