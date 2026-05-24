namespace AeroFuelHub.Web.ViewModels.Dashboard;

public class AdminDashboardViewModel
{
    public int TotalTransactions { get; set; }

    public int TotalAirlines { get; set; }

    public int TotalFuelCompanies { get; set; }

    public int TotalAirports { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal TotalFuelQuantity { get; set; }

    public List<string> ChartLabels { get; set; } = [];

    public List<int> ChartData { get; set; } = [];

    public List<RecentTransactionViewModel> RecentTransactions { get; set; } = [];
}