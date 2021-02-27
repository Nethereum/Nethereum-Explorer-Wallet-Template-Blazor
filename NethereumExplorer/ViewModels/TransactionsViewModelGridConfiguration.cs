using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;

namespace NethereumExplorer.ViewModels
{
    public class TransactionsViewModelGridConfiguration : IEntityTypeConfiguration<TransactionViewModel>
    {
        public void Configure(EntityTypeBuilder<TransactionViewModel> builder)
        {
            builder.Property(e => e.Changed).IsVisible(false);
            builder.Property(e => e.Data).IsVisible(false);
            builder.Property(e => e.Changing).IsVisible(false);
            builder.Property(e => e.BlockHash).IsVisible(false);
            builder.Property(e => e.ThrownExceptions).IsVisible(false);
            builder.Property(e => e.BlockNumber).IsVisible(false);
            builder.Property(e => e.GasPrice).IsVisible(false);
            builder.Property(e => e.Nonce).IsVisible(false);
            builder.Property(e => e.TransactionHash).HasValueFormatter(s => "{s}");
            builder.Property(e => e.Gas).IsVisible(false);
            builder.Property(e => e.Index).IsSortable();


            builder.UseCssClasses(conf =>
            {
                conf.Table = "grid-table";
                conf.TableBody = "grid-table-body";
                conf.TableCell = "grid-table-cell";
                conf.TableHeader = "grid-table-head";
                conf.TableHeaderCell = "grid-table-cell-head";
                conf.TableHeaderRow = "grid-table-head-row";
                conf.TableRow = "grid-table-row";
            });
        }
    }
}
