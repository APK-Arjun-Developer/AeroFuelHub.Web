using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AeroFuelHub.Web.Services.Implementations;

public class ReportService : IReportService
{
    public byte[] GenerateInvoicePdf(FuelTransaction transaction)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(header =>
                {
                    header.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("AeroFuel Hub").FontSize(22).Bold().FontColor(Colors.Blue.Darken2);
                            col.Item().Text("Aviation Fuel Transaction Invoice").FontSize(12).FontColor(Colors.Grey.Darken1);
                        });
                        row.ConstantItem(150).AlignRight().Column(col =>
                        {
                            col.Item().Text($"Invoice #{transaction.TransactionNumber}").SemiBold();
                            col.Item().Text(transaction.TransactionDate.ToString("dd MMM yyyy HH:mm UTC"));
                        });
                    });
                    header.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                page.Content().PaddingVertical(20).Column(content =>
                {
                    content.Spacing(15);

                    content.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Airline").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.Airline?.Name ?? "—");
                            col.Item().PaddingTop(8).Text("Flight Number").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.FlightNumber);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Airport").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.Airport?.Name ?? "—");
                            col.Item().PaddingTop(8).Text("Fuel Company").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.FuelCompany?.Name ?? "—");
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Aircraft").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.Aircraft?.Model ?? "—");
                            col.Item().PaddingTop(8).Text("Status").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.Status.ToString());
                        });
                    });

                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Description").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Quantity (L)").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Rate (₹/L)").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Amount (₹)").FontColor(Colors.White).SemiBold();
                        });

                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                            .Text("Aviation Fuel Supply");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                            .Text(transaction.FuelQuantity.ToString("N2"));
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                            .Text(transaction.PricePerLiter.ToString("N2"));
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                            .Text(transaction.TotalAmount.ToString("N2"));

                        table.Cell().ColumnSpan(3).AlignRight().Padding(8).Text("Total Amount").SemiBold();
                        table.Cell().Padding(8).Text($"₹ {transaction.TotalAmount:N2}").SemiBold().FontSize(12);
                    });

                    if (!string.IsNullOrWhiteSpace(transaction.Remarks))
                    {
                        content.Item().Column(col =>
                        {
                            col.Item().Text("Remarks").SemiBold().FontColor(Colors.Grey.Darken1);
                            col.Item().Text(transaction.Remarks);
                        });
                    }
                });

                page.Footer().AlignCenter().Column(footer =>
                {
                    footer.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    footer.Item().PaddingTop(5).Text("Thank you for using AeroFuel Hub").FontSize(9).FontColor(Colors.Grey.Darken1);
                    footer.Item().Text("This is a system-generated invoice.").FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }
}
