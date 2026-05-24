namespace AeroFuelHub.Web.ViewModels.Dashboard;

public class RecentTransactionViewModel
{
    public string TransactionNumber { get; set; }
        = string.Empty;

    public string Airline { get; set; }
        = string.Empty;

    public decimal TotalAmount { get; set; }

    public DateTime TransactionDate { get; set; }
}